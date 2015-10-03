using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSharp.Math;
using NUnit.Framework;

namespace TEst
{
    [TestFixture]
    class QuaternionTests : AssertionHelper
    {
        [TestCase]
        public void PointCenter()
        {
            float distFromScreen = -30f;
            Vector3F vec = new Vector3F(0, 0, 100);
            var ratio = (distFromScreen / vec.Z);
            var screenX = ratio * vec.X;
            var screenY = ratio * vec.Y;
            Expect(screenX, Is.EqualTo(0));
            Expect(screenY, Is.EqualTo(0));
        }

        [TestCase]
        public void PointLeft()
        {
            float distFromScreen = 30f;
            Vector3F vec = new Vector3F(3840, 0, 60);
            var ratio = (distFromScreen/vec.Z);
            var screenX = ratio * vec.X;
            var screenY = ratio * vec.Y;
            Expect(screenX, Is.EqualTo(1920));
            Expect(screenY, Is.EqualTo(0));
        }

        [TestCase]
        public void PointTopLeft()
        {
            float distFromScreen = 30f;
            Vector3F vec = new Vector3F(3840, 2160, 60);
            var ratio = (distFromScreen / vec.Z);
            var screenX = ratio * vec.X;
            var screenY = ratio * vec.Y;
            Expect(screenX, Is.EqualTo(1920));
            Expect(screenY, Is.EqualTo(1080));
        }

        [TestCase]
        public void PointTopLeftFuther()
        {
            float distFromScreen = 60f;
            Vector3F vec = new Vector3F(960, 540, 30);
            var ratio = (distFromScreen / vec.Z);
            var screenX = ratio * vec.X;
            var screenY = ratio * vec.Y;
            Expect(screenX, Is.EqualTo(1920));
            Expect(screenY, Is.EqualTo(1080));
        }

        [TestCase]
        public void MoveLeft()
        {
            QuaternionF thing = new QuaternionF(0, 1, 2, 3);
            Expect(thing.X, Is.EqualTo(0));
        }
    }
}
