using System;
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
            Expect(result.Area, Is.EqualTo(new Rectangle(100, 100, 700, 700)));
        }

        [TestCase]
        public void BringToFront()
        {
            var wa = new WindowApi();
            var window = wa.WindowFromPoint(new Point(1000, 500));
            wa.BringToFront(window);
            var frontWindow = wa.GetForegroundWindow();
            Expect(window.Ptr, Is.EqualTo(frontWindow.Ptr));
        }

        [TestCase]
        public void GetWindowRect()
        {
            var wa = new WindowApi();
            var window = wa.WindowFromPoint(new Point(1000, 500));
            Console.WriteLine(window.Area);
        }
    }
}
