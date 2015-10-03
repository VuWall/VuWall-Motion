using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyoSharp.Device;
using MyoSharp.Poses;

namespace vuwall_motion {
    static class Program {
        private static void Main()
        {
            var Myo = new MyoApi();
            var form = new TransparentForm();
            Myo.Connect(() =>
            {
                Application.Run(form);
                return false;
            });
        }
    }
}
