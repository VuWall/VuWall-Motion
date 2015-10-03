using System.Drawing;
using NUnit.Framework;
using vuwall_motion;

namespace TEst
{
    [TestFixture]
    class WindowTests : AssertionHelper
    {
        [TestCase]
        public void GetWindowAtPoint()
        {
            var wa = new WindowApi();
            var window = wa.WindowFromPoint(new Point(1000, 500));
            var newWindow = new Window(window.Ptr, new Rectangle(100, 100, 600, 600));
            wa.SetWindow(newWindow);
        }

        [TestCase]
        public void GetWindowPos()
        {
            var wa = new WindowApi();
            var window = wa.WindowFromPoint(new Point(1000, 500));
            var newWindow = new Window(window.Ptr, new Rectangle(100, 100, 700, 700));
            wa.SetWindow(newWindow);
            var result = wa.WindowFromPoint(new Point(200, 200));
            Expect(result.Area, Is.EqualTo(new Rectangle(100, 100, 800, 800)));
        }
    }
}
