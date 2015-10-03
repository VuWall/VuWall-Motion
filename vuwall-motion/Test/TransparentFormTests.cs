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
            Expect(form.blobs.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddRectangle()
        {
            form.AddRect(new Rectangle(0,0,500,500));
            Expect(form.rectangles.Count, Is.EqualTo((1)));
        }
    }
}
