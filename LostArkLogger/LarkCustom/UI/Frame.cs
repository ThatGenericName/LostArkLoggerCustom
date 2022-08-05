using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.LarkCustom.UI
{
    public class Frame : Content
    {
        public const int FrameEdgeBuffer = 15;
        public int FrameWidth { get; protected set; }
        public int FrameHeight { get; protected set; }

        public override int MinHeight => Content.MinHeight;

        public override int MinWidth => Content.MinWidth;


        private Content _content;
        public Content Content 
        { 
            get => _content; 
            set 
            {
                value.Reframe(Width, Height);
                _content = value;
            }
        }

        public override void Draw(Graphics g, int StartX, int StartY)
        {
            Content.Draw(g, StartX, StartY);
        }

        public override void Reframe(int width, int height)
        {
            Content.Reframe(width, height);
        }

        public override void OnMouseClick(int startX, int startY, MouseButtons button)
        {
            Content.OnMouseClick(startX, startY, button);
        }

        public override void OnMouseDoubleClick(int startX, int startY, MouseButtons button)
        {
            Content.OnMouseDoubleClick(startX, startY, button);
        }

        public override void OnMouseDown(int startX, int startY, MouseButtons button)
        {
            Content.OnMouseDown(startX, startY, button);
        }

        public override void OnMouseUp(int startX, int startY, MouseButtons button)
        {
            Content.OnMouseUp(startX, startY, button);
        }

        public override void OnMouseMove(int startX, int startY)
        {
            Content.OnMouseMove(startX, startY);
        }

        public override void OnMouseHover(int startX, int startY)
        {
            Content.OnMouseHover(startX, startY);
        }

        public override void OnMouseScroll(int startX, int startY, int Delta)
        {
            Content.OnMouseScroll(startX, startY, Delta);
        }
    }
}
