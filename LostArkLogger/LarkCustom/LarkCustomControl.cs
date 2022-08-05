using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LostArkLogger.LarkCustom.UI;

namespace LostArkLogger.LarkCustom
{
    public static class LarkCustomControl
    {
        public static Parser Parser { get; set; }

        public static Entity User { get; set; }

        public static LarkCustomWindow Window { get; private set; }

        public static List<FrameData> frames = new List<FrameData>();

        public struct FrameData
        {
            public Frame frame;
            public int startX;
            public int startY;
        }

        public static void Run()
        {
            Init();
            Window = new LarkCustomWindow();
            Window.DrawAction += Draw;
            Application.Run(Window);
        }

        public static void Init()
        {
            // generate the frames
        }

        public static void Draw(Graphics g, int startX, int startY)
        {
            foreach (var frameData in frames)
            {
                frameData.frame.Draw(g, frameData.startX, frameData.startY);
            }
        }
    }
}
