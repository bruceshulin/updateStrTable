namespace strtableUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnOpenStrTab = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartCompare = new System.Windows.Forms.Button();
            this.btnOpenStr2 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnImportIDtoTxt = new System.Windows.Forms.Button();
            this.btnValuetoTxt = new System.Windows.Forms.Button();
            this.cbTable2ValueEmpty = new System.Windows.Forms.CheckBox();
            this.cbTable1ExiseValue = new System.Windows.Forms.CheckBox();
            this.btnTab2ImpTab1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenStrTab
            // 
            this.btnOpenStrTab.Location = new System.Drawing.Point(12, 13);
            this.btnOpenStrTab.Name = "btnOpenStrTab";
            this.btnOpenStrTab.Size = new System.Drawing.Size(102, 35);
            this.btnOpenStrTab.TabIndex = 0;
            this.btnOpenStrTab.Text = "打开字符串表1";
            this.btnOpenStrTab.UseVisualStyleBackColor = true;
            this.btnOpenStrTab.Click += new System.EventHandler(this.btnOpenStrTab_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(122, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(629, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "有差异的字符串";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnStartCompare
            // 
            this.btnStartCompare.Location = new System.Drawing.Point(649, 103);
            this.btnStartCompare.Name = "btnStartCompare";
            this.btnStartCompare.Size = new System.Drawing.Size(102, 35);
            this.btnStartCompare.TabIndex = 0;
            this.btnStartCompare.Text = "开始对比";
            this.btnStartCompare.UseVisualStyleBackColor = true;
            this.btnStartCompare.Click += new System.EventHandler(this.btnStartCompare_Click);
            // 
            // btnOpenStr2
            // 
            this.btnOpenStr2.Location = new System.Drawing.Point(12, 65);
            this.btnOpenStr2.Name = "btnOpenStr2";
            this.btnOpenStr2.Size = new System.Drawing.Size(102, 35);
            this.btnOpenStr2.TabIndex = 0;
            this.btnOpenStr2.Text = "对比字符串表2";
            this.btnOpenStr2.UseVisualStyleBackColor = true;
            this.btnOpenStr2.Click += new System.EventHandler(this.btnOpenStr2_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(122, 73);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(629, 21);
            this.textBox3.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 173);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(738, 329);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(730, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "表1没有表2有的ID";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 297);
            this.panel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(730, 303);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "表1与表2相同ID不同值";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(724, 297);
            this.panel2.TabIndex = 0;
            // 
            // btnImportIDtoTxt
            // 
            this.btnImportIDtoTxt.Location = new System.Drawing.Point(399, 104);
            this.btnImportIDtoTxt.Name = "btnImportIDtoTxt";
            this.btnImportIDtoTxt.Size = new System.Drawing.Size(107, 33);
            this.btnImportIDtoTxt.TabIndex = 6;
            this.btnImportIDtoTxt.Text = "导出ID到文本";
            this.btnImportIDtoTxt.UseVisualStyleBackColor = true;
            this.btnImportIDtoTxt.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnValuetoTxt
            // 
            this.btnValuetoTxt.Location = new System.Drawing.Point(512, 104);
            this.btnValuetoTxt.Name = "btnValuetoTxt";
            this.btnValuetoTxt.Size = new System.Drawing.Size(131, 33);
            this.btnValuetoTxt.TabIndex = 6;
            this.btnValuetoTxt.Text = "导出不同值到文本";
            this.btnValuetoTxt.UseVisualStyleBackColor = true;
            this.btnValuetoTxt.Click += new System.EventHandler(this.btnValuetoTxt_Click);
            // 
            // cbTable2ValueEmpty
            // 
            this.cbTable2ValueEmpty.AutoSize = true;
            this.cbTable2ValueEmpty.Location = new System.Drawing.Point(144, 135);
            this.cbTable2ValueEmpty.Name = "cbTable2ValueEmpty";
            this.cbTable2ValueEmpty.Size = new System.Drawing.Size(126, 16);
            this.cbTable2ValueEmpty.TabIndex = 7;
            this.cbTable2ValueEmpty.Text = "忽略表2值为空的项";
            this.cbTable2ValueEmpty.UseVisualStyleBackColor = true;
            // 
            // cbTable1ExiseValue
            // 
            this.cbTable1ExiseValue.AutoSize = true;
            this.cbTable1ExiseValue.Location = new System.Drawing.Point(144, 113);
            this.cbTable1ExiseValue.Name = "cbTable1ExiseValue";
            this.cbTable1ExiseValue.Size = new System.Drawing.Size(114, 16);
            this.cbTable1ExiseValue.TabIndex = 7;
            this.cbTable1ExiseValue.Text = "忽略表1存在的值";
            this.cbTable1ExiseValue.UseVisualStyleBackColor = true;
            // 
            // btnTab2ImpTab1
            // 
            this.btnTab2ImpTab1.Location = new System.Drawing.Point(286, 103);
            this.btnTab2ImpTab1.Name = "btnTab2ImpTab1";
            this.btnTab2ImpTab1.Size = new System.Drawing.Size(107, 33);
            this.btnTab2ImpTab1.TabIndex = 6;
            this.btnTab2ImpTab1.Text = "表2导入到表1";
            this.btnTab2ImpTab1.UseVisualStyleBackColor = true;
            this.btnTab2ImpTab1.Click += new System.EventHandler(this.btnTab2ImpTab1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 509);
            this.Controls.Add(this.cbTable1ExiseValue);
            this.Controls.Add(this.cbTable2ValueEmpty);
            this.Controls.Add(this.btnValuetoTxt);
            this.Controls.Add(this.btnTab2ImpTab1);
            this.Controls.Add(this.btnImportIDtoTxt);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnOpenStr2);
            this.Controls.Add(this.btnStartCompare);
            this.Controls.Add(this.btnOpenStrTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "字符串对比";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenStrTab;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartCompare;
        private System.Windows.Forms.Button btnOpenStr2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnImportIDtoTxt;
        private System.Windows.Forms.Button btnValuetoTxt;
        private System.Windows.Forms.CheckBox cbTable2ValueEmpty;
        private System.Windows.Forms.CheckBox cbTable1ExiseValue;
        private System.Windows.Forms.Button btnTab2ImpTab1;
    }
}

