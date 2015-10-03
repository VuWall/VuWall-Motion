using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuwall_motion
{
    public static class Calibration
    {
        public static Point CalibrateTL(Point topLeft)
        {
            return new Point(0 - topLeft.X, 0 - topLeft.Y);
        }

        public static PointF CalibrateBR(Point screenSize, Point topLeft, Point bottomRight)
        {
            return new PointF((float)screenSize.X / ((float)bottomRight.X - (float)topLeft.X), (float)screenSize.Y / ((float)bottomRight.Y - (float)topLeft.Y));
        }

        public static Point AlterPoint(Point alterTL, PointF ratioBR, Point point)
        {
            var tl = new Point(point.X - alterTL.X, point.Y - alterTL.Y);
            var br = new Point((int)Math.Round(tl.X*ratioBR.X), (int) Math.Round(tl.Y*ratioBR.Y));
            return br;
        }
    }
}
