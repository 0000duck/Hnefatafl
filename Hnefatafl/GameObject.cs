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
    public class GameObject {

        public Vector3 position;
        public Vector3 forceVector;
        public Vector3 rotation;
        public double scale;
        public Matrix4 modelmatrix;
        public RenderingComponent Renderer;

        public Func<GameObject, int> Update {
            get; set;
        }



        public GameObject(Vector3 startposition,  Vector3 startrotation, double startscale = 1.0) {
            this.position = startposition;
            this.scale = startscale;
            this.rotation = startrotation;
            this.Renderer = new RenderingComponent(this);

        }

        public void PreUpdate() {

        }


        public void PostUpdate() {

        }

        public void ApplyTransform() {
            this.modelmatrix =
                Matrix4.CreateScale((float)this.scale) * Matrix4.CreateRotationX(this.rotation.X)
                * Matrix4.CreateRotationY(this.rotation.Y) * Matrix4.CreateRotationZ(this.rotation.Z)
                * Matrix4.CreateTranslation(this.position);
                
        }


        


    }
}
