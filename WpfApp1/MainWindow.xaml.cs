using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium.Windows;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Interactions;

namespace WpfApp1
{
    public partial class TouchableThing : Window
    { 
        private System.Windows.Point lastTouchDownPoint;
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        private const string Paint3DAppId = @"Microsoft.MSPaint_8wekyb3d8bbwe!Microsoft.MSPaint";
        protected WindowsDriver<WindowsElement> session;

        #region User32 methods
        //[DllImport("user32.dll")]
        //static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        //{
        //    int error = 0;
        //    IntPtr result = IntPtr.Zero;
        //    // Win32 SetWindowLong doesn't clear error on success
        //    SetLastError(0);

        //    if (IntPtr.Size == 4)
        //    {
        //        // use SetWindowLong
        //        Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
        //        error = Marshal.GetLastWin32Error();
        //        result = new IntPtr(tempResult);
        //    }
        //    else
        //    {
        //        // use SetWindowLongPtr
        //        result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
        //        error = Marshal.GetLastWin32Error();
        //    }

        //    if ((result == IntPtr.Zero) && (error != 0))
        //    {
        //        throw new System.ComponentModel.Win32Exception(error);
        //    }

        //    return result;
        //}

        //[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        //private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        //[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        //private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        //private static int IntPtrToInt32(IntPtr intPtr)
        //{
        //    return unchecked((int)intPtr.ToInt64());
        //}

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);

        //[DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        //public static extern void SetLastError(int dwErrorCode);


        ////[DllImport("user32.dll")]
        ////static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        //[DllImport("User32.dll")]
        //internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //internal static readonly IntPtr InvalidHandleValue = IntPtr.Zero;
        //internal const int SW_MAXIMIZE = 3;

        //public const int GWL_EXSTYLE = -20;
        //public const int WS_EX_LAYERED = 0x80000;
        //public const int WS_EX_TRANSPARENT = 32;
        //public const int LWA_ALPHA = 0x2;
        //public const int LWA_COLORKEY = 0x1;
        #endregion

        public TouchableThing()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            //session.Manage().Window.Maximize();
            InitializeComponent();
            this.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);
        }

        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {
            this.lastTouchDownPoint = e.GetTouchPoint(this).Position;            
            this.Hide();
            Tap(e);
            this.Show();
        }

        private void Tap(TouchEventArgs e) {
            Console.WriteLine("Touch: " + this.lastTouchDownPoint.ToString());
            int x;
            float xf;
            int.TryParse(this.lastTouchDownPoint.X.ToString(), out x);
            if (x == 0)
            {
                float.TryParse(this.lastTouchDownPoint.X.ToString(), out xf);
                x = (int)xf;
            }
            int y;
            float yf;
            int.TryParse(this.lastTouchDownPoint.Y.ToString(), out y);
            if (y == 0)
            {
                float.TryParse(this.lastTouchDownPoint.Y.ToString(), out yf);
                y = (int)yf;
            }
            Tap(x, y);
        }

        private void Tap(int xOffset, int yOffset)
        {
            System.Threading.Thread.Sleep(200);
            Console.WriteLine("Tap start");
            PointerInputDevice touch = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touchSequence = new ActionSequence(touch, 0);
            touchSequence.AddAction(touch.CreatePointerMove(CoordinateOrigin.Pointer, xOffset, yOffset, TimeSpan.Zero));
            touchSequence.AddAction(touch.CreatePointerDown(PointerButton.TouchContact));
            touchSequence.AddAction(touch.CreatePointerUp(PointerButton.TouchContact));
            List<ActionSequence> actions = new List<ActionSequence> { touchSequence };
            session.PerformActions(actions);
            Console.WriteLine("Echo: " + actions[actions.Count - 1].ToString());
            Console.WriteLine("Tap end");
            System.Threading.Thread.Sleep(200);            
        }
    }
}


