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

        private int IBO;

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
            


        }

        public override void Update() {
            Render();
        }
        
        private void Render() {
            //TODO Move bloc A to Shader

            //this.Shader.Prepare(); //TO REDO
            this.Shader.Prepare();
            

            //this.Shader.InitVBOs(this.Mesh);


            //Index Buffer Object
            GL.GenBuffers(1, out IBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer,
                (IntPtr)(sizeof(int)*this.Mesh.Indices.Length),
                this.Mesh.Indices,
                BufferUsageHint.StaticDraw);

            this.Shader.SetupVBOPointers();
            
            Matrix4 pvm = /*Game.view.frustummatrix **/ Game.view.viewmatrix * this.parent.modelmatrix;

            //set the uniforms of the shader
            this.Shader.SetPVMMatrix( ref pvm );
            this.Shader.LoadUniforms( ref this.parent );

            

            

            //bind ibo & draw
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.DrawElements(PrimitiveType.Triangles, this.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, positions.Length);

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
