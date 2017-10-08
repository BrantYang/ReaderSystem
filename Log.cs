using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ReaderSystem
{
    public class Log
    {
        private static string logPath = string.Empty;
        private static string resultPath = string.Empty;
        private static object _lock = new object();

        public static string LogPath
        {
            get {
                    if (logPath == string.Empty)
                    {
                        //logPath = System.AppDomain.CurrentDomain.BaseDirectory+"\\Log\\";
                        //logPath = Application.StartupPath + "\\";
                        logPath = "D:\\Log\\";
                    }
                    return logPath;
                }
            set { Log.logPath = value; }
        }
        public static string ResultPath
        {
            get
            {
                if (resultPath == string.Empty)
                {
                    resultPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Result\\";
                    //logPath = Application.StartupPath + "\\";
                }
                return resultPath;
            }
            set { Log.resultPath = value; }
        }
        public static void WriteLog(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string fileFullFileName = LogPath + DateTime.Now.ToString("yyyyMMdd") + ".Log";
            try
            {
                lock (_lock)
                {
                    using (sw = System.IO.File.AppendText(fileFullFileName))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + text);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public static void WriteExcel(string name , string[] str1, string[] str2, string[] str3,string flag)
        {
            //if (!Directory.Exists(ResultPath))
            //{
            //    Directory.CreateDirectory(ResultPath);
            //}
            //lock (_lock)
            //{
            //    OperatorExcel operatorexcel = new OperatorExcel(ResultPath + name + ".xls", str1, str2, str3, flag);
            //    operatorexcel.operatorExcel();
            //}
        }

        public static void WriteExcel(string name, string str1, string str2, string str3)
        {

            string stringpath = ResultPath + DateTime.Now.ToString("yyyy-MM-d") + ".csv";

            if (!Directory.Exists(ResultPath))
            {
                Directory.CreateDirectory(ResultPath);
            }
            if (!File.Exists(stringpath))
            {
                StringBuilder tile = new StringBuilder();
                tile.Append("Time" + "," + "Barcode" + "," + "VaccumValue");
                File.AppendAllText(stringpath, tile.ToString());
                File.AppendAllText(stringpath, "\r\n");
            }

            StringBuilder Result = new StringBuilder();
            //Result.Append(DateTime.Now.ToString("MM-dd HH:mm:ss"));
            Result.Append(str1 + "," + str2 + "," + str3);

            File.AppendAllText(stringpath, Result.ToString());
            File.AppendAllText(stringpath, "\r\n");
        }
    }
}
