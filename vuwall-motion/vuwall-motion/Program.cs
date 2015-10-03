using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vuwall_motion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var Myo = new MyoApi();

            Myo.Connect(()=> MessageBox.Show("Running!"));
        }
    }
}
