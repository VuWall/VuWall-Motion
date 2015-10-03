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
    }
}
