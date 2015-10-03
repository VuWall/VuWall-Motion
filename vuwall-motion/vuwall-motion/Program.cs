using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyoSharp.Device;
using MyoSharp.Poses;

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
            Myo.Connect(()=> MessageBox.Show("Running Me Myo!"));
        }
    }
}
