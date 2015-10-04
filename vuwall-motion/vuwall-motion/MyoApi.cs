using System;
using System.Linq;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;

namespace vuwall_motion
{
    public class MyoApi
    {
        public bool IsConnected { get; private set; }
        public IPoseSequence sendToBack { get; private set; }
        public IPoseSequence bringToFront { get; private set; }
        public IHeldPose holdPose { get; private set; }
        public bool SendToBack { get; private set; }
        public static event EventHandler PoseChanged;
        public static event EventHandler PoseSequenceDetected;
        public static event EventHandler PoseHoldDetected;

        public void Connect<T>(Func<T> process)
        {
            PoseChanged += (sender, args) => { };
            PoseSequenceDetected += (sender, args) => { };
            PoseHoldDetected += (sender, args) => { };
            using (var channel = Channel.Create(
               ChannelDriver.Create(ChannelBridge.Create(),
               MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                hub.MyoConnected += (sender, e) =>
                {
                    // Startup Connection 
                    IsConnected = true;
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.Unlock(UnlockType.Hold);

                    // Hold Poses
                    holdPose = (HeldPose)HeldPose.Create(e.Myo, Pose.Fist);
                    holdPose.Interval = TimeSpan.FromSeconds(0.5);
                    holdPose.Start();

                    // Sequences
                    sendToBack = PoseSequence.Create(e.Myo, Pose.WaveOut, Pose.WaveOut);
                    bringToFront = PoseSequence.Create(e.Myo, Pose.WaveIn, Pose.WaveIn);

                    // Event Listeners
                    sendToBack.PoseSequenceCompleted += Myo_Sequence;
                    bringToFront.PoseSequenceCompleted += Myo_Sequence;
                    holdPose.Triggered += Myo_HoldPose;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked += Myo_Locked;
                    e.Myo.Unlocked += Myo_Unlocked;
                };

                hub.MyoDisconnected += (sender, e) =>
                {
                    this.IsConnected = false;
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);

                    sendToBack.PoseSequenceCompleted -= Myo_Sequence;
                    bringToFront.PoseSequenceCompleted -= Myo_Sequence;
                    holdPose.Triggered -= Myo_HoldPose;
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                    e.Myo.Locked -= Myo_Locked;
                    e.Myo.Unlocked -= Myo_Unlocked;
                };

                channel.StartListening();
                process();
            }
        }

        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            PoseChanged(e.Myo.Pose, new EventArgs());
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }

        private void Myo_Sequence(object sender, PoseSequenceEventArgs e)
        {
            PoseSequenceDetected(sender, e);
            Console.WriteLine("{0} arm Myo performed a sequence of gestures!", e.Myo.Arm);
            e.Myo.Vibrate(VibrationType.Medium);
            e.Myo.Unlock(UnlockType.Timed);
            e.Myo.Unlock(UnlockType.Hold);
        }


        private void Myo_HoldPose(object sender, PoseEventArgs e)
        {
            PoseHoldDetected(sender, e);
            Console.WriteLine("{0} arm Myo is holding pose {1}!", e.Myo.Arm, e.Pose);
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
