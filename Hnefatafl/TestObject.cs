using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Hnefatafl {
    public class TestObject : GameObject{

        public TestObject(Vector3 position, Vector3 rotation) : base(position, rotation) {
            Console.WriteLine(Directory.GetCurrentDirectory());
            this.Renderer.Mesh = Mesh.LoadFromObjFile(@"assets/t.obj");
            this.Renderer.Mesh.TextureHandle = Mesh.LoadTexture(@"assets/images/Cube1_auv.jpg");
            this.Renderer.Shader = ShaderManager.CompiledShaders.ContainsKey("BaseShader")? ShaderManager.CompiledShaders["BaseShader"] : new BaseShaderProgram("baseVertex.txt", "baseFrag.txt");
            this.Renderer.Shader.InitVBOs(this.Renderer.Mesh);
            

        }
    }
}
