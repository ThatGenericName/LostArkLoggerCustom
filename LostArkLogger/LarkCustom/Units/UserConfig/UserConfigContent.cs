using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using LostArkLogger.LarkCustom.UI;
using LostArkLogger.LarkCustom.Config;

namespace LostArkLogger.LarkCustom.Units
{
    public class UserConfigContent : Content
    {
        public override int MinWidth => 25;

        public override int MinHeight => 25;

        private UserConfig UC;

        public UserConfigContent(UserConfig userConfig)
        {
            UC = userConfig;
        }
            
        public override void Draw(Graphics g, int StartX, int StartY)
        {
            return;
        }

        public override void Reframe(int width, int height)
        {
            return;
        }

        // input response

        public override void OnMouseClick(int startX, int startY, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                // figure out where is clicked
            }
            return;
        }

        public override void OnMouseDoubleClick(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseDown(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseUp(int startX, int startY, MouseButtons button)
        {
            return;
        }

        public override void OnMouseMove(int startX, int startY)
        {
            return;
        }

        public override void OnMouseHover(int startX, int startY)
        {
            return;
        }

        public override void OnMouseScroll(int startX, int startY, int Delta)
        {
            return;
        }
    }
}
