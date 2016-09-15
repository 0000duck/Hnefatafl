using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Hnefatafl {
    class Game : GameWindow {

        public static View view;

        List<GameObject> objects;

        public Game (int width, int height) :base(width, height){

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            view = new View(new Vector3(0,0,0), 1, 0);
            objects = new List<GameObject>();

            

            TestObject testObj = new TestObject(Vector3.Zero);
            TestObject testObj2 = new TestObject(new Vector3(-0.5f,-0.5f,0f));

            testObj.forceVector = new Vector3(0.001f, 0, 0);
            testObj2.forceVector = new Vector3(-0.001f, 0, 0);

            testObj.Update = (obj) => {
                if (obj.position.X > 1 || obj.position.X < -1) {
                    obj.forceVector.X = -obj.forceVector.X;
                }

                obj.position += obj.forceVector;

                return 0;
            };
            testObj2.Update = (obj) => {
                if (obj.position.X > 1 || obj.position.X < -1) {
                    obj.forceVector.X = -obj.forceVector.X;
                }

                obj.position += obj.forceVector;

                return 0;
            };

            objects.Add(testObj);
            objects.Add(testObj2);
            

        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            

        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);


            view.Update();
            view.ApplyTransform();
            foreach (GameObject obj in objects) {
                obj.Update(obj);
                obj.ApplyTransform();
            }

        }
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            
            this.SetRenderMode3D();

            //GL.Viewport(0, 0, this.Width, this.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            

            foreach(GameObject obj in objects) {
                obj.Renderer.Update();

            }

            GL.Flush();
            this.SwapBuffers();

        }

        protected override void OnClosing( CancelEventArgs e ) {
            Console.WriteLine( "Cleaning up..." );
            base.OnClosing( e );

            foreach (GameObject obj in objects) {
                obj.Renderer.Cleanup();
            }
            GL.UseProgram( 0 );
            


        }

        public void SetRenderMode2D() {
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.Ortho(-1f, 1f, -1f, 1f, 1000f, -1000f);
        }

        public void SetRenderMode3D() {   
            // :(         
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref view.projectionmatrix);
            //GL.LoadIdentity();

            /*GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view.viewmatrix);
            GL.Viewport(0, 0, this.Width, this.Height);
            GL.LoadIdentity();//*/


        }

    }
}
