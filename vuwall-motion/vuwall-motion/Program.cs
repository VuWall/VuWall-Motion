using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Discovery;
using MyoSharp.Exceptions;
using MyoSharp.Math;
using MyoSharp.Poses;

namespace vuwall_motion {
    static class Program {
        private static void Main()
        {
            var myoApi = new MyoApi();
            
            var form = new TransparentForm();
            MyoApi.GyroscopeChanged += (o, e) => form.Move(o, (GyroscopeDataEventArgs)e);
            MyoApi.PoseChanged += (o, e) => form.Pose(o, (PoseEventArgs)e);
            myoApi.Connect(() => {Application.Run(form);
                                     return false;
            });
        }
    }
}
