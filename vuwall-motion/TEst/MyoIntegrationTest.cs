using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyoSharp.Poses;
using NUnit.Framework;
using vuwall_motion;

namespace Test
{
    class MyoIntegrationTest : AssertionHelper
    {
        private MyoApi myo;

        [TestFixtureSetUp]
        public void Initialize()
        {
            myo = new MyoApi();
        }

        [Test]
        public void TestConnection()
        {
            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(myo.IsConnected, Is.True);
        }

        [Test]
        public void TestFistPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.Fist)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestRestPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.Rest)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestWaveInPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.WaveIn)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestWaveOutPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.WaveOut)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestSpreadPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.FingersSpread)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestDoubleTapPose()
        {
            var wasCalled = false;
            MyoApi.PoseChanged += (sender, e) => { if (((Pose)sender).Equals(Pose.DoubleTap)) wasCalled = true; };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestBringToFrontPose()
        {
            var wasCalled = false;
            MyoApi.PoseSequenceDetected += (sender, e) => { Console.WriteLine(sender);};

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

        }
    }
}
