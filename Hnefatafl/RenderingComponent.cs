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

        private int VBO;
        private int TBO;
        private int NBO;

        public CustomShaderProgram Shader {
            get;
            set;
        }
        public  Mesh Mesh {
            get;
            set;
        }


        public RenderingComponent(GameObject parent) : base("RenderingComponent", parent) {

        }

        public override void Update() {
            Render();
        }

        public void Init() {
            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(Vector3.SizeInBytes * Mesh.Positions.Length),
                (IntPtr)null,
                BufferUsageHint.DynamicDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)0,
                (IntPtr)(Vector3.SizeInBytes * Mesh.Positions.Length),
                Mesh.Positions
                );

            GL.GenBuffers(1, out TBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, TBO);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(Vector2.SizeInBytes * Mesh.TexCoords.Length),
                (IntPtr)null,
                BufferUsageHint.DynamicDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)0,
                (IntPtr)(Vector2.SizeInBytes * Mesh.TexCoords.Length),
                Mesh.TexCoords
                );

            GL.GenBuffers(1, out NBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NBO);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(Vector3.SizeInBytes * Mesh.Normals.Length),
                (IntPtr)null,
                BufferUsageHint.DynamicDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)0,
                (IntPtr)(Vector3.SizeInBytes * Mesh.Normals.Length),
                Mesh.Positions
                );

        }
        
        private void Render() {
            this.Shader.Prepare(this.Mesh, VBO, TBO, NBO);

            Matrix4 pvm = this.parent.modelmatrix * Game.view.viewmatrix * Game.view.frustummatrix;
            this.Shader.SetPVMMatrix( ref pvm );
            this.Shader.LoadUniforms( ref this.parent );

            GL.DrawArrays(PrimitiveType.Triangles, 0, Mesh.Positions.Length);


            this.Shader.EndRender();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


        }

        //to make the model move along the position of the object
        public void ApplyTransforms() {
            
        }

        public override void Cleanup() {
            this.Shader.Cleanup();
            
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(TBO);

        }

    }
}
