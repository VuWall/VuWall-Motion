using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using NUnit.Framework;
using vuwall_motion;
using System.Drawing;
using System.Threading;

namespace Test {
    public class TransparentFormTests : AssertionHelper
    {
        private TransparentForm form;
        [SetUp]
        public void SetUp()
        {
            form = new TransparentForm();
            form.Show();
        }

        [TearDown]
        public void TearDown()
        {
            form.Close();
            form.Dispose();
        }

        [Test]
        public void Basic()
        {
            Expect(form.Size, Is.EqualTo(Screen.PrimaryScreen.Bounds.Size));
        }

        [Test]
        public void AddBlob()
        {
            form.AddBlob(new Point(300,300));
            Expect(form.blobs.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddRectangle()
        {
            form.AddRect(new Rectangle(0,0,500,500));
            Expect(form.rectangles.Count, Is.EqualTo((2)));
        }

        [Test]
        public void DeleteBlob()
        {
            var blob = new Rectangle(69, 69, 50, 50);
            form.AddBlob(new Point(69, 69));
            Expect(form.blobs, Has.Member(blob));
            form.DeleteBlob(blob);
            Expect(form.blobs, Has.No.Member(blob));
        }

        [Test]
        public void DeleteRect() {
            var rect = new Rectangle(69, 69, 666, 666);
            form.AddRect(rect);
            Expect(form.rectangles, Has.Member(rect));
            form.DeleteRect(rect);
            Expect(form.rectangles, Has.No.Member(rect));
        }
    }
}
