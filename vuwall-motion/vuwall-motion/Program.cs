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
            myoApi.Connect(() => {Application.Run(form);
                                     return false;
            });
            

            //MyoApi.PoseChanged += ((o, e) =>
            //{
            //    var myo = (Myo)o;
            //    var pose = myo.Pose;

            //    if (pose == Pose.DoubleTap)
            //    {
            //        var api = new WindowApi();
            //        var position =
            //            Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
            //        var window = api.WindowFromPoint(position);
            //        var newWindow = new Window(window.Ptr, new Rectangle(0, 0, 600, 600));
            //        api.SetWindow(newWindow);
            //        api.BringToFront(newWindow);
            //    }
            //});

            //myoApi.Connect(() =>
            //{
            //    Application.Run(form);
            //    return false;
            //});
        }
    }
}
