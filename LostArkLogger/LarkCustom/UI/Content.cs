using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace LostArkLogger.LarkCustom.UI
{
    public abstract class Content
    {
        public const int FrameEdgeBuffer = 15;

        public Frame? ParentWindow { get; private set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public abstract int MinWidth { get; }
        public abstract int MinHeight { get; }

        public abstract void Draw(Graphics g, int StartX, int StartY);
        public abstract void Reframe(int width, int height);

        public abstract void OnMouseClick(int startX, int startY, MouseButtons button);

        public abstract void OnMouseDoubleClick(int startX, int startY, MouseButtons button);

        public abstract void OnMouseDown(int startX, int startY, MouseButtons button);
        public abstract void OnMouseUp(int startX, int startY, MouseButtons button);
        public abstract void OnMouseMove(int startX, int startY);

        public abstract void OnMouseHover(int startX, int startY);

        public abstract void OnMouseScroll(int startX, int startY, int Delta);
    }
}
