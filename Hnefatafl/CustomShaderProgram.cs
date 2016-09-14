using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Hnefatafl {
    abstract class CustomShaderProgram {
        protected int id;
        protected Dictionary<string, int> variablePipe;
        protected Matrix4 pvm;
        protected int vao;
        protected int[] vbos;


        public int ID {
            get {
                return id;
            }
        }
        public Dictionary<string, int> VariablePipe {
            get {
                return variablePipe;
            }
            set {
                variablePipe = value;
            }
        }

        public CustomShaderProgram(string vertexpath, string fragpath) {
            this.id = ShaderManager.CreateShaderProgram(vertexpath, fragpath);
            this.variablePipe = new Dictionary<string, int>();

            Init();

        }

        public CustomShaderProgram(int id) {
            this.id = id;
            this.variablePipe = new Dictionary<string, int>();

            Init();
        }


        public abstract void setPVMMatrix( ref Matrix4 pvm );
        public abstract void InitVariablePipe();
        public abstract void Init();
        public abstract void Prepare();
        public abstract void PostMeshAttribution();
        public abstract void Cleanup();

    }
}
