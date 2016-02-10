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
        //Color[,] pixelData = null;
        List<Point> pixelPoint = new List<Point>();

//        Point P = new Point (0,0);

        public Bitmaptest()
        {
            InitializeComponent();
        }

        private void Bitmaptest_Load(object sender, EventArgs e)
        {
           // int x = 0; int y = 0;
           // string bob = null;
            //read image
            // Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            Bitmap myBitmap = getBitmap();  
            ///Get Data
            //make pixel data new colour array of the size of bitmapped image height and width
           //     pixelData = new Color[myBitmap.Width, myBitmap.Height];

            //for each y value 
            //for (y = 0; y < myBitmap.Height; y++)
            //{
                //for each x value
               // for (x = 0; x < myBitmap.Width; x++)
                //{
                  //  pixelPoint.Add(new Point (x,y));
              //      pixelData[x, y] = myBitmap.GetPixel(x, y);

                    //testing methods
               //     bob = myBitmap.GetPixel(x, y).ToString();
                 //   MessageBox.Show(bob);
                //}
         //   }
           

            //loadimage onto Screen
            pictureBox1.Image = myBitmap;

            // add event handle to enable click
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
        }
      //  private void pict

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point P = new Point(e.X,e.Y);
          

            //for (int i = 0; i < pixelData.Length ; i++)
            //{
               // for (int j = 0; j < pixelData.Length; i++)
              //  {
               //     string bob = pixelData.GetValue(i).ToString(); 
                  //  pixelData.GetValue
                    //bob = pixelData.ToString
                   // MessageBox.Show(bob);
                       Bitmap myBitmap = getBitmap();
                      myBitmap.SetPixel(e.X, e.Y, Color.Red);
              pixelPoint.Add(new Point (e.X,e.Y));

            pictureBox1.Image = myBitmap;
             //   }
            //}
         }
            
           //     else {
             //       MessageBox.Show("Please try again.");
               // }
               

        //}

        public Bitmap getBitmap()
        {

            Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            return myBitmap;
        }

    }
}
