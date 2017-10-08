using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MacMeasure
{
    public partial class frmWaiting : Form
    {
        public frmWaiting()
        {
            InitializeComponent();
        }

        private void frmWaiting_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 1000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] strTime = labelTime.Text.Trim().Split(':');
            int iMinute = Convert.ToInt16(strTime[0]);
            int iSencond = Convert.ToInt16(strTime[1]);
            if (iSencond == 0 && iMinute > 0)
            {
                iMinute--;
                iSencond = 59;
                labelTime.Text = iMinute.ToString("00") + ":" + iSencond.ToString("00");
            }
            else if (iSencond > 0 && iSencond <= 59)
            {
                iSencond--;
                labelTime.Text = iMinute.ToString("00") + ":" + iSencond.ToString("00");
            }
            else if (iSencond == 0 && iMinute == 0)
            {
                this.Close();
            }
        }
    }
}
