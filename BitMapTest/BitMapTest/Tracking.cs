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


public class Tracking
{

    private List<Point> pixelPoint = new List<Point>();

    public Tracking()
    {
    }

    public void setJointLocations(int frameNo, Point p)
    {
        pixelPoint.Insert(frameNo,p);
        int i = 0;
        MessageBox.Show(pixelPoint[i].ToString());
            
            
    }

    public Point getJointLocations(int frameNo)
    {
        Point p = new Point(0,0);
        for (int i = 0; i < pixelPoint.Count; i++)
        {
            if (i == frameNo)
            {
                MessageBox.Show(pixelPoint[i].ToString());
                p = pixelPoint[i];
                break;
            }
        }
        return p;
    }
}