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
using System.Windows.Shapes;

namespace BitmapTest
{
    public partial class Form2 : Form
    {

        private int frameNo;
        private Bitmap myBitMap;
        private bool frameSelected;
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
            if (frameSelected)
            {
                // Pen red = new Pen(Color.Red);
                if (checkBox1.Checked)
                {
                    Graphics gBmp = Graphics.FromImage(myBitMap);
                    Point P = track.getJointLocations(frameNo);
                    if (P.X == 0 & P.Y == 0)
                      {
                        MessageBox.Show("No joint was selected in tracking");
                        return;
                     }
                 
                  
                    pictureBox1.Image = myBitMap;
                 
                    Color red = Color.FromArgb(0x60, 0xff, 0, 0);
                    Pen redPen = new Pen(red);
                    gBmp.DrawEllipse(redPen, P.X, P.Y, 160, 160);    
                    gBmp.Dispose();
                    redPen.Dispose();

                }

                else 
                {
                    // Draw myBitmap to the screen.
                    g.Graphics.DrawImage(myBitMap, 0, 0, myBitMap.Width,
                        myBitMap.Height);
                    //   Graphics gBmp = Graphics.FromImage(myBitMap);
                    pictureBox1.Image = myBitMap;
                    myBitMap.makeTransparent();

                   // PictureBox two = new PictureBox();
                    //two.SetBounds(0, 24, 733, 486);
                    //two.Show();
                    //two.Image = myBitMap;
                    //pictureBox1.Refresh();

                }
            }
            else
            {
                MessageBox.Show("Please select the first frame");
            }

        }

        public Bitmap getBitmap(string input)
        {

            //Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");

            Bitmap myBitmap = new Bitmap(input);

            return myBitmap;
        }

        public void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myBitMap =menu.openFile();
            pictureBox1.Image = myBitMap;
            frameNo = 0;
            frameSelected = true;
        }

        public void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please note, going back to joint selection will clear previously selected joints.");
            this.Hide();
            Form f2 = new Bitmaptest();
            f2.ShowDialog();
        }

        public void jointTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on joint tracking.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (frameSelected == true)
            {
                //Previous frame
                // MessageBox.Show("This is button 2");
                if (frameNo == 0)
                {
                    MessageBox.Show("Error already on the first frame.");
                }
                else
                {
                    frameNo--;
                    string input = menu.filePathLocation() + frameNo + ".png";
                    Bitmap myBitMap = getBitmap(input);
                    pictureBox1.Image = myBitMap;

                }
            }
            else
            {
                MessageBox.Show("Please select the first frame.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Next frame
            //  MessageBox.Show("This is Button 1");
            if (frameSelected == true)
            {

                try
                {
                    // jointPicked = false;
                    frameNo++;
                    string input = menu.filePathLocation() + frameNo + ".png";
                    try
                    { Bitmap myBitMap = getBitmap(input);
                        pictureBox1.Image = myBitMap;
                        //   frameJointList.Add(false);
                    }
                    catch
                    {
                        MessageBox.Show("You have reached the last frame");
                        frameNo--;
                        }

                    }
                catch (Exception a)
                {
                    MessageBox.Show("Error " + a);
                }
            } else
            {
                MessageBox.Show("Please select the first frame.");
            }

        }
    }
}
