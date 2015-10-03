using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TEst
{
    [TestFixture]
    class Dummy : AssertionHelper
    {
        [TestCase]
        public void Test()
        {
            Expect(true, Is.EqualTo(true));
        }
    }
}
