using System;
using System.Windows.Forms;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        public TransparentForm() {
            InitializeComponent();
        }

        private void TransparentForm_Load(object sender, EventArgs e) {
            int wl = TransparentWindowAPI.GetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            TransparentWindowAPI.SetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle, wl);
            TransparentWindowAPI.SetLayeredWindowAttributes(this.Handle, 0, 128, TransparentWindowAPI.LWA.Alpha);
        }
    }
}
