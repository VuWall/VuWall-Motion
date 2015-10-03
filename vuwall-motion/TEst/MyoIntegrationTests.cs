using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Discovery;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using NUnit.Framework;

namespace TEst
{
    [TestFixture]
    internal class MyoIntegrationTests
    {

        [TestCase]
        public void DiscoverMyoListener()
        {
            var channel = new Mock<IChannelListener>();
            var result = DeviceListener.Create(channel.Object);

            Assert.NotNull(result);
        }

        [TestCase]
        public void DiscoverNullMyoListener()
        {   
            var exception = Assert.Throws<ArgumentNullException>(() => DeviceListener.Create(null));
            Assert.That(exception.ParamName, Is.EqualTo("channelListener"));     
        }

        [TestCase]
        public void DiscoverNullMyoPose()
        {
            var gesture = Pose.Fist;

            var exception = Assert.Throws<ArgumentNullException>(() => HeldPose.Create(null, gesture));
            Assert.That(exception.ParamName, Is.EqualTo("myo"));
        }

        [TestCase]
        public void DiscoverNoMyoPose()
        {
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);

            var exception = Assert.Throws<ArgumentException>(() => HeldPose.Create(myo.Object));
            Assert.That(exception.ParamName, Is.EqualTo("targetPoses"));
        }

        [TestCase]
        public void DiscoverInvalidMyoPose()
        {
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);

            var gesture = Pose.DoubleTap;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => HeldPose.Create(myo.Object,
                TimeSpan.FromSeconds(-1), gesture));

            Assert.That(exception.ParamName, Is.EqualTo("interval"));
        }

        [TestCase]
        public void DiscoverSinglePose()
        {
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var gesture = Pose.Fist;

            var result = HeldPose.Create(myo.Object, gesture);

            Assert.NotNull(result);
        }

        [TestCase]
        public void DiscoverSequenceOfPoses()
        {
            var myo = new Mock<IMyoEventGenerator>(MockBehavior.Strict);
            var gestures = new Pose[] { Pose.WaveIn, Pose.WaveOut };

            var result = HeldPose.Create(myo.Object, gestures);

            Assert.NotNull(result);
        }
    }
}
