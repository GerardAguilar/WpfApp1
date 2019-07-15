using Microsoft.Test.VisualVerification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchAuto
{
    public unsafe class ImageComparer
    {

        public void Compare(Bitmap _bmp, BitmapData _bmd)
        {
            int _pixelSize = 3;
            byte* _current = (byte*)(void*)_bmd.Scan0;
            int _nWidth = _bmp.Width * _pixelSize;
            int _nHeight = _bmp.Height;
        }

        public Snapshot Screenshot(int width, int height)
        {
            Snapshot snapshot = Snapshot.FromRectangle(new System.Drawing.Rectangle(0, 0, width, height));
            //Snapshot snapshot = Snapshot.
            //snapshot.ToFile("Actual.png", ImageFormat.Png);

            return snapshot;
        }

        public Bitmap ScreenshotLockBits(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            //Snapshot snapshot = Snapshot.FromBitmap(bmp);
            Console.WriteLine("ScreenshotLockBits end");
            return bmp;
            //return snapshot;
        }

        public void CompareImages(Snapshot expected, Snapshot actual, String diffName)
        {
            Snapshot difference = actual.CompareTo(expected);
            difference.ToFile(diffName, ImageFormat.Png);
            //return difference;
        }

    }
}
