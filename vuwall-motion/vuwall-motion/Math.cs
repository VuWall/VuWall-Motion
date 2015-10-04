using System;
using System.Drawing;
using MyoSharp.Math;

namespace vuwall_motion
{
    public static class Math3D
    {
        public static Point PixelFromVector(Vector3F v)
        {
            var inverted = new Vector3F(0 - v.X, v.Y, v.Z);
            var distFromScreen = 4000;
            var ratio = (distFromScreen / inverted.Z);
            var screenX = ratio * inverted.X;
            var screenY = ratio * inverted.Y;
            return new Point((int)Math.Round(screenX), (int)Math.Round(screenY));
        }

        public static Vector3F RotateX(Vector3F v1, double rad)
        {
            double x = v1.X;
            double y = (v1.Y * Math.Cos(rad)) - (v1.Z * Math.Sin(rad));
            double z = (v1.Y * Math.Sin(rad)) + (v1.Z * Math.Cos(rad));
            return new Vector3F((float)x, (float)y, (float)z);
        }

        public static Vector3F Pitch(Vector3F v1, double rad)
        {
            return RotateX(v1, rad);
        }

        public static Vector3F RotateY(Vector3F v1, double rad)
        {
            double x = (v1.Z * Math.Sin(rad)) + (v1.X * Math.Cos(rad));
            double y = v1.Y;
            double z = (v1.Z * Math.Cos(rad)) - (v1.X * Math.Sin(rad));
            return new Vector3F((float)x, (float)y, (float)z);
        }

        public static Vector3F Yaw(Vector3F v1, double rad)
        {
            return RotateY(v1, rad);
        }

        public static Vector3F DirectionalVector(Vector3F v)
        {
            var vect = new Vector3F(0, 0, 1);
            var pitched = Pitch(vect, v.X);
            var yawed = Yaw(pitched, v.Z);
            return yawed;
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
            var pitch = QuaternionF.Pitch(q);
            var roll = QuaternionF.Roll(q);
            var yaw = QuaternionF.Yaw(q);
            return new Vector3F(pitch, roll, yaw);
        }

        private static float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }
    }
}
