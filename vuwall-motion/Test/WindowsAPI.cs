using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using vuwall_motion;

namespace Test
{
    /// <summary>
    /// Summary description for WindowsAPI
    /// </summary>
    [TestClass]
    public class WindowsAPI : AssertionHelper
    {

        [TestMethod]
        public void GetWindow()
        {
            var wa = new WindowApi();
            var result = wa.WindowFromPoint(1000, 500);
            Expect(result, Is.EqualTo(3));
        }
    }
}
