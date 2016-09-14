using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Hnefatafl {
    class View {

        public Vector3 position;
        
        //in radians, + = clockwise
        public double rotation;

        // 1 = no zoom
        // 2 = 2x zoom
        public double zoom;

        public Matrix4 viewmatrix;
        public Matrix4 frustummatrix;
        public float fov = 90f, 
            ratio = 640f/480f, 
            znear = 0.1f, 
            zfar = 1000f;

      

        public View(Vector3 startposition, double startzoom = 1.0, double startrotation = 0.0) {
            this.position = startposition;
            this.zoom = startzoom;
            this.rotation = startrotation;

            this.frustummatrix = new Matrix4(
                new Vector4((1 / (float)Math.Tan(fov)), 0, 0, 0),
                new Vector4(0, (ratio / (float)Math.Tan(fov)), 0, 0),
                new Vector4(0, 0, (zfar + znear) / (zfar - znear), 1),
                new Vector4(0, 0, -(2 * znear * zfar / (zfar - znear)), 0)
           ) ;
            Console.WriteLine(this.frustummatrix.ToString());
        }

        public void Update() {
            //this.position.Z += 0.01f;
           // this.position.X += 0.005f;
        }

        public void ApplyTransform() {
            Matrix4 transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, position.Z));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ((float)-rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale((float)zoom, (float)zoom, 1.0f));


            GL.MultMatrix(ref transform);
            this.viewmatrix = transform;
        }

    }
}
