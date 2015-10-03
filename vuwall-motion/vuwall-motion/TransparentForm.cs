using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        private Pen pen = new Pen(Color.Red, 5);
        private Brush brush = new SolidBrush(Color.Red);

        private Size blob_size = new Size(50,50);

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
            TransparentWindowAPI.SetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle, wl);
            TransparentWindowAPI.SetLayeredWindowAttributes(this.Handle, 0, 128, TransparentWindowAPI.LWA.Alpha);

            // Initialize data for testing
            blobs.Add(new Rectangle(CursorPosition(), blob_size));
            rectangles.Add(new Rectangle(CursorPosition(), new Size(500,500)));
            Invalidate();
        }

        private void TransparentForm_Paint(object sender, PaintEventArgs e) {
            foreach (var blob in blobs)
            {
                e.Graphics.FillEllipse(brush, blob);
            }

            foreach (var rect in rectangles)
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private void TransparentForm_MouseClick(object sender, EventArgs e)
        {
            UpdateRect(new Rectangle(CursorPosition(), new Size(500,500)));
        }

        private void TransparentForm_MouseMove(object sender, EventArgs e)
        {
            UpdateBlob(CursorPosition());
        }

        public void AddBlob(Point pos)
        {
            Rectangle blob = new Rectangle(pos, blob_size);
            blobs.Add(blob);
            Invalidate();
        }

        public void AddRect(Rectangle rect)
        {
            rectangles.Add(rect);
            Invalidate();
        }

        public void UpdateBlob(Point pos)
        {
            // To have multiple blobs working with MYO, we need some sort of identifier to map which MYO device controls which blob
            blobs[0] = new Rectangle(pos, blob_size);
            Invalidate();
        }

        public void UpdateRect(Rectangle rect)
        {
            rectangles[0] = rect;
            Invalidate();
        }

        public void DeleteBlob(Rectangle blob)
        {
            blobs.Remove(blob);
            Invalidate();
        }

        public void DeleteRect(Rectangle rect)
        {
            rectangles.Remove(rect);
            Invalidate();
        }

        // TODO: Method to get an event from MYO to get x & y positions, used to invalidate

        // TODO: Method to get an event from MYO to get the rectangle of a given window

        private Point CursorPosition()
        {
            var cursorLocation =  this.PointToClient(Cursor.Position);
            var centerX = cursorLocation.X - blob_size.Width / 2;
            var centerY = cursorLocation.Y - blob_size.Height / 2;
            return (new Point(centerX, centerY));
        }
    }
}
