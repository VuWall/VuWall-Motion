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
            Vector3F tlAlteration = null;
            Vector3F absoluteTL = null;

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
                            if (absoluteTL != null)
                            {
                                var myo = (Myo) o;
                                var orientation = myo.Orientation;
                                var quaternion = Math3D.FromQuaternion(orientation) - absoluteTL;
                                var vect = Math3D.DirectionalVector(quaternion);
                                var position = Math3D.PixelFromVector(vect);
                                //var calibrated = Calibration.AlterPoint((Point) tlAlteration, (PointF) brRatio, position);
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
                            }
                        };
                        e.Myo.PoseChanged += (o, args) =>
                        {
                            var myo = (Myo)o;

                            if (myo.Pose == Pose.DoubleTap)
                            {
                                if (absoluteTL == null)
                                {
                                    absoluteTL = Math3D.FromQuaternion(myo.Orientation);
                                    Console.WriteLine("Calibrated Top Left!");
                                }
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
