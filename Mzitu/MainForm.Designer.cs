namespace Mzitu
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblWebAddress = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.txtCustAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFromCust = new System.Windows.Forms.RadioButton();
            this.rbFromFirst = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSavePath = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(255, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblWebAddress
            // 
            this.lblWebAddress.AutoSize = true;
            this.lblWebAddress.Location = new System.Drawing.Point(96, 165);
            this.lblWebAddress.Name = "lblWebAddress";
            this.lblWebAddress.Size = new System.Drawing.Size(41, 12);
            this.lblWebAddress.TabIndex = 2;
            this.lblWebAddress.Text = "label1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Pause";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(174, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Continue";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "保存路径";
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(68, 253);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(389, 21);
            this.txtSavePath.TabIndex = 5;
            this.txtSavePath.Text = "D:\\Pictures\\Meizitu\\";
            // 
            // txtCustAddress
            // 
            this.txtCustAddress.Enabled = false;
            this.txtCustAddress.Location = new System.Drawing.Point(37, 75);
            this.txtCustAddress.Name = "txtCustAddress";
            this.txtCustAddress.Size = new System.Drawing.Size(262, 21);
            this.txtCustAddress.TabIndex = 5;
            this.txtCustAddress.Text = "http://www.mzitu.com/62984";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFromCust);
            this.groupBox1.Controls.Add(this.rbFromFirst);
            this.groupBox1.Controls.Add(this.txtCustAddress);
            this.groupBox1.Location = new System.Drawing.Point(15, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 106);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "抓取方式";
            // 
            // rbFromCust
            // 
            this.rbFromCust.AutoSize = true;
            this.rbFromCust.Location = new System.Drawing.Point(19, 53);
            this.rbFromCust.Name = "rbFromCust";
            this.rbFromCust.Size = new System.Drawing.Size(107, 16);
            this.rbFromCust.TabIndex = 6;
            this.rbFromCust.Text = "自定义开始地址";
            this.rbFromCust.UseVisualStyleBackColor = true;
            this.rbFromCust.CheckedChanged += new System.EventHandler(this.rbFromFirst_CheckedChanged);
            // 
            // rbFromFirst
            // 
            this.rbFromFirst.AutoSize = true;
            this.rbFromFirst.Checked = true;
            this.rbFromFirst.Location = new System.Drawing.Point(19, 31);
            this.rbFromFirst.Name = "rbFromFirst";
            this.rbFromFirst.Size = new System.Drawing.Size(95, 16);
            this.rbFromFirst.TabIndex = 6;
            this.rbFromFirst.TabStop = true;
            this.rbFromFirst.Text = "从第一张开始";
            this.rbFromFirst.UseVisualStyleBackColor = true;
            this.rbFromFirst.CheckedChanged += new System.EventHandler(this.rbFromFirst_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "当前网页地址";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "当前保存路径";
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Location = new System.Drawing.Point(96, 194);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(29, 12);
            this.lblSavePath.TabIndex = 7;
            this.lblSavePath.Text = "“”";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 286);
            this.Controls.Add(this.lblSavePath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtSavePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.lblWebAddress);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量抓取美女图片";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblWebAddress;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.TextBox txtCustAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFromCust;
        private System.Windows.Forms.RadioButton rbFromFirst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSavePath;
    }
}

