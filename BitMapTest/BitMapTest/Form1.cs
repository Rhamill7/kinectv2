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
using Accord.IO;
using Accord;
using Accord.Controls;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Analysis;
using System.IO;


namespace BitmapTest
{
    public partial class Bitmaptest : Form
    {


        // DecisionTree tree;
        string[] columnNames;
        int[] outputs;
        List<Point> pixelPoint = new List<Point>();
        List<bool> frameJointList = new List<bool>();
        //  private int frameNo = 0;
        // private bool frameSelected = false;
        // string joint;
        // Tracking track = new Tracking();
        MenuItems menu = new MenuItems();
        private Pen _Pen = new Pen(Color.Red);
        // private string filePath;
        Bitmap myBitmap;
        List<Point> polygon = new List<Point>();
        List<DecisionTree> bootStrappedDecisionTrees = new List<DecisionTree>();
        static Random rnd = new Random();

        public Bitmaptest()
        {
            InitializeComponent();
        }


        /***********drawing on screen*******************************************/
        private void Bitmaptest_Load(object sender, EventArgs e)
        {
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point polyPoint = new Point(e.X, e.Y);
            polygon.Add(polyPoint);
            pictureBox1_Paint();
        }
        private void pictureBox1_Paint()
        {

            Rectangle cloneRect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
            System.Drawing.Imaging.PixelFormat format = myBitmap.PixelFormat;
            Bitmap cloneBitmap = myBitmap.Clone(cloneRect, format);
            pictureBox1.Image = cloneBitmap;
            using (Graphics g = Graphics.FromImage(cloneBitmap))
            {
                if (polygon.Count > 1)
                {
                    for (int i = 0; i < polygon.Count - 1; i++)
                    {
                        g.DrawLine(_Pen, polygon[i].X, polygon[i].Y, polygon[i + 1].X, polygon[i + 1].Y);
                    }
                }
                else
                {
                    g.DrawLine(_Pen, polygon[0].X, polygon[0].Y, polygon[0].X, polygon[0].Y);
                }

            }
            pictureBox1.Invalidate();

        }



        /************************IsPoint in polygon algorithm*************/
        private bool IsPointInPolygon(Point[] polygon, Point point)
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


        public Bitmap getBitmap(string input)
        {

            //Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");

            myBitmap = new Bitmap(input);

            return myBitmap;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string input = ofd.FileName;
                myBitmap = getBitmap(input);
                pictureBox1.Image = myBitmap;
                polygon.Clear();
                //  frameNo = 0;
                // frameSelected = true;
                // frameJointList.Add(false);

            }
        }

        /*   private void button2_Click(object sender, EventArgs e)
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
               else
               {
                   MessageBox.Show("Please select the first frame.");
               }

           }
           */
        /*   private void button1_Click(object sender, EventArgs e)
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
                       {
                           Bitmap myBitMap = getBitmap(input);

                           pictureBox1.Image = myBitMap;
                           frameJointList.Add(false);
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
               }
               else
               {
                   MessageBox.Show("Please select the first frame.");
               }


           }*/

        private void jointTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Please make sure you have selected joints on all desired frames.");
            //this.Hide();
            //Form f2 = new Form2(track);
            //f2.Show();
        }

        private void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("You are already on joint selection");
        }




        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt3 = new DataTable();
            // Finishes and save any pending changes to the given data

            DataTable res = ConvertCSVtoDataTable((@"H:\trainingData.csv"));

            //   object dgvLearningSource = null;
            // Creates a matrix from the entire source data table
            double[,] table = res.ToMatrix(out columnNames);


            // Get only the input vector values (first two columns)
            double[][] inputs = table.GetColumns(0, 1).ToArray();

            // Get only the output labels (last column)

            outputs = table.GetColumn(2).ToInt32();

            //           table.OrderBy(r => Guid.NewGuid()).Take(5);

            // Specify the input variables
            DecisionVariable[] variables =
            {
                new DecisionVariable("x", DecisionVariableKind.Continuous),
                new DecisionVariable("y", DecisionVariableKind.Continuous),
            };



            for (int i = 0; i < 10; i++)
            {


                /**********************get random smaller data set from larger data set*******************************/
                List<int> randomNumber = new List<int>();
                for (int r = 0; r < 200; r++)
                {
                    var card = rnd.Next(inputs.Length);
                    randomNumber.Add(card);
                }
                //   MessageBox.Show("Yolo");

                dt3.ImportRow((res.Rows[0]));
                for (int m = 0; m < inputs.Length; m++)
                {
                    //int index = res.Rows.IndexOf(dr);
                    if (randomNumber.Contains(m))
                    {
                        dt3.ImportRow((res.Rows[m]));
                    }
                }


                double[,] table2 = dt3.ToMatrix(out columnNames);
                double[][] inputs2 = table.GetColumns(0, 1).ToArray();
                int[] outputs2 = table.GetColumn(2).ToInt32();

                // Create the discrete Decision tree
                DecisionTree tree = new DecisionTree(variables, 2);

                // Create the C4.5 learning algorithm
                C45Learning c45 = new C45Learning(tree);

                // Learn the decision tree using C4.5
                double error = c45.Run(inputs2, outputs2);


                bootStrappedDecisionTrees.Add(tree);
                // MessageBox.Show(error.ToString());
            }
            MessageBox.Show("Learning finished!");
        }






        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /**************************Save data from input*************/
        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Random rnd = new Random();
            //possible work around on theory of point in polygon

            Point start = polygon.First();
            polygon.Add(start);


            Point[] conversion = polygon.ToArray();
            string fileName = "trainingData.csv";
            //string fileName = "TrainingDataSimple.csv";
            string filePath = "H:\\" + fileName;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            var csv = new StringBuilder();
            var newLine = string.Format("{0},{1},{2}", "X", "Y", "G");
            csv.AppendLine(newLine);
            //    string delimiter = ",";
            for (int x = 0; x < myBitmap.Width; x++)
            {
                for (int y = 0; y < myBitmap.Height; y++)
                {
                    //test for third label value
                    var label = 0;
                    var first = x.ToString();
                    var second = y.ToString();
                    Point current = new Point(x, y);
                    bool check = IsPointInPolygon(conversion, current);
                    if (check == true) { label = 1; }
                    //  string newLine = (first + delimiter +second + delimiter + lbls);
                    var newLine2 = string.Format("{0},{1},{2}", first, second, label.ToString());
                    csv.AppendLine(newLine2);

                    //joint.ToString()} 
                    /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                }

            }

            File.WriteAllText(filePath, csv.ToString());
            MessageBox.Show("Saved Data to CSV!");
        }


     
        
        
        
        /*******************************running on new data*****************************************/
        private void button4_Click(object sender, EventArgs e)
        {
            List<Point> colourList = new List<Point>();


            string fileName = "TestingData.csv";
            //string fileName = "TestingDataSimple.csv";
            string filePath = "H:\\" + fileName;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();

                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2}", "X", "Y", "G");
                csv.AppendLine(newLine);
                //    string delimiter = ",";
                //int g = 0;
                for (int x = 0; x < myBitmap.Width; x++)
                {
                    for (int y = 0; y < myBitmap.Height; y++)
                    {

                        var first = x.ToString();
                        var second = y.ToString();
                        var third = 0;
                        var newLine2 = string.Format("{0},{1},{2}", first, second, third);
                        csv.AppendLine(newLine2);

                    }

                }

                File.WriteAllText(filePath, csv.ToString());
            }
            //  private void btnTestingRun_Click(object sender, EventArgs e)
            //{
            //if (tree == null || filePath == null)
            if (filePath == null)
            {
                MessageBox.Show("Please create a machine first.");
                return;
            }

            DataTable dt2 = ConvertCSVtoDataTable(filePath);
            // Creates a matrix from the entire source data table
            double[,] table2 = dt2.ToMatrix(out columnNames);

            // Get only the input vector values (first two columns)
            double[][] inputs = table2.GetColumns(0, 1).ToArray();

            // Get the expected output labels (last column)
            int[] expected = table2.GetColumn(2).ToInt32();


            // Compute the actual tree outputs
            int[] actual = new int[inputs.Length];

            //for every input
            for (int i = 0; i < inputs.Length; i++)
            {

                int[] result = new int[bootStrappedDecisionTrees.Count];
                //for every tree
                for (int j = 0; j < bootStrappedDecisionTrees.Count; j++)
                {
                    //add each tree result to array
                    result[j] = bootStrappedDecisionTrees[j].Compute(inputs[i]);
                }
                //get most common occuring label for each pixel
                actual[i] = CommonOccurrence(result);
            }

            //  MessageBox.Show("MADE IT!");

            /**************Print Results*****************************/
            fileName = "Results.csv";
            filePath = "H:\\" + fileName;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();

                var X = "X"; var Y = "Y"; var g = "G";
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2}", X, Y, g);
                csv.AppendLine(newLine);

                var first = dt2.AsEnumerable()
             .Select(a => a.Field<string>("X"))
             .ToList();

                var second = dt2.AsEnumerable()
                 .Select(a => a.Field<string>("Y"))
                 .ToList();

                Rectangle cloneRect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
                System.Drawing.Imaging.PixelFormat format = myBitmap.PixelFormat;
                Bitmap cloneBitmap2 = myBitmap.Clone(cloneRect, format);
                pictureBox1.Image = cloneBitmap2;

                for (int k = 0; k < inputs.Length; k++)
                {


                    var newLine2 = string.Format("{0},{1},{2}", first[k], second[k], actual[k]);
                    int bobX;
                    int bobY;
                    bobX = Convert.ToInt32(first[k]);
                    bobY = Convert.ToInt32(second[k]);
                    csv.AppendLine(newLine2);
                    if (actual[k] == 1)
                    {

                        using (Graphics L = Graphics.FromImage(cloneBitmap2))
                        {
                            cloneBitmap2.SetPixel(bobX, bobY, Color.Red);
                            this.pictureBox1.Invalidate();
                        }
                    }

                }

                pictureBox1.Invalidate();
                File.WriteAllText(filePath, csv.ToString());
            }
        }





        /*************Returns most common occurring label***********/
        private int CommonOccurrence(int[] numbers)
        {
            var counts = new Dictionary<int, int>();
            foreach (int number in numbers)
            {
                int count;
                counts.TryGetValue(number, out count);
                count++;
                //Automatically replaces the entry if it exists;
                //no need to use 'Contains'
                counts[number] = count;
            }
            int mostCommonNumber = 0, occurrences = 0;
            foreach (var pair in counts)
            {
                if (pair.Value > occurrences)
                {
                    occurrences = pair.Value;
                    mostCommonNumber = pair.Key;
                }
            }
            return mostCommonNumber;
        }
    }
}


