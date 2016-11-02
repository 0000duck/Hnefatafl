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

namespace Hnefatafl {
    public class Mesh {
        private const string
            VERTEX = "v ",
            NORMAL = "vn",
            TEXTURE = "vt",
            FACE = "f ";

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

        public int[] Indices {
            get;
            set;
        }
        public int[] TextureIndices {
            get;
            set;
        }
        public int[] NormalIndices {
            get;
            set;
        }

        public Vector3[] OrderedVertices {
            get;
            set;
        }
        public Vector2[] OrderedTexCoords {
            get;
            set;
        }

        public Vertex[] Vertices {
            get;set;
        }

        public void OrderVertices() {
            /*OrderedVertices = new Vector3[Indices.Length];
            OrderedTexCoords = new Vector2[Indices.Length];
            Vertices = new Vertex[Positions.Length];
            int index = 0;
            for (int i = 0; i < TextureIndices.Count(); i++) {
                //Console.WriteLine( "b" + TexCoords[TextureIndices[i]] );
                if (TextureIndices[i] >= 0) {
                    OrderedTexCoords[i] = TexCoords[TextureIndices[i]];
                } else {
                    OrderedTexCoords[i] = new Vector2( 0, 1 );
                }

            }

            for (int i = 0; i < Positions.Length; i++){
                Vertex v = new Vertex();
                v.Position = Positions[i];
                v.TexCoord = TexCoords[i];
                Vertices[i] = v;
            }

            OrderedVertices = new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(0,1,0)
            };

            OrderedTexCoords = new Vector2[]{
                new Vector2( 0, 0 ),
                new Vector2( 1, 0 ),
                new Vector2( 0, 1 )
            };*/
        }

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
                    float x = (float)Decimal.Parse( parts[1], System.Globalization.NumberStyles.Float );
                    float y = (float)Decimal.Parse( parts[2], System.Globalization.NumberStyles.Float );
                    float z = (float)Decimal.Parse( parts[3], System.Globalization.NumberStyles.Float );
                    Vector3 vertex = new Vector3( x, y, z );
                    positions.Add( vertex );

                } else if (line.StartsWith( TEXTURE )) {
                    string[] parts = line.Split( ' ' );
                    float u = (float)Decimal.Parse( parts[1], System.Globalization.NumberStyles.Float );
                    float v = (float)Decimal.Parse( parts[2], System.Globalization.NumberStyles.Float );
                    Vector2 uv = new Vector2( u, v );
                    textures.Add( uv );

                } else if (line.StartsWith( NORMAL )) {
                    string[] parts = line.Split( ' ' );
                    float x = (float)Decimal.Parse( parts[1], System.Globalization.NumberStyles.Float );
                    float y = (float)Decimal.Parse( parts[2], System.Globalization.NumberStyles.Float );
                    float z = (float)Decimal.Parse( parts[3], System.Globalization.NumberStyles.Float );
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
                                    //Console.WriteLine( textures[tindex - 1] );
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

            mesh.Positions = positions.ToArray();
            mesh.TexCoords = textures.ToArray();
            mesh.Normals = normals.ToArray();
            mesh.Indices = indices.ToArray();
            mesh.NormalIndices = normIndices.ToArray();
            mesh.TextureIndices = texIndices.ToArray();

            //mesh.OrderVertices();
            foreach (Vector3 v in mesh.Positions) {
                Console.WriteLine( v );
            }
            foreach(int i in indices) {
                Console.WriteLine("o" + mesh.Positions[i] );
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

    [StructLayout( LayoutKind.Explicit )]
    public struct Vertex {
        [FieldOffset(0)]public Vector3 Position;
        [FieldOffset(12)]public Vector2 TexCoord;

    }
}
