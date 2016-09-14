using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Hnefatafl {
    public class Mesh {
        public Vector3[] Positions {
            get;
            set;
        }
        public Vector3[] Normals {
            get;
            set;
        }
        public Vector4[] Colors {
            get;
            set;
        }
        public Vector2[] TexCoords {
            get;
            set;
        }

    }
}
