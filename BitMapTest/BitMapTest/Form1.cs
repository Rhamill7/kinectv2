using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            //read image
            Bitmap bitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");

            //load
            pictureBox1.Image = bitmap;
        }
    }
}
