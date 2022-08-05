using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LostArkLogger.LarkCustom.UI;

namespace LostArkLogger.LarkCustom
{
    public static class Control
    {
        public static Parser Parser { get; set; }

        public static Entity User { get; set; }

        public static Entity[] PartyMembers { get; } = { User, null, null, null};

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
