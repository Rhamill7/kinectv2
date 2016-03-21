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

    Random ran = new Random();

    public Tracking()
    {
       
    }


    /************************IsPoint in polygon algorithm*************/
    public bool IsPointInPolygon(Point[] polygon, Point point)
    {
        bool isInside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
            (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
            {
                isInside = !isInside;
            }
        }
        return isInside;
    }

    /******************************************************************************/

    public UInt16[] setJointLocations(Point p, Bitmap myBitmap)
    {
        //make new square around point
        Rectangle rec = new Rectangle(((p.X)-2), ((p.Y)-2), 5, 5);
        Point[] recPoints = new Point[3];
        recPoints[0] = new Point (rec.X,rec.Y);
        recPoints[0] = new Point(rec.X+rec.Width, rec.Y);
        recPoints[0] = new Point(rec.X+rec.Width, rec.Y+rec.Height);
        recPoints[0] = new Point(rec.X, rec.Y+rec.Height);
        UInt16[] predictionValues = new UInt16[200];
        //create random offset point pairs 200 times

        for (int i = 0; i<200; i++)
        {
        // make sure random points stay in x and y range of rextangle around point
            int offsetX = ran.Next(rec.X, rec.X + rec.Width);
            int offsetY = ran.Next(rec.Y, rec.Y + rec.Height);
            Point newPoint = new Point(offsetX,offsetY);


            int offsetX2 = ran.Next(rec.X, rec.X + rec.Width);
            int offsetY2 = ran.Next(rec.Y, rec.Y + rec.Height);
            Point newPoint2 = new Point(offsetX, offsetY);
            // Double check to make sure its in polygon
            if (IsPointInPolygon(recPoints, newPoint) && IsPointInPolygon(recPoints, newPoint))
            {

                /***********possible need for conversion to greyscale?******/

                Color pixelColour = myBitmap.GetPixel(offsetX, offsetY);
                Color pixelColour2 = myBitmap.GetPixel(offsetX2, offsetY2);
                int valueOne = pixelColour.ToArgb();
                int valueTwo = pixelColour2.ToArgb();

                int prediction = valueOne - valueTwo;
                //        UInt16 conValueOne = Convert.ToUInt16(valueOne);
                //       UInt16 convalueTwo = Convert.ToUInt16(valueOne);


                UInt16 predictionValue = Convert.ToUInt16(prediction);
                
                predictionValues[i] = predictionValue;
                //UInt16 grayValue1 = Grayscale(pixelColour.ToArgb());
            }
        }
        return predictionValues;
            
    }

   

}