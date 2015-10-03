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
            Point? tlAlteration = null;
            PointF? brRatio = null;
            Point absoluteTL = new Point();

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
                            if (tlAlteration != null && brRatio != null)
                            {
                                var myo = (Myo) o;
                                var vect = Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation));
                                Console.WriteLine(vect);
                                var position = Math3D.PixelFromVector(vect);
                                //Console.WriteLine(position + " : " + tlAlteration + " : " + brRation);
                                var calibrated = position;
                                //var calibrated = Calibration.AlterPoint((Point) tlAlteration, (PointF) brRatio, position);
                                //Console.WriteLine(calibrated);
                                if (calibrated.X < 0)
                                {
                                    calibrated.X = 0;
                                }
                                else if (calibrated.X > 1920)
                                {
                                    calibrated.X = 1920;
                                }
                                if (calibrated.Y < 0)
                                {
                                    calibrated.Y = 0;
                                }
                                else if (calibrated.Y > 1080)
                                {
                                    calibrated.Y = 1080;
                                }
                                Cursor.Position = calibrated;
                            }
                        };
                        e.Myo.PoseChanged += (o, args) =>
                        {
                            var myo = (Myo)o;

                            if (myo.Pose == Pose.DoubleTap)
                            {
                                if (tlAlteration == null)
                                {
                                    absoluteTL = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
                                    tlAlteration = Calibration.CalibrateTL(absoluteTL);
                                    Console.WriteLine("Calibrated Top Left!");
                                }
                                else if (brRatio == null)
                                {
                                    Point absoluteBR = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
                                    brRatio = Calibration.CalibrateBR(new Point(1920, 1080), absoluteTL, absoluteBR);
                                    Console.WriteLine("Calibrated! " + tlAlteration.ToString() + " : " + brRatio);
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
