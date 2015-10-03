using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using vuwall_motion;

namespace TEst
{
    class MyoIntegrationTest : AssertionHelper
    {
        [Test]
        public void TestConnection()
        {
            var Myo = new MyoApi();

            Myo.Connect(() =>
            {
                Thread.Sleep(5000);
                return false;
            });

            Assert.That(Myo.IsConnected, Is.True);
        }
    }
}
