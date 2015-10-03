using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        private Pen pen = new Pen(Color.Red, 5);
        public List<Rectangle> blobs = new List<Rectangle>();
        public List<Rectangle> rectangles = new List<Rectangle>(); 

        public TransparentForm() {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = BackColor;
            ShowInTaskbar = false;
        }

        private void TransparentForm_Load(object sender, EventArgs e)
        {
            int wl = TransparentWindowAPI.GetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            TransparentWindowAPI.SetLayeredWindowAttributes(this.Handle, 0, 128, TransparentWindowAPI.LWA.Alpha);
            Invalidate();
        }

        private void TransparentForm_Paint(object sender, PaintEventArgs e) {
            foreach (var blob in blobs)
            {
                e.Graphics.DrawEllipse(pen, blob);
            }

            foreach (var rect in rectangles)
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private void TransparentForm_MouseClick(object sender, EventArgs e)
        {
            Point local = this.PointToClient(Cursor.Position);
            AddRectangle(new Rectangle(local.X, local.Y, 500,500 ));
        }

        public void AddBlob(Rectangle blob)
        {
            blobs.Add(blob);
            Invalidate();
        }

        public void AddRectangle(Rectangle rect)
        {
            rectangles.Add(rect);
            Invalidate();
        }

        // TODO: Method to get an event from MYO to get x & y positions, used to invalidate
    }
}
