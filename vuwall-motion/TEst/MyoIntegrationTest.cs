using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyoSharp.Device;
using MyoSharp.Poses;
using NUnit.Framework;
using vuwall_motion;

namespace Test
{
    class MyoIntegrationTest : AssertionHelper
    {
        private MyoApi myo;

        [TestFixtureSetUp]
        public void SetUp()
        {
            myo = new MyoApi();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            
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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.Fist)) wasCalled = true; };

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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.Rest)) wasCalled = true; };

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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.WaveIn)) wasCalled = true; };

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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.WaveOut)) wasCalled = true; };

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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.FingersSpread)) wasCalled = true; };

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
            MyoApi.PoseChanged += (sender, e) => { if ((((Myo)sender).Pose).Equals(Pose.DoubleTap)) wasCalled = true; };

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
            MyoApi.PoseSequenceDetected += (sender, e) =>
            {
                var poses = ((PoseSequence)sender)._sequence.ToArray();
                if (poses.Length == 2 && poses.First().Equals(Pose.WaveIn) && poses.Last().Equals(Pose.WaveIn))
                {
                    wasCalled = true;
                }
            };

            myo.Connect(() =>
            {
                Thread.Sleep(8000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestSendToBackPose()
        {
            var wasCalled = false;
            MyoApi.PoseSequenceDetected += (sender, e) =>
            {
                var poses = ((PoseSequence)sender)._sequence.ToArray();
                if (poses.Length == 2 && poses.First().Equals(Pose.WaveOut) && poses.Last().Equals(Pose.WaveOut))
                {
                    wasCalled = true;
                }
            };

            myo.Connect(() =>
            {
                Thread.Sleep(8000);
                return false;
            });

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void TestHoldPose()
        {
            var wasCalled = false;

            MyoApi.PoseHoldDetected += (sender, e) =>
            {
                var holdPose = ((HeldPose)sender);
                if (holdPose != null)
                {
                    wasCalled = true;
                }
            };

            myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(wasCalled, Is.True);

        }
    }
}
