using System;
using System.Drawing;
using MyoSharp.Math;

namespace vuwall_motion
{
    public static class Math3D
    {
        public static Point PixelFromVector(Vector3F v)
        {
            var inverted = new Vector3F(0 - v.X, 0 - v.Y, v.Z);
            var distFromScreen = 2300;
            var position = new Point(960, 540);
            var ratio = (distFromScreen / inverted.Z);
            var screenX = ratio * inverted.X + position.X;
            var screenY = ratio * inverted.Y + position.Y;
            return new Point((int)Math.Round(screenX), (int)Math.Round(screenY));
        }

        public static Vector3F DirectionalVector(Vector3F v)
        {
            var pitchOp = Math.Sin(DegreeToRadian(v.X));
            var pitchAdj = Math.Cos(DegreeToRadian(v.X));
            var yawOp = Math.Sin(DegreeToRadian(v.Z));
            var yawAdj = Math.Cos(DegreeToRadian(v.Z));
            var vX = yawOp;
            var vY = pitchOp;
            var vZ = yawAdj > pitchAdj ? pitchAdj : yawAdj;
            if (vZ < 0)
            {
                if (yawAdj < 0 && pitchAdj > 0)
                {
                    vZ = yawAdj;
                }
                else if (pitchAdj < 0 && yawAdj > 0)
                {
                    vZ = pitchAdj;
                }
                else
                {
                    vZ = yawAdj < pitchAdj ? pitchAdj : yawAdj;
                }
            }
            return new Vector3F((float)vX, (float)vY, (float)vZ);
        }

        public static Vector3F RoundTo90(Vector3F v)
        {
            return new Vector3F(RoundTo90(v.X), RoundTo90(v.Y), RoundTo90(v.Z));
        }

        public static float RoundTo90(float f)
        {
            var t = (float)Math.Round(f / 90) * 90;
            return t == 360 ? 0 : t;
        }

        public static Vector3F FromQuaternion(QuaternionF q)
        {
            float sqw = q.W * q.W;
            float sqx = q.X * q.X;
            float sqy = q.Y * q.Y;
            float sqz = q.Z * q.Z;
            float unit = sqx + sqy + sqz + sqw;
            float test = q.X * q.W - q.Y * q.Z;
            double vY;
            double vX;
            double vZ;

            if (test > 0.4995f * unit)
            {
                vY = 2f * Math.Atan2(q.Y, q.X);
                vX = Math.PI / 2;
                vZ = 0;
                Vector3F v1 = new Vector3F((float)vX, (float)vY, (float)vZ);
                return NormalizeAngles(RadianToDegree(v1));
            }
            if (test < -0.4995f * unit)
            {
                vY = -2f * Math.Atan2(q.Y, q.X);
                vX = -Math.PI / 2;
                vZ = 0;
                Vector3F v2 = new Vector3F((float)vX, (float)vY, (float)vZ);
                return NormalizeAngles(RadianToDegree(v2));
            }
            QuaternionF q1 = new QuaternionF(q.W, q.Z, q.X, q.Y);
            vY = (float)Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W));     // Yaw
            vX = (float)Math.Asin(2f * (q.X * q.Z - q.W * q.Y));                             // Pitch
            vZ = (float)Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z));      // Roll
            Vector3F v3 = new Vector3F((float)vX, (float)vY, (float)vZ);
            return NormalizeAngles(RadianToDegree(v3));
        }

        private static Vector3F DegreeToRadian(Vector3F v)
        {
            return new Vector3F(DegreeToRadian(v.X), DegreeToRadian(v.Y), DegreeToRadian(v.Z));
        }

        private static Vector3F RadianToDegree(Vector3F v)
        {
            return new Vector3F(RadianToDegree(v.X), RadianToDegree(v.Y), RadianToDegree(v.Z));
        }

        private static float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        private static float RadianToDegree(float angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }

        static Vector3F NormalizeAngles(Vector3F angles)
        {
            return new Vector3F(NormalizeAngle(angles.X), NormalizeAngle(angles.Y), NormalizeAngle(angles.Z));
        }

        static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }
}
