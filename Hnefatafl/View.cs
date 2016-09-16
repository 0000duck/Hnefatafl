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
        public Vector3 rotation;


        // 1 = no zoom
        // 2 = 2x zoom
        public double zoom;

        public Matrix4 viewmatrix;
        public Matrix4 frustummatrix = Matrix4.Identity;
        public float fov = (float)(Math.PI) /4f, 
            ratio = 640f/480f, 
            znear = 0.01f, 
            zfar = 1000f;

      

        public View(Vector3 startposition, Vector3 startrotation, double startzoom = 1.0 ) {
            this.position = startposition;
            this.zoom = startzoom;
            this.rotation = startrotation;


            Matrix4.CreatePerspectiveFieldOfView(fov, ratio, znear, zfar, out this.frustummatrix);
            
        }

        public void Update() {
            
        }

        public void ApplyTransform() {
            this.viewmatrix = Matrix4.CreateRotationX(this.rotation.X)
                * Matrix4.CreateRotationY(this.rotation.Y) * Matrix4.CreateRotationZ(this.rotation.Z)
                * Matrix4.CreateTranslation(this.position);

        }

    }
}
