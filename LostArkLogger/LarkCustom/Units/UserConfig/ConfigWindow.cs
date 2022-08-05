using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LostArkLogger.LarkCustom.Units
{
    public partial class ConfigWindow : Form
    {
        private BufferedGraphics _graphicsBuffer;
        private Graphics graphics;

        private Bitmap graphBitmap;
        private Graphics bitmapGfx;

        private System.Windows.Forms.Timer gfxRenderTimer;

        private Action<Graphics, int, int>? configSettingDraw;

        public Graphics DrawingGraphics { get => bitmapGfx; }
        public ConfigWindow()
        {
            InitializeComponent();
            UpdateGraphicsBuffer();
            graphBitmap = new Bitmap(Width, Height);
            bitmapGfx = Graphics.FromImage(graphBitmap);

            gfxRenderTimer = new System.Windows.Forms.Timer();
            gfxRenderTimer.Interval = (1000 / 15); // 15 fps i guess
            gfxRenderTimer.Tick += new EventHandler(Draw);
        }

        public void UpdateGraphicsBuffer()
        {
            BufferedGraphicsContext bufferedContext = BufferedGraphicsManager.Current;
            _graphicsBuffer = bufferedContext.Allocate(CreateGraphics(), DisplayRectangle);
            _graphicsBuffer.Graphics.Clear(Color.White);
            _graphicsBuffer.Graphics.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
        }

        public void Draw(object sender, EventArgs e)
        {
            bitmapGfx.Clear(Color.White);

            configSettingDraw(bitmapGfx, 0, 0);

            _graphicsBuffer.Graphics.DrawImage(graphBitmap, 0, 0, this.Width, this.Height);
            _graphicsBuffer.Render();
        }
        public void ShowWindow(Action<Graphics, int, int> csd, int x, int y, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            configSettingDraw = csd ?? throw new InvalidOperationException("ConfigWindow.ShowWindow() called without being assigned a config");
            gfxRenderTimer.Start();
            this.WindowState = FormWindowState.Normal;
        }

        public void HideWindow()
        {
            gfxRenderTimer.Stop();
            configSettingDraw = default;
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
