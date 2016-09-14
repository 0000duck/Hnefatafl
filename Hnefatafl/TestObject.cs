using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Hnefatafl {
    public class TestObject : GameObject{

        public TestObject(Vector3 position) : base(position) {
            this.Renderer.Mesh.Positions = new Vector3[]{
                new Vector3( 0.0f, 0.0f, 0.0f ),
                new Vector3( 1.0f, 0.0f, 0.0f ),
                new Vector3( 0.5f, 1.0f, 0.0f )

            };
            this.Renderer.Mesh.Normals = new Vector3[]{
                new Vector3(0.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(1.0f, 0.0f, 0.0f)
            };

            this.Renderer.Shader = new BaseShaderProgram("baseVertex.txt", "baseFrag.txt");
        }
    }
}
