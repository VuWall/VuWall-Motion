using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using NUnit.Framework;
using vuwall_motion;
using System.Drawing;

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
            var formSize = form.Size;
            var screenRes = Screen.PrimaryScreen.Bounds;
            Expect(form.Size, Is.EqualTo(Screen.PrimaryScreen.Bounds.Size));
        }
    }
}
