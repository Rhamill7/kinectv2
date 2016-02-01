using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public static class AngleCalculator
    {
        public static double GetAngleBetweenPoints(double x1, double y1, double x2, double y2)
        {
            double y = x1 * y2 - x2 * y1;
            double x = x1 * x2 + y1 * y2;
            double angle = Math.Atan2(y, x) / Math.PI * 180.0;
            if (angle < 0.0) angle += 360.0;
            return angle;
        }

        public static double GetAngleBetweenPoints(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dot = x1 * x2 + y1 * y2 + z1 * z2;
            double cos_theta = dot / (Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1) * Math.Sqrt(x2 * x2 + y2 * y2 + z2 * z2));

            return Math.Acos(cos_theta) / Math.PI * 180.0;
        }

        public static double Process(CameraSpacePoint md1, CameraSpacePoint md2, CameraSpacePoint md3, bool twoD, bool supplementary = false, bool relative = false)
        {
            double rx1 = md1.X - md2.X;
            double ry1 = md1.Y - md2.Y;
            double rz1 = md1.Z - md2.Z;
            double rx2 = md3.X - md2.X;
            double ry2 = md3.Y - md2.Y;
            double rz2 = md3.Z - md2.Z;
            double angle = double.NaN;
            if (twoD)
                angle = GetAngleBetweenPoints(rx1, ry1, rx2, ry2);
            else
                angle = GetAngleBetweenPoints(rx1, ry1, rz1, rx2, ry2, rz2);
            double ret = supplementary ? 180.0 - angle : angle;
            if (twoD) ret = -ret;
            return ret;
        }
    }
}
