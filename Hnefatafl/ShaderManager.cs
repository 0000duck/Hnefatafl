using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Hnefatafl {
    class ShaderManager {
        public static Dictionary<string, CustomShaderProgram> CompiledShaders {
            get;
            private set;
        } = new Dictionary<string, CustomShaderProgram>();

        public static int CreateShaderProgram(string vertexPath, string fragmentPath){
            int id = GL.CreateProgram();
            int vertid;
            int fragid;

            LoadShader(vertexPath, ShaderType.VertexShader, id, out vertid);
            LoadShader(fragmentPath, ShaderType.FragmentShader, id, out fragid);

            
            return id;

        }

        public static void LoadShader(string path, ShaderType type, int program, out int address) {
            if (!File.Exists("assets/shaders/" + path)) {
                Console.WriteLine("shader file not found at assets/shaders/" + path);
                throw new FileNotFoundException("shader file not found at assets/shaders/" + path);

            }

            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader("assets/shaders/" + path)) {
                String src = sr.ReadToEnd();
                GL.ShaderSource(address, src);
                //Console.WriteLine( "SHADER SROUCE:\n" + src );
            }

            GL.CompileShader(address);
            GL.AttachShader(program, address);
            int status = 9;
            GL.GetShader( address, ShaderParameter.CompileStatus, out status );
            if (status != 1) {
                Console.WriteLine( "{0} Compile failed: status code: {1}", path, status );
                string log;
                log = GL.GetShaderInfoLog(address);
                Console.WriteLine("\t" + log);

            }else {
                Console.WriteLine( "{0} Compiled successfully", path );

            }


        }

        public static void LinkProgram(CustomShaderProgram shaderprogram) {
            GL.LinkProgram(shaderprogram.ID);
        }


    }
    
}
