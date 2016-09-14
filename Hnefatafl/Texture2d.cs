using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl {
    struct Texture2D {
        private int id;
        private int width, height;

        public int ID {
            get {
                return id;
            }
        }
        public int WIDTH {
            get {
                return width;
            }
        }
        public int HEIGHT {
            get {
                return height;
            }
        }

        public Texture2D(int id, int width, int height) {
            this.id = id;
            this.width = width;
            this.height = height;
        }

    }
}
