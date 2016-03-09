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


public class MenuItems
    {
    string filePath;

    public Bitmap openFile()
        {
          filePath = Interaction.InputBox("Please enter a valid file path to the folder of depth images",
         "File Path", "C:\\Users\\Robbie\\Pictures\\");
            string input = filePath + "0" + ".png";
            Bitmap myBitMap = new Bitmap(input);
           //this.pictureBox1.Image = myBitMap;
            //frameNo = 0;
            return myBitMap;

        }

        public string filePathLocation()
        {
        return filePath;
        }

        public void nextFrame()
        {

        }

        public void previousFrame()
        {

        }

        public void jointTracking()
        {

        }

        public void jonitSelection()
        {

        }

    }

