using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Hnefatafl {
    class BaseShaderProgram : CustomShaderProgram{

        public BaseShaderProgram(string vertpath, string fragpath): base(vertpath, fragpath, "BaseShader") {
            this.vbos = new int[3];

            this.Init();
            
        }

        

        public override void InitVariablePipe() {

            variablePipe.Add("in_position", 0);
            GL.BindAttribLocation( this.id, 0, "in_position" );
            variablePipe.Add("in_normal", 1);
            GL.BindAttribLocation(this.id, 1, "in_normal");
            variablePipe.Add( "in_uv", 2 );
            GL.BindAttribLocation(this.id, 2, "in_uv");

            variablePipe.Add( "pvm", GL.GetUniformLocation( this.id, "pvm") );
            variablePipe.Add( "sampler", GL.GetUniformLocation( this.id, "sampler" ) );

        }


        public override void Init() {
            ShaderManager.LinkProgram(this);
            GL.UseProgram(this.id);

            InitVariablePipe();



            this.GenerateVAO();
            this.GenerateVBOs();
             
        }

        public override void SetPVMMatrix( ref Matrix4 pvm ) {
            GL.UniformMatrix4( variablePipe["pvm"], false, ref pvm );

        }

        public override void LoadUniforms( ref GameObject renderedObject) {

            GL.Uniform1( variablePipe["sampler"], 0f );
            //GL.BindBuffer( BufferTarget.ArrayBuffer, this.vbos[0] );

        }

        public override void Prepare() {
            GL.UseProgram(this.id);

            this.BindVAO();

            //enable the various types of buffers
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState( ArrayCap.TextureCoordArray );
            GL.Enable( EnableCap.Texture2D );

            GL.EnableVertexAttribArray( this.VariablePipe["in_position"] );
            GL.EnableVertexAttribArray( this.VariablePipe["in_normal"] );
            GL.EnableVertexAttribArray( this.VariablePipe["in_uv"] );
        }

        public override void GenerateVBOs() {
            GL.GenBuffers(3, this.vbos);
            
        }

        public override void InitVBOs(Mesh mesh) {
            this.Prepare();
            GL.BindBuffer( BufferTarget.ArrayBuffer, this.vbos[0] );
            GL.BufferData( BufferTarget.ArrayBuffer, 
                (IntPtr)mesh.Positions.Length, 
                mesh.Positions, 
                BufferUsageHint.StaticDraw
                );

            //texture
            GL.BindBuffer( BufferTarget.ArrayBuffer, this.vbos[2] );
            GL.BufferSubData( BufferTarget.ArrayBuffer,
                (IntPtr)(mesh.Positions.Length * Vector3.SizeInBytes),
                (IntPtr)(mesh.TexCoords.Length * Vector2.SizeInBytes),//tex.size
                mesh.TexCoords//tex
                );

            //normals
            //GL.BindBuffer( BufferTarget.ArrayBuffer, this.vbos[1] );
            /*GL.BufferData( BufferTarget.ArrayBuffer,
                (IntPtr)(mesh.Normals.Length * Vector3.SizeInBytes),//norm.size
                mesh.Normals,//norm
                BufferUsageHint.StaticDraw );
                */


            GL.VertexAttribPointer( this.VariablePipe["in_position"], 3, VertexAttribPointerType.Float, false, 0, 0 );
            GL.VertexAttribPointer( this.VariablePipe["in_normal"], 3, VertexAttribPointerType.Float, false, 0, 0 );
            GL.VertexAttribPointer( this.VariablePipe["in_uv"], 2, VertexAttribPointerType.Float, false, 0, 0 );

        }

        public override void SetupVBOPointers(Mesh mesh) {
            

            //GL.ActiveTexture( TextureUnit.Texture0 );
            //GL.BindTexture( TextureTarget.Texture2D, mesh.TextureHandle );
            GL.EnableVertexAttribArray( this.VariablePipe["in_position"] );
            GL.EnableVertexAttribArray( this.VariablePipe["in_normal"] );
            GL.EnableVertexAttribArray( this.VariablePipe["in_uv"] );

           // GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[0]);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[1]);
            /*GL.NormalPointer(
                NormalPointerType.Float,
                0,
                0
                );
            GL.EnableClientState( ArrayCap.NormalArray );
            */

            IntPtr texCoordOffset= Marshal.OffsetOf<Vertex>( "TexCoord" );
            IntPtr positionOffset= Marshal.OffsetOf<Vertex>( "Position" );
            int vertexStructSize = Marshal.SizeOf<Vertex>();
            /*
            GL.TexCoordPointer(
                2,
                TexCoordPointerType.Float,
                vertexStructSize,
                texCoordOffset
                );*/
            /*GL.VertexPointer(
                3,
                VertexPointerType.Float,
                vertexStructSize,
                positionOffset
                );*/


            //GL.BindBuffer( BufferTarget.ArrayBuffer, 0 );
            
        }


        public override void EndRender() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState( ArrayCap.TextureCoordArray );
            GL.Disable( EnableCap.Texture2D );

            GL.DisableVertexAttribArray(this.VariablePipe["in_position"]);
            GL.DisableVertexAttribArray(this.VariablePipe["in_normal"]);
            GL.DisableVertexAttribArray( this.VariablePipe["in_uv"] );

        }

        public override void Cleanup() {
            this.EndRender();

            GL.DeleteProgram(this.ID);

            GL.DeleteVertexArray(this.vao);
            GL.DeleteBuffers(this.vbos.Length, this.vbos);
        }

    }


}
