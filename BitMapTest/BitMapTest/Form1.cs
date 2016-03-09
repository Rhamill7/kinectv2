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


namespace BitmapTest
{
    public partial class Bitmaptest : Form
    {
        //Global Variables for pixel data?
        //Color[,] pixelData = null;
        List<Point> pixelPoint = new List<Point>();
        private int frameNo;
       // private string filePath;
        Tracking track = new Tracking();
        MenuItems menu = new MenuItems();
        public Bitmaptest()
        {
            InitializeComponent();
        }

        private void Bitmaptest_Load(object sender, EventArgs e)
        {
           
            //read image
            // Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");
    //        Bitmap myBitmap = getBitmap();
         

            //loadimage onto Screen
      //      pictureBox1.Image = myBitmap;

            // add event handle to enable click
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);

        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point P = new Point(e.X, e.Y);


            //for (int i = 0; i < pixelData.Length ; i++)
            //{
            // for (int j = 0; j < pixelData.Length; i++)
            //  {
            //     string bob = pixelData.GetValue(i).ToString(); 
            //  pixelData.GetValue
            //bob = pixelData.ToString
            // MessageBox.Show(bob);
          //  string bob = "bob";
           

            try
            {
                Bitmap myBitmap = getBitmap(menu.filePathLocation() + frameNo + ".png");
                myBitmap.SetPixel(e.X, e.Y, Color.Red);
           //     pixelPoint.Add(new Point(e.X, e.Y));
                pictureBox1.Image = myBitmap;
                track.setJointLocations(frameNo,P);
            }
            catch (Exception b)
            {
                MessageBox.Show("Error " + b);                
            }
        }
        //     else {
        //       MessageBox.Show("Please try again.");
        // }
        //}

        public Bitmap getBitmap(string input)
        {

            //Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");

            Bitmap myBitmap = new Bitmap(input);

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
            // MessageBox.Show(input);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Previous frame
           // MessageBox.Show("This is button 2");
            if(frameNo == 0) {
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Next frame
          //  MessageBox.Show("This is Button 1");
            try {
                frameNo++;
                string input = menu.filePathLocation() + frameNo + ".png";
                Bitmap myBitMap = getBitmap(input);
                pictureBox1.Image = myBitMap;
            }
            catch (Exception a)
            {
                MessageBox.Show("Error " + a);
            }
     

        }

        private void jointTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
          //  Bitmaptest.Form2 f2 = new BitMaptest.Form2();
             Form f2 = new Form2(track);
            f2.Show();
        }

        private void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on joint selection");
        }
    }
}