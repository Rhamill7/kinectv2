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
    public List<List<Point>> hipPoints = new List<List<Point>>();
    public List<List<Point>> kneePoints = new List<List<Point>>();
    public List<List<Point>> anklePoints = new List<List<Point>>();
  private  Point avgHipPoint, avgKneePoint, avgAnklePoint = new Point();


    public Tracking()
    {

       // Populate 2d lists
        for (int i = 0; i < 10; i++)
        {

            List<Point> sublist = new List<Point>();
            hipPoints.Add(sublist);
        }


        for (int j = 0; j < 10; j++)
        {

            List<Point> sublist = new List<Point>();
            kneePoints.Add(sublist);
        }


        for (int k= 0; k < 10; k++)
        {

            List<Point> sublist = new List<Point>();
            anklePoints.Add(sublist);
        }
        
    }

    public void setJointLocations(int frameNo, Point p, string jointName)
    {

      //  MessageBox.Show(frameNo + " " + p + " "+ jointName);
        switch (jointName)
        {
            case "hip":
                hipPoints[frameNo].Add(p);
              //  MessageBox.Show(lists.ToString());
                break;
            case "knee":
            //  MessageBox.Show("knee");
                kneePoints[frameNo].Add(p);
                break;
            case "ankle":
               // MessageBox.Show("ankle");
                anklePoints[frameNo].Add(p);
                break;
            default:
               MessageBox.Show("No Joint Selected");
                break;
        }
            
    }

   

    public Point getAvgHipLocations(int frameNo) {

    Point avgHipPoint = new Point
    {
        X = (int)Math.Round(hipPoints[frameNo].Average(a => a.X)),
        Y = (int)Math.Round(hipPoints[frameNo].Average(a => a.Y))
    };

    return avgHipPoint;
    }

    public Point getAvgKneeLocations(int frameNo)
    {
        Point avgKneePoint = new Point
        {
            X = (int)Math.Round(kneePoints[frameNo].Average(a => a.X)),
            Y = (int)Math.Round(kneePoints[frameNo].Average(a => a.Y))
        };
        return avgKneePoint;
    }

    public Point getAvgAnkleLocations(int frameNo)
    {
        Point avgAnklePoint = new Point
        {
            X = (int)Math.Round(anklePoints[frameNo].Average(a => a.X)),
            Y = (int)Math.Round(anklePoints[frameNo].Average(a => a.Y))
        };
        return avgAnklePoint;
    }


}