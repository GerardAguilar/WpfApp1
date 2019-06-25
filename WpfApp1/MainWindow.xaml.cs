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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TouchableThing : Window
    { 
        private Point lastTouchDownPoint;
        
        public TouchableThing()
        {
            InitializeComponent();
            this.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);
            this.MouseDown += new MouseButtonEventHandler(TouchableThing_MouseDown);
        }
        
        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {
            this.lastTouchDownPoint = e.GetTouchPoint(this.TouchRectangle).Position;
            Console.WriteLine(this.lastTouchDownPoint.ToString());
        }

        private void TouchableThing_MouseDown(object sender, MouseEventArgs e) {
            Console.WriteLine(e.GetPosition(this.TouchRectangle).ToString());
        }

        [TestMethod]
        public void TapTest4()
        {
            Activate(21572);
            TapTest3();
        }

        public void Activate(int processId)
        {
            //Process currentProcess = Process.GetCurrentProcess();
            Process currentProcess = Process.GetProcessById(processId);
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != User32.InvalidHandleValue)
            {
                User32.SetForegroundWindow(hWnd);
                //User32.ShowWindow(hWnd, User32.SW_MAXIMIZE);
            }
            else
            {
                Console.WriteLine("hWnd is invalid");
            }
        }

        private void Tap(int xOffset, int yOffset)
        {
            PointerInputDevice touch = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touchSequence = new ActionSequence(touch, 0);
            touchSequence.AddAction(touch.CreatePointerMove(zoomInteractor, xOffset, yOffset, TimeSpan.Zero));
            touchSequence.AddAction(touch.CreatePointerDown(PointerButton.TouchContact));
            touchSequence.AddAction(touch.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { touchSequence });
        }
    }
}
