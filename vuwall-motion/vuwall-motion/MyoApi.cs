using System;
using System.Windows.Forms;
using System.Linq;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System.Drawing;
using MyoSharp.Poses;
using System.Runtime.Remoting.Channels;

namespace vuwall_motion
{
    public class MyoApi
    {
        public bool IsConnected { get; private set; }
        public IPoseSequence sendToBack { get; private set; }
        public IPoseSequence bringToFront { get; private set; }
        public bool SendToBack { get; private set; }
        public static event EventHandler PoseChanged;
        public static event EventHandler GyroscopeChanged;
        public static event EventHandler PoseSequenceDetected;

        public void Connect<T>(Func<T> process)
        {
            PoseChanged += (sender, args) => { };
            MyoApi.GyroscopeChanged += (sender, args) => { };
            PoseSequenceDetected += (sender, args) => { };
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
                    sendToBack = PoseSequence.Create(e.Myo, Pose.WaveOut, Pose.WaveOut);
                    bringToFront = PoseSequence.Create(e.Myo, Pose.WaveIn, Pose.WaveIn);

                    sendToBack.PoseSequenceCompleted += Myo_Sequence;
                    bringToFront.PoseSequenceCompleted += Myo_Sequence;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked += Myo_Locked;
                    e.Myo.Unlocked += Myo_Unlocked;
                    e.Myo.GyroscopeDataAcquired += Myo_GyroscopeDataAcquired;
                };
                
                hub.MyoDisconnected += (sender, e) =>
                {
                    this.IsConnected = false;
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);

                    sendToBack.PoseSequenceCompleted -= Myo_Sequence;
                    bringToFront.PoseSequenceCompleted -= Myo_Sequence;
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
            PoseChanged(e.Myo, e);
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }

        private void Myo_Sequence(object sender, PoseSequenceEventArgs e)
        {
            Console.WriteLine("{0} arm Myo performed a sequence of gestures!", e.Myo.Arm);
            e.Myo.Vibrate(VibrationType.Medium);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }

        public void Myo_GyroscopeDataAcquired(object o, GyroscopeDataEventArgs e)
        {
            MyoApi.GyroscopeChanged(o, e);
            //Console.WriteLine("X: {0}, Y: {1}", e.Gyroscope.X, e.Gyroscope.Y);
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
