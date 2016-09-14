using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Hnefatafl{
    class Hnefatafl{
        public static void Main(){
            Game window = new Game(640, 480);
            window.Run();

        }
    }
}