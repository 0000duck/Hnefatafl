using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Hnefatafl {
    class GameObject {

        public Vector3 position;

        //in radians, + = clockwise
        public double rotation;

        public double scale;

        public Matrix4 modelmatrix;

        public RenderingComponent Renderer;





        public GameObject(Vector3 startposition, double startscale = 1.0, double startrotation = 0.0) {
            this.position = startposition;
            this.scale = startscale;
            this.rotation = startrotation;
            this.Renderer = new RenderingComponent(this);

        }

        public void PreUpdate() {

        }

        public void Update() {
            // this.position.X += 0.001f;
             //this.position.Z += 0.01f;
            //this.rotation += 0.01f;
        }

        public void PostUpdate() {

        }

        public void ApplyTransform() {
            this.modelmatrix = Matrix4.Identity;
            this.modelmatrix = Matrix4.Mult(this.modelmatrix, Matrix4.CreateTranslation(position.X, position.Y, position.Z));
            this.modelmatrix = Matrix4.Mult(this.modelmatrix, Matrix4.CreateRotationZ((float)-rotation));
            this.modelmatrix = Matrix4.Mult(this.modelmatrix, Matrix4.CreateScale((float)scale, (float)scale, 1.0f));
            
            GL.MultMatrix(ref this.modelmatrix);

        }



    }
}
