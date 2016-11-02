using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Hnefatafl {
    public class RenderingComponent: Component {

        private int IBO;//index buffer

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
            InitIBO();


        }

        public override void Update() {
            Render();
        }

        private void InitIBO() {
            GL.GenBuffers( 1, out IBO );
            
            
        }
        
        private void Render() {
            this.Shader.Prepare();

            //unless dynamic mesh
            //GL.DeleteBuffer( IBO );
            //this.Shader.InitVBOs(this.Mesh);


            this.Shader.SetupVBOPointers( this.Mesh );




            Matrix4 pvm = this.parent.modelmatrix * Game.view.viewmatrix * Game.view.frustummatrix;

            //set the uniforms of the shader
            this.Shader.SetPVMMatrix( ref pvm );
            this.Shader.LoadUniforms( ref this.parent );


            //bind ibo & draw
            GL.BindBuffer( BufferTarget.ElementArrayBuffer, IBO );
            GL.BufferData( BufferTarget.ElementArrayBuffer,
                (IntPtr)(sizeof( int ) * this.Mesh.Indices.Length),
                this.Mesh.Indices,
                BufferUsageHint.StaticDraw );
            GL.DrawElements( PrimitiveType.Triangles, this.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            /*
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3( 0, 0, 0 );
            GL.Vertex3( 0, 1, 0 );
            GL.Vertex3( 1, 1, 0 );
            GL.End();
            //*/
            //cleanup
            this.Shader.EndRender();

        }
    
        //to make the model move along the position of the object
        public void ApplyTransforms() {
            
        }

        public override void Cleanup() {
            this.Shader.Cleanup();
            
            GL.DeleteBuffer(IBO);

            
        }

    }
}
