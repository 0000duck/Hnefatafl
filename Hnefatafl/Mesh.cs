using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Hnefatafl {
    public class Mesh {
        private const string
            VERTEX = "v ",
            NORMAL = "vn",
            TEXTURE = "vt",
            FACE = "f ",
            USEMTL = "usemtl ",
            MTLLIB = "mtllib ";

        public int TextureHandle {
            get;
            set;
        }
        public int VAO {
            get;
            set;
        }
        public int[] VBOs {
            get;
            set;
        }

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
        public Material Material;




        public static Mesh LoadFromObjFile( string filepath ) {
            StreamReader file = new StreamReader( filepath );
            string line;
            Mesh mesh = new Mesh();
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<int> indices = new List<int>();
            List<int> texIndices = new List<int>();
            List<int> normIndices = new List<int>();

            while ((line = file.ReadLine()) != null) {
                if (line.StartsWith( VERTEX )) {
                    string[] parts = line.Split( ' ' );
                    float x = float.Parse( parts[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    Vector3 vertex = new Vector3( x, y, z );
                    positions.Add( vertex );

                } else if (line.StartsWith( TEXTURE )) {
                    string[] parts = line.Split( ' ' );
                    float u = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    float v = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    Vector2 uv = new Vector2( u, v );
                    textures.Add( uv );

                } else if (line.StartsWith( NORMAL )) {
                    string[] parts = line.Split( ' ' );
                    float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    Vector3 normal = new Vector3( x, y, z );
                    normals.Add( normal );
                } else if (line.StartsWith( FACE )) {
                    string[] parts = line.Split( ' ' );
                    for(int i = 1; i < 4; i++) {
                        string[] rawFace = parts[i].Split( '/' );
                        int index;
                        int tindex;
                        int nindex;
                        switch (rawFace.Length) {
                            case 1:
                                index = int.Parse( rawFace[0] );
                                indices.Add( index-1 );
                                break;
                            case 2:
                                index = int.Parse( rawFace[0] );
                                indices.Add( index-1 );
                                if (rawFace[1] == "") {
                                    tindex = 0;
                                }else {
                                    tindex = int.Parse( rawFace[1] );
                                }
                                texIndices.Add( tindex-1 );
                                break;
                            case 3:
                                index = int.Parse( rawFace[0] );
                                indices.Add( index-1 );
                                if (rawFace[1] == "") {
                                    tindex = 0;
                                } else {
                                    tindex = int.Parse( rawFace[1] );
                                }
                                texIndices.Add( tindex-1 );
                                
                                if (rawFace[2] == "") {
                                    nindex = 0;
                                } else {
                                    nindex = int.Parse( rawFace[2] );
                                }
                                normIndices.Add( nindex-1 );
                                break;
                        }
                    }

                }
            }
            file.Close();

            mesh.Positions = new Vector3[indices.Count()];
            mesh.TexCoords = new Vector2[indices.Count()];
            mesh.Normals = new Vector3[indices.Count()];

            int it = 0;
            foreach(int i in indices) {
                mesh.Positions[it] = positions[i];
                it++;
            }
            it = 0;
            foreach (int i in texIndices) {
                mesh.TexCoords[it] = textures[i];
                it++;
            }
            it = 0;
            foreach (int i in normIndices) {
                mesh.Normals[it] = normals[i];
                it++;
            }

            return mesh;
        }

        public static int LoadTexture(string filepath ) {
            Bitmap texture = new Bitmap( filepath );
            int textureHandle;
            GL.GenTextures(1, out textureHandle);
            int maxfilter = (int)TextureMagFilter.Linear;
            int minfilter = (int)TextureMinFilter.Linear;
            int wrapS = (int)TextureWrapMode.Repeat;
            int wrapT = (int)TextureWrapMode.Repeat;

            GL.Enable( EnableCap.Texture2D );
            GL.TexEnv( TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate );
            GL.BindTexture( TextureTarget.Texture2D, textureHandle );
            GL.TexParameterI( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref maxfilter );
            GL.TexParameterI( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref minfilter );
            GL.TexParameterI( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref wrapS );
            GL.TexParameterI( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref wrapT );

            System.Drawing.Imaging.BitmapData data = texture.LockBits( new System.Drawing.Rectangle( 0, 0, texture.Width, texture.Height ),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexSubImage2D( TextureTarget.Texture2D, 0, 0, 0, data.Width, data.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0 );

            texture.UnlockBits( data );
            texture.Dispose();

            GL.GenerateMipmap( GenerateMipmapTarget.Texture2D );

            GL.Disable( EnableCap.Texture2D );

            return textureHandle;

        }

        
    }
    
}
