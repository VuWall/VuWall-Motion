using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuwall_motion
{
    public class Window
    {
        public IntPtr Ptr { get; private set; }
        public Rectangle Area { get; private set; }

        public Window(IntPtr ptr, Rectangle area)
        {
            Ptr = ptr;
            Area = area;
        }
    }
}
