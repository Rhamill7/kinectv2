using System;
using System.Collections.Generic;
using System.Collections;
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


namespace BitmapTest
{
    public partial class Bitmaptest : Form
    {
        
        List<Point> pixelPoint = new List<Point>();
        List<bool> frameJointList = new List<bool>();
        private int frameNo=0;
        private bool frameSelected = false;
        string joint;
        Tracking track = new Tracking();
        MenuItems menu = new MenuItems();

        public Bitmaptest()
        {
            InitializeComponent();
        }

        private void Bitmaptest_Load(object sender, EventArgs e)
        {

         //   if (checkBox1.Checked)
         //   {
                // add event handle to enable click
                this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
        //    }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point P = new Point(e.X, e.Y);

            if (frameSelected == true)
            {
                if (checkBox1.Checked && (groupBox1.Controls.OfType<RadioButton>().Any(x => x.Checked)))
                {   
                    //if(frameJointList[frameNo]==false) { 
                    try
                    {
                        Bitmap myBitmap = getBitmap(menu.filePathLocation() + frameNo + ".png");
                        myBitmap.SetPixel(P.X, P.Y, Color.Red);
                        pictureBox1.Image = myBitmap;
                        MessageBox.Show(P.ToString());
                        track.setJointLocations(frameNo,P,joint);

                           // frameJointList.Insert(frameNo, true);
                    }
                      catch (Exception r)
                    {
                        MessageBox.Show(r + "Please click somewhere on the picture.");
                    }

                    //}
                      // else { MessageBox.Show("You have already picked a joint for this frame");
                    //}
                }
                else
                {
                    MessageBox.Show("Please enable Joint selection and pick the joint of interest.");
                }
            }

            else
            {
                MessageBox.Show("Please select the first frame.");
            }
        }
      

        public Bitmap getBitmap(string input)
        {
            Bitmap myBitmap;
            //Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
            
                myBitmap = new Bitmap(input);
            
            return myBitmap;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            Bitmap myBitMap = menu.openFile();
        //  filePath = Interaction.InputBox("Please enter a valid file path to the folder of depth images",
         //    "File Path", "C:\\Users\\Robbie\\Pictures\\");
        // string input = filePath + "0"+".png";
        //    Bitmap myBitMap = getBitmap(input);
            pictureBox1.Image = myBitMap;
            frameNo = 0;
            frameSelected = true;
            frameJointList.Add(false);
            // MessageBox.Show(input);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Previous frame
            // MessageBox.Show("This is button 2");
            if (frameSelected == true)
            {

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
            else {
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
                        frameJointList.Add(false);
                    }
                    catch { MessageBox.Show("You have reached the last frame");
                        frameNo--;
                    }
                    }
                catch (Exception a)
                {
                    MessageBox.Show("Error " + a);
                }
            }
            else{
                MessageBox.Show("Please select the first frame.");
            }
     

        }

        private void jointTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please make sure you have selected joints on all desired frames.");
            this.Hide();
             Form f2 = new Form2(track);
            f2.Show();
        }

        private void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on joint selection");
        }

      
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
             joint = "hip";
      //      MessageBox.Show(joint);

        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            joint = "knee";
        //    MessageBox.Show(joint);

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            joint = "ankle";
          //  MessageBox.Show(joint);

        }

      
    }
}