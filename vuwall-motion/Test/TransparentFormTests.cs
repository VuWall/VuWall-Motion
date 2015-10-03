using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using vuwall_motion;

namespace Test {
    public class TransparentFormTests : AssertionHelper {
        [SetUp]
        public void SetUp()
        {
            var form = new TransparentForm();
            form.Show(null);
        }

        public void Basic() {
            
        }
    }
}
