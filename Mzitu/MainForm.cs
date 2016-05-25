using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Timer = System.Timers.Timer;

namespace Mzitu
{
    public partial class MainForm : Form
    {
        private DetailProcesser dp = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (dp != null)
            {
                dp.Start();
                return;
            }
            var url = "";
            var savePath = txtSavePath.Text.Trim();
            savePath = savePath == "" ? @"D:\Pictures\Meizitu\" : savePath;

            //从第一个开始
            if (rbFromFirst.Checked)
            {
                var main = "http://www.mzitu.com/";
                var html = HttpRequestHelper.Get_Data(main, null, "utf-8", "");
                if (string.IsNullOrEmpty(html))
                    return;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var node = doc.DocumentNode.SelectSingleNode("//ul[@id='pins']//a");
                if (node != null)
                {
                    url = node.GetAttributeValue("href", "");
                }
            }
            //自定义开始
            else
            {
                url = txtCustAddress.Text.Trim();
            }

            if (url != "")
            {
                dp = new DetailProcesser(url, savePath);
                dp.OnUrlChanged += Dp_OnUrlChanged;
                dp.OnSavePathChanged += Dp_OnSavePathChanged;
                dp.Start();
            }

        }

        private void Dp_OnSavePathChanged(object sender, DetailProcesser.StringEventArgs e)
        {
            if (lblSavePath.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    lblSavePath.Text = e.StringArg;
                }));
            }
        }

        private void Dp_OnUrlChanged(object sender, DetailProcesser.StringEventArgs e)
        {
            if (lblWebAddress.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    lblWebAddress.Text = e.StringArg;
                }));
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (dp != null)
                dp.Stop();
        }

        private void rbFromFirst_CheckedChanged(object sender, EventArgs e)
        {
            txtCustAddress.Enabled = rbFromCust.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
