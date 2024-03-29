﻿using Microsoft.Win32;
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
using System.IO;
using System.Drawing.Imaging;
using Microsoft.Test.VisualVerification;
using System.Diagnostics;

namespace TouchAuto
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
        protected String installDirectory;
        protected int currentEventTapCount;
        protected DesiredCapabilities appCapabilities;

        public ReplayPage()
        {
            appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            appCapabilities.SetCapability("newCommandTimeout", TIMEOUT);
            
            imageComparer = new ImageComparer();
            jsonSimpleWrapper = new JsonSimpleWrapper();
            eventFilesList = new List<String>();
            installDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            currentEventTapCount = 0;
            InitializeComponent();
        }

        public void ToggleView(Window source) {
            parent = source;
            this.Show();
            parent.Hide();
        }

        public void LoadFile(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JSON file (*.json)|*.json| All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) {
                foreach (string filename in openFileDialog.FileNames) {
                    //JsonList.Items.Add(filename);
                    JsonList.Items.Add(filename);
                    eventFilesList.Add(filename);
                                     
                }
            };
            foreach (string filename in JsonList.Items) {
                Console.WriteLine("Loaded file: " + filename);
            }
        }

        //private void PreviewImages(String fileURL) {
        //    System.Windows.Controls.Image im2 = new System.Windows.Controls.Image();
        //    System.Windows.Controls.Image im3 = new System.Windows.Controls.Image();
        //    BitmapImage bmp = new BitmapImage();
        //    //String filename = eventFolder + "\\" + currentEventName + "_" + currentEventTapCount + ".png";
        //    String directory = GetParentDirectory(fileURL);
        //    String filename = GetFilename(fileURL);
        //    Console.WriteLine("Full Path: " + directory + filename);
        //    bmp.BeginInit();
        //    bmp.UriSource = new Uri(directory+filename+"_0.png");
        //    bmp.EndInit();
        //    TransformedBitmap resizedBitmap = new TransformedBitmap(bmp, new ScaleTransform(.25, .25));
        //    im2.Source = resizedBitmap;
        //    im3.Source = resizedBitmap;
        //    //Image1.Source = bmp;
        //    ImageList.Items.Add(im2);
        //    ImageList.Items.Add(im3);
        //}

        private void PreviewImages(String fileURL) {
            String directory = GetParentDirectory(fileURL);
            String filename = GetFilename(fileURL);
            int fCount = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly).Length;
            for (int i = 0; i < (fCount/3)-1; i++) {//-1 to exclude the json file
                System.Windows.Controls.Image im = new System.Windows.Controls.Image();
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(directory + filename + "_" + i + "_diff.png");
                bmp.EndInit();
                im.Source = new TransformedBitmap(bmp, new ScaleTransform(.25, .25));
                ImageList.Items.Add(im);
            }
        }

        private void PreviewImages(object e, RoutedEventArgs args) {
            Console.WriteLine("Item was selected");
        }

        private String GetParentDirectory(String fullPath) {
            int lastBackslash = fullPath.LastIndexOf('\\');
            String parentDirectory = fullPath.Substring(0, lastBackslash);
            Console.WriteLine("ParentDirectory: " + parentDirectory);
            return parentDirectory;
        }
        private String GetFilename(String fullPath) {
            int lastBackslash = fullPath.LastIndexOf('\\');
            String filename = fullPath.Substring(lastBackslash, fullPath.Length - lastBackslash - 5);//the -4 is to omit the ".json"
            Console.WriteLine("Filename: " + filename);

            return filename;
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

            ////String filename = "Tap_" + System.DateTime.Now.ToString("yyyymmdd-HHmm-ssfff") + "_" + xOffset + "_" + yOffset + ".png";
            ////Bitmap bmp = imageComparer.ScreenshotLockBits(width, height);
            ////bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            
            
            Console.WriteLine("Echo: " + actions[actions.Count - 1].ToString());
            Console.WriteLine("Tap end");
        }

        public void RepeatTaps(List<Coordinate> list, String filename)
        {
            
            Console.WriteLine("RepeatTaps()");
            //Point point;
            int x;
            int y;
            int timeDiff;
            for (int i = 0; i < list.Count; i++)
            {
                x = list[i].getX();
                y = list[i].getY();
                Tap(x, y);

                //from filename, save current
                String eventFolder = installDirectory + "\\" + GetFilename(filename);
                String currentFileName = eventFolder + "\\" + GetFilename(filename) + "_" + i + "_current.png";
                String baselineFileName = eventFolder + "\\" + GetFilename(filename) + "_" + i + ".png";
                String diffFileName = eventFolder + "\\" + GetFilename(filename) + "_" + i + "_diff.png";
            
                Bitmap bmp = imageComparer.ScreenshotLockBits(width, height);
                bmp.Save(currentFileName, ImageFormat.Png);

                //from filename, get baseline
                Snapshot expected = Snapshot.FromFile(baselineFileName);
                imageComparer.CompareImages(expected, Snapshot.FromBitmap(bmp), diffFileName);

                //Now taken cared off in the JsonSimpleWrapper
                ////take into consideration the end of the list
                //if (i == (list.Count - 1))
                //{
                //    timeDiff = 0;
                //    list[i].setTimeDiff(timeDiff);
                //}
                //else
                //{
                //    //take into consideration the time between the next step and the current one.
                //    //Wait for that amount of time
                //    Console.WriteLine(list[i + 1].getTimestamp() + " - " + list[i].getTimestamp() + " = " + (list[i + 1].getTimestamp() - list[i].getTimestamp()));
                //    timeDiff = list[i + 1].getTimestamp() - list[i].getTimestamp();
                //    if (timeDiff < 0)
                //    {
                //        Console.WriteLine("timeDiff is less than 0");
                //        timeDiff = 0;
                //    }
                //    list[i].setTimeDiff(timeDiff);
                //    Console.WriteLine("Waiting for : " + timeDiff + " ms");
                //    System.Threading.Thread.Sleep(timeDiff);
                //}

                System.Threading.Thread.Sleep((int)list[i].getTimeDiff());
            }
        }

        public void ReplayListed(object e, RoutedEventArgs args) {
            this.Hide();
            //Array.ForEach(Process.GetProcessesByName("WinAppDriver"), x => x.Kill());
            if (session != null) {
                session.Close();
                //Array.ForEach(Process.GetProcessesByName("WinAppDriver"), x => x.Kill());
            }
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            foreach (string filename in eventFilesList) {
                RepeatTaps(jsonSimpleWrapper.LoadEvent(filename), filename);
            }
            session.Close();
            this.Show();
            foreach (string filename in eventFilesList) {
                PreviewImages(filename);
            }            
        }

        public void ReturnToMainWindow(object e, RoutedEventArgs args) {
            parent.Show();            
            this.Hide();
        }
    }
}
