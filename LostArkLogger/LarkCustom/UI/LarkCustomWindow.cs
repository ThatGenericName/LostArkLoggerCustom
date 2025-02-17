﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LostArkLogger.LarkCustom.UI
{
    public partial class LarkCustomWindow : Form
    {
        private BufferedGraphics _graphicsBuffer;
        private Graphics graphics;

        private Bitmap graphBitmap;
        private Graphics bitmapGfx;

        private System.Windows.Forms.Timer gfxRenderTimer;

        public event Action<Graphics, int, int> DrawAction;

        public Graphics DrawingGraphics { get => bitmapGfx; }
        public LarkCustomWindow()
        {
            InitializeComponent();
            UpdateGraphicsBuffer();
            graphBitmap = new Bitmap(Width, Height);
            bitmapGfx = Graphics.FromImage(graphBitmap);

            gfxRenderTimer = new System.Windows.Forms.Timer();
            gfxRenderTimer.Interval = (1000 / 30); // 30 fps i guess
            gfxRenderTimer.Tick += new EventHandler(Draw);
            gfxRenderTimer.Start();
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

            DrawAction.Invoke(bitmapGfx, 0, 0);

            _graphicsBuffer.Graphics.DrawImage(graphBitmap, 0, 0, this.Width, this.Height);
            _graphicsBuffer.Render();
        }
    }
}
