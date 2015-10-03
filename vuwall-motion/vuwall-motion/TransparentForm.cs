using System;
using System.Drawing;
using System.Windows.Forms;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        private Pen pen = new Pen(Color.Red, 5);

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
            e.Graphics.DrawEllipse(pen, 250, 250, 20, 20);
        }

        // TODO: Method to get an event from MYO to get x & y positions, used to invalidate
    }
}
