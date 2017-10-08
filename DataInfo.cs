using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderSystem
{

    public class DataInfo
    {
        public string ReadDataTime { get; set; }

        public string Barcode { get; set; }

        public double Value { get; set; }

        public DataInfo(string readDataTime, string barcode, double value)
        {
            this.ReadDataTime = readDataTime;
            this.Barcode = barcode;
            this.Value = value;
        }
    }
}
