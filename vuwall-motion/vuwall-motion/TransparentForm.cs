using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using MyoSharp.Device;
using MyoSharp.Math;
using MyoSharp.Poses;

namespace vuwall_motion {
    public partial class TransparentForm : Form {
        private Pen pen = new Pen(Color.Red, 5);
        private Brush brush = new SolidBrush(Color.Red);
        private Size clientRes = Screen.PrimaryScreen.Bounds.Size;
        private Window SelectedWindow;
        private Point? DragOffset;

        Vector3F absoluteTL;
        Vector3F absoluteBR;

        private Size blob_size = new Size(50,50);

        public List<Rectangle> blobs = new List<Rectangle>();
        public List<Rectangle> rectangles = new List<Rectangle>(); 

        public TransparentForm() {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = BackColor;
            ShowInTaskbar = false;
            this.TopMost = true;
        }

        private void TransparentForm_Load(object sender, EventArgs e)
        {
            int wl = TransparentWindowAPI.GetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            TransparentWindowAPI.SetWindowLong(this.Handle, TransparentWindowAPI.GWL.ExStyle, wl);
            TransparentWindowAPI.SetLayeredWindowAttributes(this.Handle, 0, 128, TransparentWindowAPI.LWA.Alpha);
            blobs.Add(new Rectangle(new Point(0,0), blob_size));
            Invalidate();
        }

        private void TransparentForm_Paint(object sender, PaintEventArgs e) {
            foreach (var blob in blobs.ToList())
            {
                var transformed = new Rectangle(blob.X - blob.Width/2, blob.Y - blob.Height/2, blob.Width, blob.Height);
                e.Graphics.FillEllipse(brush, transformed);
            }

            if (rectangles.Any())
            {
                foreach (var rect in rectangles.ToList())
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        public void AddBlob(Point pos)
        {
            Rectangle blob = new Rectangle(pos, blob_size);
            blobs.Add(blob);
            Invalidate();
        }

        public void AddRect(Rectangle rect)
        {
            rectangles.Add(rect);
            Invalidate();
        }

        public void UpdateBlob(Point pos)
        {
            // To have multiple blobs working with MYO, we need some sort of identifier to map which MYO device controls which blob
            blobs[0] = new Rectangle(pos, blob_size);
            Invalidate();
        }

        public void UpdateRect(Rectangle rect)
        {
            rectangles[0] = rect;
            Invalidate();
        }

        public void DeleteBlob(Rectangle blob)
        {
            blobs.Remove(blob);
            Invalidate();
        }

        public void DeleteRect(Rectangle rect)
        {
            rectangles.Remove(rect);
            Invalidate();
        }

        public void Move(object o, GyroscopeDataEventArgs e)
        {
            if (absoluteTL != null && absoluteBR != null)
            {
                var myo = (Myo) o;
                var position = GetPixelPosition(myo);

                if (SelectedWindow != null)
                {
                    if (rectangles.Any())
                    {
                        UpdateRect(new Rectangle(position.X - DragOffset.Value.X, position.Y - DragOffset.Value.Y,
                            SelectedWindow.Area.Width, SelectedWindow.Area.Height));
                    }
                    else
                    {
                        AddRect(new Rectangle(position.X - DragOffset.Value.X, position.Y - DragOffset.Value.Y,
                            SelectedWindow.Area.Width, SelectedWindow.Area.Height));
                    }
                    if (blobs.Any())
                    {
                        blobs.ToList().ForEach(b => DeleteBlob(b));
                    }
                }
                else {
                    blobs[0] = new Rectangle(position, blob_size);
                }
                Invalidate();
            }
        }

        public void Pose(object o, PoseEventArgs e)
        {
            var myo = (Myo)o;
            if (myo.Pose == MyoSharp.Poses.Pose.DoubleTap)
            {
                if (absoluteTL == null)
                {
                    absoluteTL = Math3D.FromQuaternion(myo.Orientation);
                    Console.WriteLine("Calibrated Top Left!");
                    UpdateBlob(new Point(clientRes.Width, clientRes.Height));
                }
                else if (absoluteBR == null)
                {
                    absoluteBR = Math3D.FromQuaternion(myo.Orientation);
                    Console.WriteLine("Calibrated Bottom Right!");
                }
                else
                {
                    absoluteTL = null;
                    absoluteBR = null;
                    UpdateBlob(new Point(0, 0));
                }
            }
            else if (myo.Pose == MyoSharp.Poses.Pose.Fist)
            {
                if (absoluteTL != null && absoluteBR != null)
                {
                    var position = GetPixelPosition(myo);
                    var api = new WindowApi();
                    var window = api.WindowFromPoint(position);
                    if (window != null)
                    {
                        SelectedWindow = api.GetRoot(window);
                        if (SelectedWindow != null) {
                            if (DragOffset == null)
                            {
                                DragOffset = new Point(position.X - SelectedWindow.Area.Location.X, position.Y - SelectedWindow.Area.Location.Y);
                            }
                        }
                    }
                }
            }

            else
            {
                if (absoluteTL != null && absoluteBR != null)
                {
                    if (SelectedWindow != null)
                    {
                        var position = GetPixelPosition(myo);
                        var api = new WindowApi();
                        var newWindow = new Window(SelectedWindow.Ptr, new Rectangle(position.X - DragOffset.Value.X, position.Y - DragOffset.Value.Y, SelectedWindow.Area.Width, SelectedWindow.Area.Height));
                        SelectedWindow = null;
                        api.SetWindow(newWindow);
                        api.BringToFront(newWindow);
                        Rectangle blob = new Rectangle(position, blob_size);
                        blobs.Add(blob);
                    }
                    if (rectangles.Any()) {
                        rectangles.ToList().ForEach(r => DeleteRect(r));
                    }
                    DragOffset = null;
                }
            }
        }

        public Point GetPixelPosition(Myo myo)
        {
            var orientation = myo.Orientation;
            var eulerAngles = Math3D.FromQuaternion(orientation) - absoluteTL;
            eulerAngles = new Vector3F(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
            var vect = Math3D.DirectionalVector(eulerAngles);
            var position = Math3D.PixelFromVector(vect);

            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > clientRes.Width)
            {
                position.X = clientRes.Width;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > clientRes.Height)
            {
                position.Y = clientRes.Height;
            }
            return position;
        }
    }
}
