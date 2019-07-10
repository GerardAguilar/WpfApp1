using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium.Windows;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Interactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Test.VisualVerification;
using System.Drawing.Imaging;
using System.Drawing;

namespace WpfApp1
{
    public partial class RecordPage : Window
    { 
        private System.Windows.Point lastTouchDownPoint;
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        protected WindowsDriver<WindowsElement> session;
        private List<TouchEventArgs> touchEventList;
        private List<Coordinate> eventList;
        private bool record = false;
        private bool flip = false;
        protected JsonSimpleWrapper jsonSimpleWrapper;
        protected String installDirectory;
        protected const int TIMEOUT = 3600; //1 hour
        protected Snapshot expected;
        protected int width = 1920;
        protected int height = 1080;
        ImageComparer imageComparer;
        Window parent;

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

        public RecordPage()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            appCapabilities.SetCapability("newCommandTimeout", TIMEOUT);
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            //session.Manage().Window.Maximize();
            InitializeComponent();
            this.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);
            touchEventList = new List<TouchEventArgs>();
            eventList = new List<Coordinate>();
            jsonSimpleWrapper = new JsonSimpleWrapper();
            installDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            imageComparer = new ImageComparer();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        public void ToggleView(Window source) {
            this.Show();
            parent = source;
            parent.Hide();
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString() + " held down");
            switch (e.Key.ToString())
            {
                case "D1":
                    record = !record;
                    if (record)
                    {
                        Console.WriteLine("Recording ON");
                        this.Opacity = 0.25;
                    }
                    else {
                        Console.WriteLine("Recording OFF");
                        this.Opacity = 0.50;
                    }
                    break;
                case "D2":
                    RepeatTaps(eventList);
                    break;
                case "D3":
                    parent.Show();
                    this.Close();                    
                    break;
                case "D4":
                    jsonSimpleWrapper.WriteEvents(eventList);
                    Console.WriteLine("List count: " + eventList.Count);
                    jsonSimpleWrapper.SaveEvents(installDirectory);
                    break;
                case "D5":
                    RepeatTaps(jsonSimpleWrapper.LoadEvent(installDirectory + "events.json"));
                    break;
                case "D6":
                    Console.WriteLine("Clearing Events");
                    eventList = new List<Coordinate>();
                    jsonSimpleWrapper.ClearEvents();
                    jsonSimpleWrapper.WriteEvents(eventList);
                    jsonSimpleWrapper.SaveEvents(installDirectory);
                    break;
                case "D7":
                    Console.WriteLine("Screenshot");
                    expected = imageComparer.Screenshot(width, height);
                    break;
                case "D8":
                    imageComparer.CompareImages(expected, imageComparer.Screenshot(width, height));
                    break;
                default:
                    break;
            }
        }

        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {
            Console.WriteLine("Touch");
            flip = !flip;
            this.lastTouchDownPoint = e.GetTouchPoint(this).Position;

            if (record)
            {
                Coordinate newCoordinate = new Coordinate(
                        (int)e.GetTouchPoint(this).Position.X,
                        (int)e.GetTouchPoint(this).Position.Y,
                        e.Timestamp);

                eventList.Add(newCoordinate);
            }
            this.Hide();
            Tap(this.lastTouchDownPoint.X, this.lastTouchDownPoint.Y);
            this.Show();                       
        }

        private void Tap(TouchEventArgs e) {
            
            Tap(this.lastTouchDownPoint.X, this.lastTouchDownPoint.Y);
        }

        private void Tap(double xOffset, double yOffset) {
            int x;
            double xf;
            int.TryParse(this.lastTouchDownPoint.X.ToString(), out x);
            if (x == 0)
            {
                double.TryParse(this.lastTouchDownPoint.X.ToString(), out xf);
                x = (int)xf;
            }
            int y;
            double yf;
            int.TryParse(this.lastTouchDownPoint.Y.ToString(), out y);
            if (y == 0)
            {
                double.TryParse(this.lastTouchDownPoint.Y.ToString(), out yf);
                y = (int)yf;
            }
            Console.WriteLine("Tap: " + x + ", " + y);
            Tap(x, y);
        }

        private void Tap(int xOffset, int yOffset)
        {
            System.Threading.Thread.Sleep(100);
            //the screenshot can take the place of sleeping instead

            Console.WriteLine("Tap start");
            PointerInputDevice touch = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touchSequence = new ActionSequence(touch, 0);
            touchSequence.AddAction(touch.CreatePointerMove(CoordinateOrigin.Pointer, xOffset, yOffset, TimeSpan.Zero));
            touchSequence.AddAction(touch.CreatePointerDown(PointerButton.TouchContact));
            touchSequence.AddAction(touch.CreatePointerUp(PointerButton.TouchContact));
            List<ActionSequence> actions = new List<ActionSequence> { touchSequence };
            session.PerformActions(actions);

            String filename = "Tap_" + DateTime.Now.ToString("yyyymmdd-HHmm-ssfff") + "_" + xOffset + "_" + yOffset + ".png";
            Bitmap bmp = imageComparer.ScreenshotLockBits(width, height);
            bmp.Save(filename, ImageFormat.Png);

            Console.WriteLine("Echo: " + actions[actions.Count - 1].ToString());            
            Console.WriteLine("Tap end");
        }
        
        //public static Bitmap getDifferenceImage(Bitmap img1, Bitmap img2)
        //{
        //    int width1 = img1.Width; // Change - getWidth() and getHeight() for BufferedImage
        //    int width2 = img2.Width; // take no arguments
        //    int height1 = img1.Height;
        //    int height2 = img2.Height;
        //    if ((width1 != width2) || (height1 != height2))
        //    {
        //        Console.WriteLine("Incorrect image sizes");
        //    }

        //    // NEW - Create output Buffered image of type RGB
        //    Bitmap outImg = new Bitmap(width1, height1);

        //    // Modified - Changed to int as pixels are ints
        //    int diff;
        //    int result; // Stores output pixel
        //    for (int i = 0; i < height1; i++)
        //    {
        //        for (int j = 0; j < width1; j++)
        //        {
        //            int rgb1 = img1.getRGB(j, i);
        //            int rgb2 = img2.getRGB(j, i);
        //            int r1 = (rgb1 >> 16) & 0xff;
        //            int g1 = (rgb1 >> 8) & 0xff;
        //            int b1 = (rgb1) & 0xff;
        //            int r2 = (rgb2 >> 16) & 0xff;
        //            int g2 = (rgb2 >> 8) & 0xff;
        //            int b2 = (rgb2) & 0xff;
        //            diff = Math.abs(r1 - r2); // Change
        //            diff += Math.abs(g1 - g2);
        //            diff += Math.abs(b1 - b2);
        //            diff /= 3; // Change - Ensure result is between 0 - 255
        //                       // Make the difference image gray scale
        //                       // The RGB components are all the same
        //            result = (diff << 16) | (diff << 8) | diff;
        //            outImg.setRGB(j, i, result); // Set result
        //        }
        //    }

        //    // Now return
        //    return outImg;
        //}

        public void RepeatTaps(List<Coordinate> list)
        {
            this.Hide();
            Console.WriteLine("RepeatTaps()");
            //Point point;
            int x;
            int y;
            int timeDiff;
            for (int i = 0; i < list.Count; i++)
            {
                //point = list[i].GetTouchPoint(this).Position;
                x = list[i].getX();
                y = list[i].getY();
                Tap(x,y);
                //take into consideration the end of the list
                if (i == (list.Count - 1))
                {
                    timeDiff = 0;
                    list[i].setTimeDiff(timeDiff);
                }
                else {
                    //take into consideration the time between the next step and the current one.
                    //Wait for that amount of time
                    Console.WriteLine(list[i + 1].getTimestamp() + " - " + list[i].getTimestamp() + " = " + (list[i + 1].getTimestamp() - list[i].getTimestamp()));
                    timeDiff = list[i + 1].getTimestamp() - list[i].getTimestamp();
                    //timeDiff = timeDiff - 200;//adjusts for transparency transitions
                    if (timeDiff < 0)
                    {
                        Console.WriteLine("timeDiff is less than 0");
                        timeDiff = 0;
                    }
                    list[i].setTimeDiff(timeDiff);
                    Console.WriteLine("Waiting for : " + timeDiff + " ms");                    
                    System.Threading.Thread.Sleep(timeDiff);
                }                
            }
            this.Show();
        }

        private void PrintTaps(List<Coordinate> list) {
            Console.WriteLine("PrintTaps()");
            for (int i = 0; i < list.Count; i++) {
                list[i].printCoordinate();
            }
        }
    }
}