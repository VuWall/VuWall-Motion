using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSharp.Math;
using NUnit.Framework;

namespace TEst
{
    [TestFixture]
    class Trigo : AssertionHelper
    {
        [TestCase]
        public void First()
        {
            Vector3F angles = new Vector3F(45, 45, 0);
            var pitchOp = Math.Sin(DegreeToRadian(angles.X));
            var pitchAdj = Math.Cos(DegreeToRadian(angles.X));
            var yawOp = Math.Sin(DegreeToRadian(angles.Y));
            var yawAdj = Math.Cos(DegreeToRadian(angles.Y));
            var vX = yawOp;
            var vY = pitchOp;
            var vZ = pitchAdj;
            var vect = new Vector3F((float)vX, (float)vY, (float)vZ);
            Expect(vect, Is.EqualTo(new Vector3F(0.5f, 0.5f, 0.5f)));
        }

        private static float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        private static float RadianToDegree(float angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }
    }
}
