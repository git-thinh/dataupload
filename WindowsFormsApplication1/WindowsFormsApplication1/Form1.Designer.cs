namespace WindowsFormsApplication1
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.clickText = new System.Windows.Forms.ListBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtloadding = new System.Windows.Forms.TextBox();
            this.txtpage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(705, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "抓取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(158, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(464, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "http://www.soufun.com/house/%B1%B1%BE%A9_________________1_.htm";
            // 
            // clickText
            // 
            this.clickText.FormattingEnabled = true;
            this.clickText.ItemHeight = 12;
            this.clickText.Location = new System.Drawing.Point(12, 56);
            this.clickText.Name = "clickText";
            this.clickText.ScrollAlwaysVisible = true;
            this.clickText.Size = new System.Drawing.Size(926, 136);
            this.clickText.TabIndex = 2;
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(0, 198);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(938, 313);
            this.webBrowser.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtloadding
            // 
            this.txtloadding.Location = new System.Drawing.Point(787, 1);
            this.txtloadding.Name = "txtloadding";
            this.txtloadding.Size = new System.Drawing.Size(151, 21);
            this.txtloadding.TabIndex = 5;
            // 
            // txtpage
            // 
            this.txtpage.Location = new System.Drawing.Point(629, 2);
            this.txtpage.Name = "txtpage";
            this.txtpage.Size = new System.Drawing.Size(79, 21);
            this.txtpage.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 511);
            this.Controls.Add(this.txtpage);
            this.Controls.Add(this.txtloadding);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.clickText);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox clickText;
        public System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtloadding;
        private System.Windows.Forms.TextBox txtpage;
    }
}

