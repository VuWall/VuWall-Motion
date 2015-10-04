using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace vuwall_motion
{
    public class WindowApi
    {
        private IntPtr DesktopWindow { get; set; }
        public WindowApi()
        {
            DesktopWindow = GetDesktopWindow(DesktopWindowEnum.SysListView32);
        }

        public enum DesktopWindowEnum
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }

        public static IntPtr GetDesktopWindow(DesktopWindowEnum desktopWindow)
        {
            IntPtr _ProgMan = _GetShellWindow();
            IntPtr _SHELLDLL_DefViewParent = _ProgMan;
            IntPtr _SHELLDLL_DefView = _FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            IntPtr _SysListView32 = _FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

            if (_SHELLDLL_DefView == IntPtr.Zero)
            {
                _EnumWindows((hwnd, lParam) =>
                {
                    string className;
                    _GetClassName(hwnd, out className, int.MaxValue);
                    if (className == "WorkerW")
                    {
                        IntPtr child = _FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                        if (child != IntPtr.Zero)
                        {
                            _SHELLDLL_DefViewParent = hwnd;
                            _SHELLDLL_DefView = child;
                            _SysListView32 = _FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView"); ;
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }

            switch (desktopWindow)
            {
                case DesktopWindowEnum.ProgMan:
                    return _ProgMan;
                case DesktopWindowEnum.SHELLDLL_DefViewParent:
                    return _SHELLDLL_DefViewParent;
                case DesktopWindowEnum.SHELLDLL_DefView:
                    return _SHELLDLL_DefView;
                case DesktopWindowEnum.SysListView32:
                    return _SysListView32;
                default:
                    return IntPtr.Zero;
            }
        }

        public Window WindowFromPoint(Point p)
        {
            var ptr = _WindowFromPoint(p);
            if (ptr == DesktopWindow)
            {
                return null;
            }
            Rectangle rect;
            _GetWindowRect(ptr, out rect); 
            return new Window(ptr, rect);
        }

        public Window GetRoot(Window child)
        {
            var ptr = _GetAncestor(child.Ptr, 2);
            if (ptr == DesktopWindow)
            {
                return null;
            }
            Rectangle rect;
            _GetWindowRect(ptr, out rect);
            return new Window(ptr, rect);
        }

        public void SetWindow(Window window)
        {
            _SetWindowPos(window.Ptr, 0, window.Area.X, window.Area.Y, window.Area.Width, window.Area.Height, 0x0040);
        }

        public void BringToFront(Window window)
        {
            _SetForegroundWindow(window.Ptr);
        }

        public Window GetForegroundWindow()
        {
            var ptr = _GetForegroundWindow();
            if (ptr == DesktopWindow)
            {
                return null;
            }
            Rectangle rect;
            _GetWindowRect(ptr, out rect);
            return new Window(ptr, rect);
        }

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        public static extern IntPtr _WindowFromPoint(Point point);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr _SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern IntPtr _GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern IntPtr _SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr _GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetAncestor")]
        public static extern IntPtr _GetAncestor(IntPtr hWnd, uint gaFlags);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr _GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr _FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "GetShellWindow")]
        public static extern IntPtr _GetShellWindow();

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int _GetClassName(IntPtr hwnd, out string lpClassName, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumWindows")]
        public static extern bool _EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    }
}
