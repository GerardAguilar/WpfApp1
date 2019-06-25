using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
//using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Windows;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Interactions;

using System.Drawing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Appium.Windows;
//using OpenQA.Selenium.Remote;
//using System;
//using System.Net;
//using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TouchableThing : Window
    { 
        private System.Windows.Point lastTouchDownPoint;
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        private const string Paint3DAppId = @"Microsoft.MSPaint_8wekyb3d8bbwe!Microsoft.MSPaint";
        public Paint3DSession instance;

        public TouchableThing()
        {
            InitializeComponent();
            instance = new Paint3DSession();
            this.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);
            this.MouseDown += new MouseButtonEventHandler(TouchableThing_MouseDown);
        }
        
        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {
            this.lastTouchDownPoint = e.GetTouchPoint(this.TouchRectangle).Position;
            Console.WriteLine("Touch: " + this.lastTouchDownPoint.ToString());
            Tap(0, 0, e);
        }

        private void TouchableThing_MouseDown(object sender, MouseEventArgs e) {
            Console.WriteLine("Mouse: " + e.GetPosition(this.TouchRectangle).ToString());
        }

        // Note: append /wd/hub to the URL if you're directing the test at Appium
        //private const string Paint3DAppId = "C:\\Paint3D";

        //protected static WindowsDriver<WindowsElement> DesktopSession;

        //public void Setup(TestContext context)
        //{

        //        // Create a new session to launch Paint 3D application
        //        DesiredCapabilities appCapabilities = new DesiredCapabilities();
        //        appCapabilities.SetCapability("app", Paint3DAppId);
        //        appCapabilities.SetCapability("deviceName", "WindowsPC");
        //        session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

        //        //// Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
        //        //session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

        //        //// Maximize Paint 3D window to ensure all controls being displayed
        //        //session.Manage().Window.Maximize();

        //}

        //[TestMethod]
        //public void TapTest4()
        //{
        //    Activate(21572);
        //    TapTest3();
        //}

        //public void Activate(int processId)
        //{
        //    //Process currentProcess = Process.GetCurrentProcess();
        //    Process currentProcess = Process.GetProcessById(processId);
        //    IntPtr hWnd = currentProcess.MainWindowHandle;
        //    if (hWnd != User32.InvalidHandleValue)
        //    {
        //        User32.SetForegroundWindow(hWnd);
        //        //User32.ShowWindow(hWnd, User32.SW_MAXIMIZE);
        //    }
        //    else
        //    {
        //        Console.WriteLine("hWnd is invalid");
        //    }
        //}

        private void Tap(System.Windows.Point point, TouchEventArgs e) {
            Tap(point.X, point.Y, e);
        }

        private void Tap(double xOffset, double yOffset, TouchEventArgs e)
        {
            PointerInputDevice touch = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touchSequence = new ActionSequence(touch, 0);
            //touchSequence.AddAction(touch.CreatePointerMove(, xOffset, yOffset, TimeSpan.Zero));//need some kind of web element to reference movement
            touchSequence.AddAction(touch.CreatePointerDown(PointerButton.TouchContact));
            touchSequence.AddAction(touch.CreatePointerUp(PointerButton.TouchContact));
            //touchSequence.AddAction(touch.CreatePointerDown(OpenQA.Selenium.Interactions.MouseButton.Left));
            //touchSequence.AddAction(touch.CreatePointerDown(OpenQA.Selenium.Appium.Interactions.PointerButton.TouchContact));
            //instance.session.PerformActions(new List<ActionSequence> { touchSequence });
            instance.DoActions(new List<ActionSequence> { touchSequence });
        }
    }


    public class Paint3DSession
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        private const string Paint3DAppId = @"Microsoft.MSPaint_8wekyb3d8bbwe!Microsoft.MSPaint";
        //private const string Paint3DAppId = "C:\\Paint3D";

        protected static WindowsDriver<WindowsElement> session;
        //protected static WindowsDriver<WindowsElement> DesktopSession;

        public Paint3DSession()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", Paint3DAppId);
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

            //DesiredCapabilities rootCapabilities = new DesiredCapabilities();
            //rootCapabilities.SetCapability("platformName", "Windows");
            //rootCapabilities.SetCapability("deviceName", "WindowsPC");
            //rootCapabilities.SetCapability("app", "Root");
            //DesktopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/wd/hub"), rootCapabilities);

            ////setup session
            //var ApplicationWindow = DesktopSession.FindElementByName(applicationTitle);
            //var ApplicationTopLevelWindowHandle = ApplicationWindow.GetAttribute("NativeWindowHandle");
            //ApplicationTopLevelWindowHandle = (int.Parse(ApplicationTopLevelWindowHandle)).ToString("x");//Convert to Hex
            //DesiredCapabilities appCapabilities = new DesiredCapabilities();
            //appCapabilities.SetCapability("platformName", "Windows");
            //appCapabilities.SetCapability("deviceName", "WindowsPC");
            //appCapabilities.SetCapability("appTopLevelWindow", ApplicationTopLevelWindowHandle);
            //session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
        }

        public void DoActions(List<ActionSequence> actions) {
            session.PerformActions(actions);
        }
    }
    
}
