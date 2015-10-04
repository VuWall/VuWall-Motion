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
        private Dictionary<Myo, Window> SelectedWindow = new Dictionary<Myo, Window>();
        private Dictionary<Myo, Point?> DragOffset = new Dictionary<Myo, Point?>();

        public Dictionary<Myo, Vector3F> absoluteTL = new Dictionary<Myo, Vector3F>();

        private Size blob_size = new Size(50,50);

        public Dictionary<IMyo, Point> blobs = new Dictionary<IMyo, Point>();
        public Dictionary<IMyo, Rectangle> rectangles = new Dictionary<IMyo, Rectangle>();

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
            Invalidate();
        }

        private void TransparentForm_Paint(object sender, PaintEventArgs e) {
            foreach (var blob in blobs.ToList())
            {
                var transformed = new Rectangle(blob.Value.X - blob_size.Width/2, blob.Value.Y - blob_size.Height/2, blob_size.Width, blob_size.Height);
                e.Graphics.FillEllipse(brush, transformed);
            }

            if (rectangles.Any())
            {
                foreach (var rect in rectangles.ToList())
                {
                    e.Graphics.DrawRectangle(pen, rect.Value);
                }
            }
        }

        public void AddBlob(IMyo myo, Point pos)
        {
            blobs.Add(myo, pos);
            Invalidate();
        }

        public void AddRect(IMyo myo, Rectangle rect)
        {
            rectangles.Add(myo, rect);
            Invalidate();
        }

        public void UpdateBlob(IMyo myo, Point pos)
        {
            // To have multiple blobs working with MYO, we need some sort of identifier to map which MYO device controls which blob
            blobs[myo] = pos;
            Invalidate();
        }

        public void UpdateRect(IMyo myo, Rectangle rect)
        {
            rectangles[myo] = rect;
            Invalidate();
        }

        public void DeleteBlob(IMyo myo)
        {
            blobs.Remove(myo);
            Invalidate();
        }

        public void DeleteRect(IMyo myo)
        {
            rectangles.Remove(myo);
            Invalidate();
        }

        public void Move(object o, GyroscopeDataEventArgs e)
        {
            var myo = (Myo) o;
            if (absoluteTL.ContainsKey(myo))
            {
                var position = GetPixelPosition(myo);

                if (SelectedWindow.ContainsKey(myo))
                {
                    if (rectangles.ContainsKey(myo))
                    {
                        UpdateRect(myo, new Rectangle(position.X - DragOffset[myo].Value.X, position.Y - DragOffset[myo].Value.Y,
                            SelectedWindow[myo].Area.Width, SelectedWindow[myo].Area.Height));
                    }
                    else
                    {
                        AddRect(myo, new Rectangle(position.X - DragOffset[myo].Value.X, position.Y - DragOffset[myo].Value.Y,
                            SelectedWindow[myo].Area.Width, SelectedWindow[myo].Area.Height));
                    }
                    if (blobs.ContainsKey(myo))
                    {
                        DeleteBlob(myo);
                    }
                }
                else {
                    blobs[myo] = position;
                }
                Invalidate();
            }
        }

        public void Pose(object o, PoseEventArgs e)
        {
            var myo = (Myo)o;
            if (myo.Pose == MyoSharp.Poses.Pose.DoubleTap)
            {
                if (!absoluteTL.ContainsKey(myo))
                {
                    absoluteTL.Add(myo, Math3D.FromQuaternion(myo.Orientation));
                    Console.WriteLine("Calibrated Top Left!");
                    UpdateBlob(myo, new Point(clientRes.Width, clientRes.Height));
                }
                else
                {
                    absoluteTL.Remove(myo);
                    UpdateBlob(myo, new Point(0, 0));
                }
            }
            else if (myo.Pose == MyoSharp.Poses.Pose.Fist)
            {
                if (absoluteTL.ContainsKey(myo))
                {
                    var position = GetPixelPosition(myo);
                    var api = new WindowApi();
                    var window = api.WindowFromPoint(position);
                    if (window != null)
                    {
                        SelectedWindow.Add(myo, api.GetRoot(window));
                        if (SelectedWindow.ContainsKey(myo)) {
                            if (!DragOffset.ContainsKey(myo))
                            {
                                DragOffset.Add(myo, new Point(position.X - SelectedWindow[myo].Area.Location.X, position.Y - SelectedWindow[myo].Area.Location.Y));
                            }
                        }
                    }
                }
            }

            else
            {
                if (absoluteTL != null)
                {
                    if (SelectedWindow.ContainsKey(myo))
                    {
                        var position = GetPixelPosition(myo);
                        var api = new WindowApi();
                        var newWindow = new Window(SelectedWindow[myo].Ptr, new Rectangle(position.X - DragOffset[myo].Value.X, position.Y - DragOffset[myo].Value.Y, SelectedWindow[myo].Area.Width, SelectedWindow[myo].Area.Height));
                        SelectedWindow.Remove(myo);
                        api.SetWindow(newWindow);
                        api.BringToFront(newWindow);
                        blobs.Add(myo, position);
                    }
                    if (rectangles.ContainsKey(myo)) {
                        DeleteRect(myo);
                    }
                    DragOffset.Remove(myo);
                }
            }
        }

        public Point GetPixelPosition(Myo myo)
        {
            var orientation = myo.Orientation;
            var eulerAngles = Math3D.FromQuaternion(orientation) - absoluteTL[myo];
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
