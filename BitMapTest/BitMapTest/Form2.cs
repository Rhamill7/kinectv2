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
using Microsoft.VisualBasic;
using BitmapTest;

namespace BitmapTest
{
    public partial class Form2 : Form
    {

        private int frameNo;
        private Bitmap myBitMap;
      //  private string filePath;
        Tracking track = new Tracking();
        MenuItems menu = new MenuItems();

        public Form2(Tracking track)
        {
            this.track = track;
            InitializeComponent();
        }

        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Point P =  track.getJointLocations(frameNo);
            MessageBox.Show(P.ToString(
                ));
            //Bitmap myBitmap = pictureBox1.Image();
            myBitMap.SetPixel(P.X, P.Y, Color.Red);
            //     pixelPoint.Add(new Point(e.X, e.Y));
            pictureBox1.Image = myBitMap;
        }

        public void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myBitMap =menu.openFile();
            pictureBox1.Image = myBitMap;
            frameNo = 0;
        }

        public void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f2 = new Bitmaptest();
            f2.ShowDialog();
        }

        public void jointTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on joint tracking.");
        }

       
    }
}
