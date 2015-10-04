using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using NUnit.Framework;
using vuwall_motion;
using System.Drawing;
using System.Threading;
using Moq;
using MyoSharp.Device;

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
            var myo = new Mock<IMyo>(MockBehavior.Strict);
            form.AddBlob(myo.Object, new Point(300,300));
            Expect(form.blobs.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddRectangle()
        {
            var myo = new Mock<IMyo>(MockBehavior.Strict);
            form.AddRect(myo.Object,new Rectangle(0,0,500,500));
            Expect(form.rectangles.Count, Is.EqualTo((1)));
        }

        [Test]
        public void DeleteBlob()
        {
            var myo = new Mock<IMyo>(MockBehavior.Strict);
            var blob = new Point(69, 69);
            form.AddBlob(myo.Object, blob);
            Expect(form.blobs.ContainsKey(myo.Object), Is.True);
            Expect(form.blobs.Count, Is.EqualTo(1));
            form.DeleteBlob(myo.Object);
            Expect(form.blobs, Is.Empty);
        }

        [Test]
        public void DeleteRect() {
            var myo = new Mock<IMyo>(MockBehavior.Strict);
            var rect = new Rectangle(69, 69, 666, 666);
            form.AddRect(myo.Object, rect);
            Expect(form.rectangles.ContainsKey(myo.Object), Is.True);
            form.DeleteRect(myo.Object);
            Expect(form.rectangles, Is.Empty);
        }
    }
}
