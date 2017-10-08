using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace ReaderSystem
{
    public partial class MainForm : Form
    {

        public ManualResetEvent machineTimeout = new ManualResetEvent(false);

        /// <summary>
        /// 测试数据
        /// </summary>
        public DataInfo data { get; set; }

        public bool StartRecData = false;

        public MainForm()
        {
            InitializeComponent();
            Init();
            Log.WriteLog("程序已启动");
        }

        private void Init()
        {
            data = new DataInfo(DateTime.Now.ToString("HH-mm-ss"), "", 0.0);
            txtInputBarcode.Focus();
            SerialPort sp = new SerialPort();
            txtInputBarcode.KeyUp += new KeyEventHandler(textbox1_keyup);
        }

        /// <summary>
        /// 延时后执行读取
        /// </summary>
        public void ReadVaccumValue()
        {
            DataInfo data = new DataInfo("18:09", "234", 1.3);
            UpdateData(data);
            MessageBox.Show("读取OK");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void PressEnter(object sender, EventArgs e)
        {
            
        }

        private void textbox1_keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
               
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SerialPort(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] buf = new byte[serialPort1.ReadBufferSize + 1];
                int COUNT = serialPort1.Read(buf, 0, serialPort1.ReadBufferSize);
                string sStatus = System.Text.Encoding.ASCII.GetString(buf, 0, COUNT);
                Log.WriteLog("收到串口数据：" + sStatus);
                string[] testresult = new string[13];
                testresult = CaptureValue(buf);
                String time = DateTime.Now.ToLocalTime().ToString();
                if (StartRecData)
                {
                    data.Value = Convert.ToDouble(testresult[12]);
                    UpdateData(data);
                    SaveData(data);
                    data = new DataInfo("", "", 0.0);
                    StartRecData = false;
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        FinishUpdate();
                    }));
                    
                }   
            }
            catch(Exception ee)
            {
                Log.WriteLog("串口数据错误：" + ee.Message);
            }
  
        }
        public string[] CaptureValue(byte[] bytes)
        {

            double valuechange = new double();
            string[] result = new string[13];
           
            int i = 0;
            for (i = 0; i < 6; i++)
            {
                result[i] = Convert.ToString(bytes[i], 10);

            }                                                          //年月日十分秒

            byte[] testtimebuf = new byte[4];
            for (i = 0; i < 4; i++)
            {
                testtimebuf[i] = bytes[11 + i];
            }

            valuechange = BitConverter.ToSingle(testtimebuf, 0);
            result[11] = valuechange.ToString();                        //测试时间

            byte[] Testvaluebyte = new byte[4];
            for (i = 0; i < 4; i++)
            {
                Testvaluebyte[i] = bytes[15 + i];
            }

            valuechange = BitConverter.ToSingle(Testvaluebyte, 0);
            result[12] = valuechange.ToString();                        //测试值
            return result;
        }

        private void textbox1_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(txtInputBarcode.Text != null )
                {
                    data.ReadDataTime = DateTime.Now.ToString("HH-mm-ss");
                    data.Barcode = txtInputBarcode.Text;
                    txtShowBarcode.Text = data.Barcode;
                    txtInputBarcode.Text = "";
                    WaitFrmShow((int)nudWaitTime.Value);
                    InitDisplay();
                    StartRecData = true;


                }
                else
                {
                    MessageBox.Show("输入条码为空，请重新录入条码！");
                }

            }
        }

        public bool WaitTime(int Seconds)
        {
            if (machineTimeout.WaitOne(new TimeSpan(0, 0, 0, Seconds), false))
            {
                //machineTimeout.Set();
                return true;
            }
            else
            {
                //执行读取/获取数据
                UpdateData(new DataInfo("19-70", "1234782947", 2.55));
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
           serialPort1.Open();           
        }

        private void InitDisplay()
        {
            btnTest.Text = "读取中....";
            btnTest.BackColor = Color.Yellow;
            txtInputBarcode.Enabled = false;
        }

        private void FinishUpdate()
        {
            txtInputBarcode.Enabled = true;
            btnTest.Text = "读取完成";
            btnTest.BackColor = Color.Lime;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            //wait.
            //WaitTime((int)nudWaitTime.Value);
            //Log.WriteExcel("Value","2027","23434","345435");
             //WaitFrmShow((int)nudWaitTime.Value);

        }

        public void WaitFrmShow(int Seconds)
        {
            AutoClosingMessageBox.Show("等待时间:" + Seconds.ToString() + "秒","延时等待..", Seconds * 1000);
        }


        /// <summary>
        /// 更新界面数据显示
        /// </summary>
        /// <param name="data"></param>
        public void UpdateData(DataInfo data)
        {
            //machineTimeout.Reset();
            this.BeginInvoke(new MethodInvoker(delegate
            {
                dgvDataInfo.Rows.Add(dgvDataInfo.Rows.Count,data.ReadDataTime, data.Barcode, data.Value.ToString("f2"));
                //sDisplay.SetRecord(stepInfo.RecordResult);
            }));

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        public void SaveData(DataInfo data)
        {
            Log.WriteLog("保存数据至CSV");
            Log.WriteExcel("Data", data.ReadDataTime, data.Barcode, data.Value.ToString("f2"));
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.WriteLog("程序已退出");
            if (serialPort1.IsOpen)
            {
                Log.WriteLog("程序串口关闭");
                serialPort1.Close();
            }
        }
    }
}
