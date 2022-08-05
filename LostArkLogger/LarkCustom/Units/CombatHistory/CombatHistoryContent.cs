using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


using LostArkLogger.LarkCustom.UI;

namespace LostArkLogger.LarkCustom.Units
{
    public class CombatHistoryContent : Content
    {
        public enum CombatHistoryMode
        {
            All,
            Self,
            BattleItems
        }

        public override int MinWidth => throw new NotImplementedException();

        public override int MinHeight => throw new NotImplementedException();

        public override void Draw(Graphics g, int StartX, int StartY)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseClick(int startX, int startY, MouseButtons button)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseDoubleClick(int startX, int startY, MouseButtons button)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseDown(int startX, int startY, MouseButtons button)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseHover(int startX, int startY)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseMove(int startX, int startY)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseScroll(int startX, int startY, int Delta)
        {
            throw new NotImplementedException();
        }

        public override void OnMouseUp(int startX, int startY, MouseButtons button)
        {
            throw new NotImplementedException();
        }

        public override void Reframe(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
