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
        public Bitmaptest()
        {
            InitializeComponent();
        }

        private void Bitmaptest_Load(object sender, EventArgs e)
        {

          //  pictureBox1.Image = System.Drawing.Image.FromFile("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            //read image
            BitmapSource bitmap = new BitmapImage((new Uri("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png")));

            //load
            pictureBox1.Image = bitmap;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {

            BitmapSource bSource = new BitmapImage(new Uri("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png"));
           // BitmapSource img = pictureBox1.Image.get

           // int stride = pictureBox1.Image.PixelWidth * 4;
            int size = img.PixelHeight * stride;
            byte[] pixels = new byte[size];
            img.CopyPixels(pixels, stride, 0);

        }
    }
}
