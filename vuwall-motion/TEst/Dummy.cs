using NUnit.Framework;
using MyoSharp.Math;

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

        [TestCase]
        public void QuickTest()
        {
            QuaternionF thing = new QuaternionF(0, 1, 2, 3);
            Expect(thing.X, Is.EqualTo(0));
        }
    }
}
