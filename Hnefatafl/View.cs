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
        public Matrix4 frustummatrix = Matrix4.Identity;
        public float fov = (float)(Math.PI) /4f, 
            ratio = 640f/480f, 
            znear = 0.01f, 
            zfar = 1000f;

      

        public View(Vector3 startposition, double startzoom = 1.0, double startrotation = 0.0) {
            this.position = startposition;
            this.zoom = startzoom;
            this.rotation = startrotation;


            Matrix4.CreatePerspectiveFieldOfView(fov, ratio, znear, zfar, out this.frustummatrix);/*new Matrix4(
                new Vector4( (1 / (float)Math.Tan(fov/2f))/ratio, 0, 0, 0),
                new Vector4( 0, (1 / (float)Math.Tan(fov/2f)), 0, 0),
                new Vector4( 0, 0, -(zfar + znear) / (zfar - znear), -(2 * znear * zfar / (zfar - znear))),
                new Vector4( 0, 0, -1, 0)

            ) ;*/

            this.viewmatrix = Matrix4.LookAt(this.position, new Vector3(0, 0, 0), Vector3.UnitY);
        }

        public void Update() {
            //this.position.Z += 0.01f;
            // this.position.X += 0.005f;
            //this.rotation = (float)Math.PI/2f;
        }

        public void ApplyTransform() {
            //TODO make rotation 3-axis
            Matrix4 transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, position.Z));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationY((float)-rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale((float)zoom, (float)zoom, 1.0f));


            GL.MultMatrix(ref transform);
            this.viewmatrix = transform;
        }

    }
}
