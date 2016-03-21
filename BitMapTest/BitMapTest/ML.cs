using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMapTest
{
    public class ML
    {
        public static void Train()
        {
            using (System.IO.TextReader fs = System.IO.File.OpenText("dfsafias"))
            {
                using (CsvHelper.CsvReader reader = new CsvHelper.CsvReader(fs))
                {
                   
                    //reader.GetField
                }
            }



            OpenCvSharp.ML.RTrees forest = OpenCvSharp.ML.RTrees.Create();
          //  forest.Predict();
           // forest.
        }
    }
}
