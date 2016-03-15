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
//using AForge;
//using Components;
using System.IO;

//using ZedGraph;


namespace BitmapTest
{
    public partial class Bitmaptest : Form
    {


        DecisionTree tree;
        string[] columnNames;
        int[] outputs;
        // DataTable table;
        // double[] xVals;
        // double[] yVals;
        // double[] xValues;
        //double[] yValues;
        // double[] lbls2;
        //int[]jointLabels;
        // double[] inputs;
        List<Point> pixelPoint = new List<Point>();
        List<bool> frameJointList = new List<bool>();
        private int frameNo = 0;
        private bool frameSelected = false;
        string joint;
        Tracking track = new Tracking();
        MenuItems menu = new MenuItems();
        private Point? _Previous = null;
        private Pen _Pen = new Pen(Color.Black);
        private string filePath;
        Bitmap myBitmap;

        public Bitmaptest()
        {
            InitializeComponent();

            // dgvLearningSource.AutoGenerateColumns = true;
            //   dgvPerformance.AutoGenerateColumns = false;

            //    openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "Resources");
        }


        /***********drawing on screen*******************************************/
        private void Bitmaptest_Load(object sender, EventArgs e)
        {
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _Previous = new Point(e.X, e.Y);
            pictureBox1_MouseMove(sender, e);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {


            if (_Previous != null)
            {
                if (pictureBox1.Image == null)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBox1.Image = bmp;
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawLine(_Pen, _Previous.Value.X, _Previous.Value.Y, e.X, e.Y);
                }
                pictureBox1.Invalidate();
                _Previous = new Point(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _Previous = null;
        }
        /******************************************************************************/


        /*

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point P = new Point(e.X, e.Y);

            if (frameSelected == true)
            {
                if (checkBox1.Checked) // && (groupBox1.Controls.OfType<RadioButton>().Any(x => x.Checked)))
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
                    MessageBox.Show("Please enable Joint selection.");
                }
            }

            else
            {
                MessageBox.Show("Please select the first frame.");
            }
        }
      */

        public Bitmap getBitmap(string input)
        {

            //Bitmap myBitmap = new Bitmap("H:\\MarkerlessSamples\\20160125_132341_00\\depth\\0.png");

            myBitmap = new Bitmap(input);

            return myBitmap;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if( ofd.ShowDialog() == DialogResult.OK) {
                //  Bitmap myBitMap = menu.openFile();
                // filePath = Interaction.InputBox("Please enter a valid file path to the folder of depth images",
                //  "File Path", "C:\\Users\\Robbie\\Pictures\\");
                //string input = filePath + "0" + ".png";
                string input = ofd.FileName;
                myBitmap = getBitmap(input);
            pictureBox1.Image = myBitmap;
            frameNo = 0;
            frameSelected = true;
            frameJointList.Add(false);
                //  Bitmap myBitmap = getBitmap(menu.filePathLocation() + frameNo + ".png");
                // MessageBox.Show(input);
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
            MessageBox.Show("Please make sure you have selected joints on all desired frames.");
            this.Hide();
            Form f2 = new Form2(track);
            f2.Show();
        }

        private void jointSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are already on joint selection");
        }




        private void button3_Click(object sender, EventArgs e)
        {
            /* if (dgvLearningSource.DataSource == null)
             {
                 MessageBox.Show("Please load some data first.");
                 return;
             }

             // Finishes and save any pending changes to the given data
             dgvLearningSource.EndEdit();
             */
            DataTable res = ConvertCSVtoDataTable((@"H:\trainingData.csv"));

            //   object dgvLearningSource = null;
            // Creates a matrix from the entire source data table
            double[,] table = res.ToMatrix(out columnNames);
            //convert array of strings to array of ints

            // Get only the input vector values (first two columns)
            double[][] inputs = table.GetColumns(0, 1).ToArray();

            // Get only the output labels (last column)

            outputs = table.GetColumn(2).ToInt32();

            // Get only the input vector values (first two columns)
            //    double[][] inputs = new double[2][];
            //  inputs[0] = xValues;
            // inputs[1] = yValues;

            //inputs.InsertColumn(xValues);
            //inputs.InsertColumn(yValues);
            // Get only the output labels (last column)
            //      int[] outputs = jointLabels;


            // Specify the input variables
            DecisionVariable[] variables =
            {
                new DecisionVariable("x", DecisionVariableKind.Continuous),
                new DecisionVariable("y", DecisionVariableKind.Continuous),
            };

            // Create the discrete Decision tree
            tree = new DecisionTree(variables, 2);

            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            double error = c45.Run(inputs, outputs);



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



        //***********************************loading data from TrainingData***************************************// 
        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //var reader = new StreamReader(File.OpenRead(@"H:\TrainingData.csv"));
            // DataTable res = ConvertCSVtoDataTable((@"H:\TrainingData.csv"));
            //List<string> Xvalues = new List<string>();
            //List<string> Yvalues = new List<string>();
            //List<string> lbls = new List<string>();

            //// char[] delimiters = new char[] {',',';' };
            //while (!reader.EndOfStream)
            //{
            //    char[] delimiters = new char[] { ',', '\r', '\n', ' ' };
            //    string line = reader.ReadLine();
            //    string[] values = line.Split(delimiters);                 //Split(',');

            //    Xvalues.Add(values[0]);
            //    Yvalues.Add(values[1]);
            //    lbls.Add(values[2]);
            //    //for (int i = 0; i < values.Length; i++)
            //    //{
            //    //    MessageBox.Show(values[i]);
            //    //}


            //}


            //double[] xValues = new double[Xvalues.Count];
            //double[] yValues = new double[Yvalues.Count];
            //double[] jointlbls = new double[lbls.Count];

            ///************Convert to doubles*******/
            //double result;

            //foreach (string value in Xvalues)
            //{
            //    try
            //    {
            //        result = Convert.ToDouble(value);
            //        xValues.Add(result);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("Unable to convert '{0}' to a Double.", value);
            //    }
            //    catch (OverflowException)
            //    {
            //        Console.WriteLine("'{0}' is outside the range of a Double.", value);
            //    }
            //}

            //foreach (string value in Yvalues)
            //{
            //    try
            //    {
            //        result = Convert.ToDouble(value);
            //        yValues.Add(result);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("Unable to convert '{0}' to a Double.", value);
            //    }
            //    catch (OverflowException)
            //    {
            //        Console.WriteLine("'{0}' is outside the range of a Double.", value);
            //    }
            //}

            //foreach (string value in lbls)
            //{
            //    try
            //    {
            //        result = Convert.ToDouble(value);
            //        jointlbls.Add(result);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("Unable to convert '{0}' to a Double.", value);
            //    }
            //    catch (OverflowException)
            //    {
            //        Console.WriteLine("'{0}' is outside the range of a Double.", value);
            //    }
            //}

            //jointLabels = jointlbls.Select(d => (int)d).ToArray();
            ///************************************************************************/
            //MessageBox.Show("When ready, click 'Create Tree' to start the tree inducing algorithm!");
        }



        /**************************Save data from input*************/
        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
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
                    var card = rnd.Next(2);
                    var first = x.ToString();
                    var second = y.ToString();
                    //  string newLine = (first + delimiter +second + delimiter + lbls);
                    var newLine2 = string.Format("{0},{1},{2}", first, second, card.ToString());
                    csv.AppendLine(newLine2);

                    //joint.ToString()} 
                    /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                }

            }

            File.WriteAllText(filePath, csv.ToString());
        }


        /*******************************running on new data*****************************************/
        private void button4_Click(object sender, EventArgs e)
        {
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
                int g = 0;
                for (int x = 0; x < myBitmap.Width; x++)
                {
                    for (int y = 0; y < myBitmap.Height; y++)
                    {
                        //test for third label value
                        //  var card = rnd.Next(2);
                        var first = x.ToString();
                        var second = y.ToString();
                        var third = outputs[g].ToString();
                        //  string newLine = (first + delimiter +second + delimiter + lbls);
                        var newLine2 = string.Format("{0},{1},{2}", first, second, third);
                        csv.AppendLine(newLine2);
                        g++;

                        //joint.ToString()} 
                        /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                    }

                }

                File.WriteAllText(filePath, csv.ToString());
            }
            //  private void btnTestingRun_Click(object sender, EventArgs e)
            //{
            if (tree == null || filePath == null)
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
            for (int i = 0; i < inputs.Length; i++)
            {
                actual[i] = tree.Compute(inputs[i]);
                MessageBox.Show(actual[i].ToString());
            }

            // Use confusion matrix to compute some statistics.
         //   ConfusionMatrix confusionMatrix = new ConfusionMatrix(actual, expected, 1, 0);

            MessageBox.Show("MADE IT!");
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
                //    string delimiter = ",";
                for (int x = 0; x < myBitmap.Width; x++)
                {
                    for (int y = 0; y < myBitmap.Height; y++)
                    {
                        //test for third label value
                        //  var card = rnd.Next(2);
                        var first = x.ToString();
                        var second = y.ToString();

                        //  string newLine = (first + delimiter +second + delimiter + lbls);
                        var newLine2 = string.Format("{0},{1},{2}", first, second, actual);
                        csv.AppendLine(newLine2);

                        //joint.ToString()} 
                        /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                    }

                }

                File.WriteAllText(filePath, csv.ToString());

                //     dgvPerformance.DataSource = new[] { confusionMatrix };

                // Create performance scatter plot
                //   CreateResultScatterplot(zedGraphControl1, inputs, expected.ToDouble(), actual.ToDouble());
            }

            //  double[][] inputs = new double[2][];
            ////  double[] xVals;
            // // double[] yVals;

            //  if (tree == null)
            //  {
            //      MessageBox.Show("Please create a machine first and load in a new image.");
            //      return;
            //  }
            //  for (int x = 0; x < myBitmap.Width; x++)
            //  {
            //      for (int y = 0; y < myBitmap.Height; y++)
            //      {


            //          var first = x.ToString();
            //          var second = y.ToString();
            //          xVals.Add(Convert.ToDouble(x));
            //          yVals.Add(Convert.ToDouble(y));
            //      }

            //  }

            //  // Creates a matrix from the entire source data table
            ////  double[,] table = (dgvLearningSource.DataSource as DataTable).ToMatrix(out columnNames);

            //  // Get only the input vector values (first two columns)
            //  inputs.InsertColumn(xVals);
            //  inputs.InsertColumn(yVals);


            //  // Get the expected output labels (last column)
            //  int[] expected = jointLabels;


            //  // Compute the actual tree outputs
            //  int[] actual = new int[inputs.Length];
            //  for (int i = 0; i < inputs.Length; i++)
            //      actual[i] = tree.Compute(inputs[i]);

            //  string fileName = "OutputData.csv";
            //  string filePath = "H:\\" + fileName;
            //  if (!File.Exists(filePath))
            //  {
            //      File.Create(filePath).Close();
            //  }
            //  var csv = new StringBuilder();
            ////  var newLine = string.Format("X", "Y", "Actual label");
            ////  string delimiter = ",";
            //  for (int x = 0; x < myBitmap.Width; x++)
            //  {
            //      for (int y = 0; y < myBitmap.Height; y++)
            //      {


            //          var first = x.ToString();
            //          var second = y.ToString();
            //          //  string newLine = (first + delimiter + second + delimiter + lbls);
            //          var newLine = string.Format("{0},{1}", first, second);
            //          csv.AppendLine(newLine);

            //          //joint.ToString()} 
            //          /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
            //      }

            //  }

            //  File.WriteAllText(filePath, csv.ToString());
            // Use confusion matrix to compute some statistics.
            //            ConfusionMatrix confusionMatrix = new ConfusionMatrix(actual, expected, 1, 0);
            // dgvPerformance.DataSource = new[] { confusionMatrix };

            // Create performance scatter plot
            //  CreateResultScatterplot(zedGraphControl1, inputs, expected.ToDouble(), actual.ToDouble());
        }
    }
}

    
