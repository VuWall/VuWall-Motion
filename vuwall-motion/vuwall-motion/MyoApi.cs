using System;
using System.Windows.Forms;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System.Drawing;
using System.Runtime.Remoting.Channels;

namespace vuwall_motion
{
    public class MyoApi
    {
        public bool IsConnected { get; private set; }
        public static event EventHandler PoseChanged;
        public static event EventHandler GyroscopeChanged;


        Point? absoluteTL = null;
        Point? absoluteBR = null;

        public void Connect<T>(Func<T> process)
        {
            MyoApi.PoseChanged += (sender, args) => { };
            MyoApi.GyroscopeChanged += (sender, args) => { };
            using (var channel = Channel.Create(
               ChannelDriver.Create(ChannelBridge.Create(),
               MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                hub.MyoConnected += (sender, e) =>
                {
                    this.IsConnected = true;
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);

                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked += Myo_Locked;
                    e.Myo.Unlocked += Myo_Unlocked;
                    e.Myo.GyroscopeDataAcquired += Myo_GyroscopeDataAcquired;
                };
                
                hub.MyoDisconnected += (sender, e) =>
                {
                    this.IsConnected = false;
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                    e.Myo.Locked -= Myo_Locked;
                    e.Myo.Unlocked -= Myo_Unlocked;
                    e.Myo.GyroscopeDataAcquired -= Myo_GyroscopeDataAcquired;
                };
                
                channel.StartListening();
                process();
            }
        }

        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            PoseChanged(e.Myo, new EventArgs());
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }

        public void Myo_GyroscopeDataAcquired(object o, GyroscopeDataEventArgs e)
        {
            MyoApi.GyroscopeChanged(o, e);
            var myo = (Myo)o;
            if (absoluteTL == null) {
                absoluteTL = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
            }

            var position = Math3D.PixelFromVector(Math3D.DirectionalVector(Math3D.FromQuaternion(myo.Orientation)));
            position.Offset(0 - absoluteTL.Value.X, 0 - absoluteTL.Value.Y);

            if (position.X < 0) {
                position.X = 0;
            } else if (position.X > 1920) {
                position.X = 1920;
            }
            if (position.Y < 0) {
                position.Y = 0;
            } else if (position.Y > 1080) {
                position.Y = 1080;
            }
            Cursor.Position = position;
        }

        private void Myo_Unlocked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has unlocked!", e.Myo.Arm);
        }

        private void Myo_Locked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has locked!", e.Myo.Arm);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }
    }
}
