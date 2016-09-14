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

        public BaseShaderProgram(string vertpath, string fragpath): base(vertpath, fragpath) {
            this.vbos = new int[1];
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

        }

        public override void setPVMMatrix( ref Matrix4 pvm ) {
            //GL.UniformMatrix4( variablePipe["pvm"], false, ref pvm );

        }

        public override void Prepare() {
            ShaderManager.LinkProgram( this );
            GL.UseProgram(this.id);


            GL.GenVertexArrays(1, out this.vao);
            GL.GenBuffers(1, out this.vbos[0]);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbos[0]);



        }

        public override void PostMeshAttribution() {
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);


        }

        public override void Cleanup() {
            GL.DisableVertexAttribArray(0);

            GL.DeleteProgram(this.ID);
            GL.DeleteVertexArray(this.vao);
            
            foreach( int vbo in vbos) {
                GL.DeleteBuffer(vbo);
            }
        }

    }


}
