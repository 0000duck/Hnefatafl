using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Hnefatafl {
    class RenderingComponent: Component {

        private int[] VBO;
        private int IBO;
        Vector3[] positions,
            normals;
        Vector2[] textures;

 

        public CustomShaderProgram Shader {
            get;
            set;
        }
        public  Mesh Mesh {
            get;
            set;
        }


        public RenderingComponent(GameObject parent) : base("RenderingComponent", parent) {
            this.Mesh = new Mesh();
            positions = new Vector3[]{
                new Vector3( 0.0f, 0.0f, 0.0f ),
                new Vector3( 1.0f, 0.0f, 0.0f ),
                new Vector3( 0.5f, 1.0f, 0.0f )
            };
            normals = new Vector3[]{
                new Vector3( 0.0f, 0.0f, 1.0f ),
                new Vector3( 0.0f, 1.0f, 0.0f ),
                new Vector3( 1.0f, 0.0f, 0.0f )
            };
            textures = new Vector2[]{
                new Vector2( 0.0f, 0.0f ),
                new Vector2( 1.0f, 0.0f ),
                new Vector2( 0.0f, 1.0f )
            };


        }

        public override void Update() {
            RenderIBO();
        }

        [Obsolete("Render is Obsolete, use RenderIBO instead.")]
        public void Render() {
            this.Shader.Prepare();

        

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(this.Mesh.Positions.Length * Vector3.SizeInBytes), this.Mesh.Positions, BufferUsageHint.StaticDraw);
            
            
            

            Vector4 c = new Vector4(1f, 0f, 0f, 1f);
            Matrix4 pvm = /*Game.view.frustummatrix */ Game.view.viewmatrix * this.parent.modelmatrix; //Matrix4.Mult(Game.view.projectionmatrix, Matrix4.Mult(Game.view.viewmatrix, this.parent.modelmatrix));

            GL.UniformMatrix4(this.Shader.VariablePipe["pvm"], false, ref pvm );
            GL.Uniform4( this.Shader.VariablePipe["i_color"], c );

            //Console.WriteLine("m: " + this.Shader.VariablePipe["pvm"]);
            //Console.WriteLine( pvm.ToString()+"\n");

            this.Shader.PostMeshAttribution();

            GL.DrawArrays(PrimitiveType.Quads, 0, this.Mesh.Positions.Length);
            
        }
        
        public void RenderIBO() {
            //TODO

            //this.Shader.Prepare();
            int[] pindices = new int[3];
            pindices[0] = 0;
            pindices[1] = 1;
            pindices[2] = 2;

            int vao;

            GL.GenVertexArrays(1, out vao );

            

            //enable the various types of buffers
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            //GL.EnableClientState(ArrayCap.TextureCoordArray);

            VBO = new int[3];
            //Vertex Buffer Objects
            GL.GenBuffers(3, VBO);
            GL.BindVertexArray(vao);
            //TODO split MyVertex into 3 arrays
            //positions
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, 
                (IntPtr)(positions.Length * Vector3.SizeInBytes),//pos.size
                positions,//pos
                BufferUsageHint.StaticDraw);

            //normals
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(normals.Length * Vector3.SizeInBytes),//norm.size
                normals,//norm
                BufferUsageHint.StaticDraw);

            //texture
            /*GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)textures.Length,//tex.size
                textures,//tex
                BufferUsageHint.StaticDraw);*/



            //Index Buffer Object
            GL.GenBuffers(1, out IBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                (IntPtr)(sizeof(int)*pindices.Length),
                pindices,
                BufferUsageHint.StaticDraw);

            //bind & config VBOs
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.VertexPointer(
                3,
                VertexPointerType.Float,
                0,
                0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.NormalPointer(
                NormalPointerType.Float,
                0,
                0
                );
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);


            /*GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2);
            GL.TexCoordPointer(2,
                TexCoordPointerType.Float,
                textures.Length,
                6);*/
            Vector4 c = new Vector4(0f, 0.5f, 0.5f, 1f);
            Matrix4 pvm = /*Game.view.frustummatrix **/ Game.view.viewmatrix * this.parent.modelmatrix; //Matrix4.Mult(Game.view.projectionmatrix, Matrix4.Mult(Game.view.viewmatrix, this.parent.modelmatrix));

            GL.UniformMatrix4(this.Shader.VariablePipe["pvm"], false, ref pvm);
            GL.Uniform4(this.Shader.VariablePipe["i_color"], c);

            //this.Shader.PostMeshAttribution();
            

            

            //bind ibo & draw
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.DrawElements(PrimitiveType.Triangles, pindices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, positions.Length);

            //cleanup
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            
            //GL.DisableClientState(ArrayCap.NormalArray);
            //GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
    
        //to make the model move along the position of the object
        public void ApplyTransforms() {
            
        }

        public override void Cleanup() {
            this.Shader.Cleanup();
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DeleteBuffers(VBO.Length, VBO);
            GL.DeleteBuffer(IBO);

            
        }

    }
}
