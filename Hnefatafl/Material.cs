using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl {
    public class Material {
        public enum KEYWORDS {
            NEWMTL,
            KA,
            KD,
            KS,
            NS,
            D,//unused
            TR,//unused
            MAP_KA,//unused
            MAP_KD,
            MAP_KS//unused

        }

        public string Name;
        public Vector3 AmbientColor;
        public Vector3 DiffuseColor;
        public Vector3 SpecularColor;
        public int SpecularExponent;
        public float Dissolved;//transparency
        public float Transparency;// 1 - Dissolve
        public string Map_Kd;

        public static Material LoadMaterialFromFile(string filepath) {
            Material material = new Material();
            StreamReader file = new StreamReader(filepath);
            string line;

            while ((line = file.ReadLine()) != null) {
                if (line.ToUpper().StartsWith(KEYWORDS.KD.ToString())) {
                    string[] parts = line.Split(' ');
                    material.DiffuseColor = new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]) );
                } else if (line.ToUpper().StartsWith(KEYWORDS.MAP_KD.ToString())) {
                    material.Map_Kd = line.Split(' ')[1];
                }
            }

            return material;
        }

    }
}
