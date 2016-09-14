using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl {
    abstract class Component {
        private static int COMPONENT_UID = 0;

        protected GameObject parent;

        public string Name {
            get;
            private set;
        }
        public int ID {
            get;
            private set;
        }

        public Component(string name, GameObject parent) {
            this.Name = name;
            this.ID = COMPONENT_UID;
            this.parent = parent;
            COMPONENT_UID++;

        }

        public abstract void Update();
        public abstract void Cleanup();

    }
}
