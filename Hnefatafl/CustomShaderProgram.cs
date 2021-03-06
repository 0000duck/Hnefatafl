﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Hnefatafl {
    public abstract class CustomShaderProgram {
        protected int id;
        protected Dictionary<string, int> variablePipe;
        protected Matrix4 pvm;
        protected int vao;
        protected int[] vbos;

        public string Name {
            get;
            private set;
        }


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

        public CustomShaderProgram(string vertexpath, string fragpath, string name){

            if (ShaderManager.CompiledShaders.ContainsKey(name)) {
                throw new Exception("Shader program with the name "+name+" already exists.");
            }
            this.id = ShaderManager.CreateShaderProgram(vertexpath, fragpath);
            this.Name = name;
            this.variablePipe = new Dictionary<string, int>();
            ShaderManager.CompiledShaders.Add(this.Name, this);

        }

        public CustomShaderProgram(int id) {
            this.id = id;
            this.variablePipe = new Dictionary<string, int>();

            Init();
        }


        public abstract void SetPVMMatrix( ref Matrix4 pvm );
        public abstract void LoadUniforms( ref GameObject renderedObject );
        public abstract void InitVariablePipe();
        public abstract void Init();
        public abstract void Prepare(Mesh m, int VBO, int TBO, int NBO);
        public abstract void Cleanup();
        public abstract void EndRender();

        public abstract void GenerateVBOs();
        public abstract void InitVBOs(Mesh m);
        public virtual void GenerateVAO() {
            GL.GenVertexArrays(1, out this.vao);
        }
        public virtual void BindVAO() {
            GL.BindVertexArray(this.vao);
        }
    }
}
