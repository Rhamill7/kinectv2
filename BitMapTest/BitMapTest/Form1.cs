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
using Microsoft.Kinect;


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
        string imageName;
        //string input;
        Tracking track = new Tracking();
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

        /***********drawing on screen*******************************************/
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
                string fileName = ofd.SafeFileName;
                string[] words = fileName.Split('.');
                imageName = words[0];
                myBitmap = getBitmap(input);
                pictureBox1.Image = myBitmap;
                polygon.Clear();

            

            }
        }


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
            Point start = polygon.First();
            polygon.Add(start);
          //  int length;
            Point[] conversion = polygon.ToArray();
            string newFileName = "trainingData" + imageName + ".csv";
            string filePath = @"H:\CodeOutputs\" + newFileName;


            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
         
            string delimter = ",";
            List<UInt16[]> output = new List<UInt16[]>();

            for (int x = 0; x < myBitmap.Width; x++)
            {
                for (int y = 0; y < myBitmap.Height; y++)
                {
                    //test for third label value
                    int label = 0;
                //    var first = x.ToString();
                    var second = y.ToString();
                    Point current = new Point(x, y);
                    bool check = track.IsPointInPolygon(conversion, current);
                    if (check == true)
                    {
                        //label = 1; }
                        UInt16[] futureValues = track.setJointLocations(current, myBitmap);
                        output.Add(futureValues);
                    }
                }

            }
            int length = output.Count;

            using (System.IO.TextWriter writer = File.CreateText(filePath))
            {
                for (int index = 0; index < length; index++)
                {
                    for (int j = 0; j<output[index].Count(); j++)
                    writer.WriteLine(string.Join(delimter, output[index].ToString()), delimter, 1.ToString());
                }
            }

            //   File.WriteAllText(filePath, csv.ToString());
            MessageBox.Show("Saved Data to CSV! Now creating tree");

          //  createTree(filePath);
        }


        private void createTree(string filePath)
        {
        //    //DataTable dt3 = new DataTable();
        //    // Finishes and save any pending changes to the given data
        //    //  filePath = "@" + filePath;
        //    DataTable res = ConvertCSVtoDataTable((filePath));
        //    //@"H:\trainingData.csv"));

        //    //   object dgvLearningSource = null;
        //    // Creates a matrix from the entire source data table
        //    double[,] table = res.ToMatrix(out columnNames);


        //    // Get only the input vector values (first two columns)
        //    double[][] inputs = table.GetColumns(0, 1).ToArray();

        //    // Get only the output labels (last column)

        //    outputs = table.GetColumn(2).ToInt32();

        //    //           table.OrderBy(r => Guid.NewGuid()).Take(5);

        //    // Specify the input variables
        //    DecisionVariable[] variables =
        //    {
        //        new DecisionVariable("x", DecisionVariableKind.Continuous),
        //        new DecisionVariable("y", DecisionVariableKind.Continuous),                                                                                                                                                                                                                                                                                                                                                                                                                           
        //    };
         
        //    // Create the discrete Decision tree
        //    DecisionTree tree = new DecisionTree(variables, 2);

        //    // Create the C4.5 learning algorithm
        //    C45Learning c45 = new C45Learning(tree);

        //    // Learn the decision tree using C4.5
        //    double error = c45.Run(inputs, outputs);


        //    bootStrappedDecisionTrees.Add(tree);
        //    // MessageBox.Show(error.ToString());
        //    //  }
        //    MessageBox.Show("Learning finished!");
        }




        /*******************************running on new data*****************************************/
        private void button4_Click(object sender, EventArgs e)
        {

          //  /**************create new csv of input pixels******/
          //  List<Point> colourList = new List<Point>();
          //  string newFileName = "TestingData" + imageNumber + ".csv";
          //  //string fileName = "TrainingDataSimple.csv";
          //  string filePath = @"H:\CodeOutputs\" + newFileName;
          //  string first;
          //  string second;

          //  if (!File.Exists(filePath))
          //  {
          //      File.Create(filePath).Close();
          //  }

          //  var csv = new StringBuilder();
          //  var newLine = string.Format("{0},{1}", "X", "Y");
          //  csv.AppendLine(newLine);
          //  for (int x = 0; x < myBitmap.Width; x++)
          //  {
          //      for (int y = 0; y < myBitmap.Height; y++)
          //      {
          //          first = x.ToString();
          //          second = y.ToString();
          //          var newLine2 = string.Format("{0},{1}", first, second);
          //          csv.AppendLine(newLine2);
          //      }
          //  }
          //  File.WriteAllText(filePath, csv.ToString());
          //  MessageBox.Show("Saved Data to CSV! Now Executing");


          //  /***************************************/


          //  DataTable dt2 = ConvertCSVtoDataTable(filePath);
          //  // Creates a matrix from the entire source data table
          //  double[,] table2 = dt2.ToMatrix(out columnNames);

          //  // Get only the input vector values (first two columns)
          //  double[][] inputs = table2.GetColumns(0, 1).ToArray();

          //  // Get the expected output labels (last column)
          //  //  int[] expected = table2.GetColumn(2).ToInt32();


          //  // Compute the actual tree outputs
          //  int[] actual = new int[inputs.Length];

          //  //for every input
          //  for (int i = 0; i < inputs.Length; i++)
          //  {

          //      int[] result = new int[bootStrappedDecisionTrees.Count];
          //      //for every tree
          //      for (int j = 0; j < bootStrappedDecisionTrees.Count; j++)
          //      {
          //          //add each tree result to array
          //          result[j] = bootStrappedDecisionTrees[j].Compute(inputs[i]);
          //      }
          //      //get most common occuring label for each pixel
          //      actual[i] = CommonOccurrence(result);
          //  }

          //  MessageBox.Show("MADE IT!");

          //  /**************Print Results*****************************/
          //  newFileName = "Results" + imageNumber + ".csv";
          //  filePath = @"H:\CodeOutputs\" + newFileName;
          //  if (!File.Exists(filePath))
          //  {
          //      File.Create(filePath).Close();
          //  }
          //  //  var X = "X"; var Y = "Y"; var g = "G";
          //  csv = new StringBuilder();
          //  newLine = string.Format("{0},{1},{2}", "X", "Y", "G");
          //  csv.AppendLine(newLine);

          //  var xValue = dt2.AsEnumerable()
          //  .Select(a => a.Field<string>("X"))
          //  .ToList();

          //  var yValue = dt2.AsEnumerable()
          //   .Select(a => a.Field<string>("Y"))
          //   .ToList();

          //  Rectangle cloneRect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
          //  System.Drawing.Imaging.PixelFormat format = myBitmap.PixelFormat;
          //  Bitmap cloneBitmap2 = myBitmap.Clone(cloneRect, format);
          //  pictureBox1.Image = cloneBitmap2;

          //  for (int k = 0; k < inputs.Length; k++)
          //  {

          //      var newLine2 = string.Format("{0},{1},{2}", xValue[k], yValue[k], actual[k]);
          //      csv.AppendLine(newLine2);

          //      Point P = new Point();
          //      P.X = Convert.ToInt32(xValue[k]);
          //      P.Y = Convert.ToInt32(yValue[k]);

          //      if (actual[k] == 1)
          //      {

          //          using (Graphics L = Graphics.FromImage(cloneBitmap2))
          //          {
          //              cloneBitmap2.SetPixel(P.X, P.Y, Color.Red);
          //              this.pictureBox1.Invalidate();
          //          }
          //      }
          //  }
          ////  pictureBox1 = cloneBitmap2;
          //  pictureBox1.Invalidate();
          //  File.WriteAllText(filePath, csv.ToString());



            MessageBox.Show("Finished");
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


