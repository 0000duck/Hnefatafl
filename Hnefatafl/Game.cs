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
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            view = new View(new Vector3(0,0,-10f), new Vector3(0,0,0), 1);
            objects = new List<GameObject>();

            TestObject testObj = new TestObject(new Vector3(3,0,0), Vector3.Zero);
            TestObject testObj2 = new TestObject(new Vector3(-0.5f,-0.5f,0f), Vector3.Zero);

            testObj.forceVector  = new Vector3(0, 0, 0);
            testObj2.forceVector = new Vector3(0, 0, 0.5f);

            testObj.Update = (obj) => {
                if (obj.position.X > 1 || obj.position.X < -1) {
                    obj.forceVector.X = -obj.forceVector.X;
                }

                obj.position += obj.forceVector;
                obj.rotation.Y += 0.05f;
                obj.rotation.X += 0.05f;

                return 0;
            };
            testObj2.Update = (obj) => {
                obj.rotation.Y += 0.05f;
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
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
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



    }
}
