using Microsoft.Win32;
using OpenQA.Selenium.Appium.Interactions;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ReplayPage : Window
    {
        protected Window parent;
        protected List<String> eventFilesList;
        protected WindowsDriver<WindowsElement> session;
        protected const int TIMEOUT = 3600; //1 hour
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        protected ImageComparer imageComparer;
        protected int width = 1920;
        protected int height = 1080;
        protected JsonSimpleWrapper jsonSimpleWrapper;

        public ReplayPage()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            appCapabilities.SetCapability("newCommandTimeout", TIMEOUT);
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            imageComparer = new ImageComparer();
            jsonSimpleWrapper = new JsonSimpleWrapper();
            eventFilesList = new List<String>();
            InitializeComponent();
        }

        public void ToggleView(Window source) {
            parent = source;
            this.Show();
            source.Hide();
        }

        public void LoadFile(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JSON file (*.json)|*.json| All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) {
                foreach (string filename in openFileDialog.FileNames) {
                    JsonList.Items.Add(filename);
                    eventFilesList.Add(filename);
                }
            };
            foreach (string filename in JsonList.Items) {
                Console.WriteLine("Loaded file: " + filename);
            }
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

            String filename = "Tap_" + System.DateTime.Now.ToString("yyyymmdd-HHmm-ssfff") + "_" + xOffset + "_" + yOffset + ".png";
            Bitmap bmp = imageComparer.ScreenshotLockBits(width, height);
            bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);

            Console.WriteLine("Echo: " + actions[actions.Count - 1].ToString());
            Console.WriteLine("Tap end");
        }

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
                Tap(x, y);
                //take into consideration the end of the list
                if (i == (list.Count - 1))
                {
                    timeDiff = 0;
                    list[i].setTimeDiff(timeDiff);
                }
                else
                {
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

        public void ReplayListed(object e, RoutedEventArgs args) {
            foreach (string filename in eventFilesList) {
                RepeatTaps(jsonSimpleWrapper.LoadEvent(filename));
            }            
        }
    }
}
