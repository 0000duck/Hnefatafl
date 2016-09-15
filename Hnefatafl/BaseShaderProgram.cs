using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

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
            GL.BindAttribLocation(this.id, 1, "in_position");

            variablePipe.Add( "pvm", GL.GetUniformLocation( this.id, "pvm") );
            variablePipe.Add("i_color", GL.GetUniformLocation(this.id, "i_color"));


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

        public override void LoadUniforms( ref GameObject redneredObject) {
            Vector4 c = new Vector4(0f, 0.5f, 0.5f, 1f);

            GL.Uniform4(this.VariablePipe["i_color"], c);

        }

        public override void Prepare() {
            GL.UseProgram(this.id);

            this.BindVAO();

            //enable the various types of buffers
            GL.EnableClientState(ArrayCap.VertexArray);//MV PREPARE
            GL.EnableClientState(ArrayCap.NormalArray);//MV PREPARE

            GL.EnableVertexAttribArray(this.VariablePipe["in_position"]);
            GL.EnableVertexAttribArray(this.VariablePipe["in_normal"]);
        }

        public override void GenerateVBOs() {
            GL.GenBuffers(3, this.vbos);
        }

        public override void InitVBOs(Mesh mesh) {
            //positions
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[0]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(mesh.Positions.Length * Vector3.SizeInBytes),//pos.size
                mesh.Positions,//pos
                BufferUsageHint.StaticDraw);

            //normals
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(mesh.Normals.Length * Vector3.SizeInBytes),//norm.size
                mesh.Normals,//norm
                BufferUsageHint.StaticDraw);

            //texture
            /*GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)textures.Length,//tex.size
                textures,//tex
                BufferUsageHint.StaticDraw);*/

        }

        public override void SetupVBOPointers() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[0]);
            GL.VertexPointer(
                3,
                VertexPointerType.Float,
                0,
                0);
            GL.VertexAttribPointer(this.VariablePipe["in_position"], 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_position"]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[1]);
            GL.NormalPointer(
                NormalPointerType.Float,
                0,
                0
                );
            GL.VertexAttribPointer(this.VariablePipe["in_normal"], 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_normal"]);

            /*GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbos[2);
            GL.TexCoordPointer(2,
                TexCoordPointerType.Float,
                textures.Length,
                6);
            GL.VertexAttribPointer(this.VariablePipe["in_texCoord"], 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_texCoord"]);*/
        }


        public override void EndRender() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.DisableVertexAttribArray(this.VariablePipe["in_position"]);
            GL.DisableVertexAttribArray(this.VariablePipe["in_normal"]);
        }

        public override void Cleanup() {
            this.EndRender();

            GL.DeleteProgram(this.ID);

            GL.DeleteVertexArray(this.vao);
            GL.DeleteBuffers(this.vbos.Length, this.vbos);
        }

    }


}
