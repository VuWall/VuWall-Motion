using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        public TransparentForm() {
            InitializeComponent();
            Opacity = 1;
            WindowState = FormWindowState.Maximized;
        }

        private void TransparentForm_Load(object sender, EventArgs e) {

        }
    }
}
