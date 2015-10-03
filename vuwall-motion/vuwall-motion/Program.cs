using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Discovery;
using MyoSharp.Exceptions;
using MyoSharp.Math;
using MyoSharp.Poses;

namespace vuwall_motion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Point? absolute = null;

            using (var channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(), MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            {
                using (var hub = Hub.Create(channel))
                {
                    hub.MyoConnected += (sender, e) =>
                    {
                        e.Myo.Unlock(UnlockType.Hold);
                        e.Myo.Unlock(UnlockType.Timed);
                        e.Myo.GyroscopeDataAcquired += (o, args) =>
                        {
                            var myo = (Myo)o;
                            if (absolute == null)
                            {
                                absolute = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
                            }
                            var position = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
                            position.Offset(0 - absolute.Value.X, 0 - absolute.Value.Y);
                            if (position.X < 0)
                            {
                                position.X = 0;
                            }
                            else if (position.X > 1920)
                            {
                                position.X = 1920;
                            }
                            if (position.Y < 0)
                            {
                                position.Y = 0;
                            }
                            else if (position.Y > 1080)
                            {
                                position.Y = 1080;
                            }
                            Cursor.Position = position;
                            
                        };
                        e.Myo.PoseChanged += (o, args) =>
                        {
                            var myo = (Myo)o;

                            if (myo.Pose == Pose.Fist)
                            {
                                var api = new WindowApi();
                                var position = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
                                var window = api.WindowFromPoint(position);
                                var newWindow = new Window(window.Ptr, new Rectangle(0, 0, 600, 600));
                                api.SetWindow(newWindow);
                                api.BringToFront(newWindow);
                            }

                            if (!myo.IsUnlocked)
                            {
                                myo.Unlock(UnlockType.Hold);
                                myo.Unlock(UnlockType.Timed);
                            }
                        };
                    };

                    channel.StartListening();

                    while (true)
                    {

                    }
                }
            }
        }
    }
}
