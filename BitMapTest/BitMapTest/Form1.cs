using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace BitMapTest
{
    public partial class Bitmaptest : Form
    {   
        //Global Variables for pixel data?
        Color[,] pixelData = null;
//        Point P = new Point (0,0);

        public Bitmaptest()
        {
            InitializeComponent();
        }

        private void Bitmaptest_Load(object sender, EventArgs e)
        {
            int x = 0; int y = 0;

            //read image
            // Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            Bitmap myBitmap = getBitmap();  
            ///Get Data
            //Color[,]
                pixelData = new Color[myBitmap.Width, myBitmap.Height];

            for (y = 0; y < myBitmap.Height; y++)
                for (x = 0; x < myBitmap.Width; x++)
                    pixelData[x, y] = myBitmap.GetPixel(x, y);
           
            //loadimage onto Screen
            pictureBox1.Image = myBitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point P = new Point(e.X,e.Y);

            for (int i = 0; i < pixelData.Length; i++) {
                if (pixelData.GetValue(i).Equals(P))
                {

                    Bitmap myBitmap = getBitmap();
                    myBitmap.SetPixel(e.X, e.Y, Color.Red);
                    pictureBox1.Image = myBitmap;
                }
               }

        //    BitmapSource bSource = new BitmapImage(new Uri("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png"));
           // BitmapSource img = pictureBox1.Image.get
        //    int stride = bSource.PixelWidth * 4;
       //     int size = bSource.PixelHeight * stride;
       //     byte[] pixels = new byte[size];
        //    bSource.CopyPixels(pixels, stride, 0);

        }

        public Bitmap getBitmap()
        {

            Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            return myBitmap;
        }

    }
}
