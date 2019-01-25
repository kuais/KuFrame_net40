namespace Test
{
    partial class FrmPrintTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPageSet = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtSzName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtGh = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.chkWorkStatus1 = new System.Windows.Forms.CheckBox();
            this.chkWorkStatus2 = new System.Windows.Forms.CheckBox();
            this.txtJsName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRelation = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtBook = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.optSex2 = new System.Windows.Forms.RadioButton();
            this.optSex1 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFont);
            this.panel1.Controls.Add(this.btnPageSet);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1067, 48);
            this.panel1.TabIndex = 3;
            // 
            // btnPageSet
            // 
            this.btnPageSet.Location = new System.Drawing.Point(37, 9);
            this.btnPageSet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPageSet.Name = "btnPageSet";
            this.btnPageSet.Size = new System.Drawing.Size(100, 31);
            this.btnPageSet.TabIndex = 6;
            this.btnPageSet.Text = "页面设置";
            this.btnPageSet.UseVisualStyleBackColor = true;
            this.btnPageSet.Click += new System.EventHandler(this.btnPageSet_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(415, 9);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 31);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(285, 9);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 31);
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "打印预览";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 83);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "编号";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(109, 79);
            this.txtID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(132, 26);
            this.txtID.TabIndex = 5;
            // 
            // txtSzName
            // 
            this.txtSzName.Location = new System.Drawing.Point(109, 151);
            this.txtSzName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSzName.Name = "txtSzName";
            this.txtSzName.Size = new System.Drawing.Size(132, 26);
            this.txtSzName.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 155);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "逝者姓名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "安装位置";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(109, 115);
            this.txtPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(132, 26);
            this.txtPosition.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 191);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "安放时间";
            // 
            // dtGh
            // 
            this.dtGh.CustomFormat = "yyyy-MM-dd";
            this.dtGh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtGh.Location = new System.Drawing.Point(109, 187);
            this.dtGh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtGh.Name = "dtGh";
            this.dtGh.Size = new System.Drawing.Size(132, 26);
            this.dtGh.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(267, 83);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 16);
            this.label5.TabIndex = 12;
            // 
            // chkWorkStatus1
            // 
            this.chkWorkStatus1.AutoSize = true;
            this.chkWorkStatus1.Location = new System.Drawing.Point(293, 189);
            this.chkWorkStatus1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkWorkStatus1.Name = "chkWorkStatus1";
            this.chkWorkStatus1.Size = new System.Drawing.Size(59, 20);
            this.chkWorkStatus1.TabIndex = 16;
            this.chkWorkStatus1.Text = "退休";
            this.chkWorkStatus1.UseVisualStyleBackColor = true;
            // 
            // chkWorkStatus2
            // 
            this.chkWorkStatus2.AutoSize = true;
            this.chkWorkStatus2.Location = new System.Drawing.Point(381, 189);
            this.chkWorkStatus2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkWorkStatus2.Name = "chkWorkStatus2";
            this.chkWorkStatus2.Size = new System.Drawing.Size(59, 20);
            this.chkWorkStatus2.TabIndex = 17;
            this.chkWorkStatus2.Text = "离休";
            this.chkWorkStatus2.UseVisualStyleBackColor = true;
            // 
            // txtJsName
            // 
            this.txtJsName.Location = new System.Drawing.Point(109, 223);
            this.txtJsName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtJsName.Name = "txtJsName";
            this.txtJsName.Size = new System.Drawing.Size(132, 26);
            this.txtJsName.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 227);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 18;
            this.label6.Text = "持证人";
            // 
            // txtRelation
            // 
            this.txtRelation.Location = new System.Drawing.Point(381, 223);
            this.txtRelation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRelation.Name = "txtRelation";
            this.txtRelation.Size = new System.Drawing.Size(132, 26);
            this.txtRelation.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 227);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 20;
            this.label7.Text = "与逝者关系";
            // 
            // dtBook
            // 
            this.dtBook.CustomFormat = "yyyy-MM-dd";
            this.dtBook.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtBook.Location = new System.Drawing.Point(109, 259);
            this.dtBook.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtBook.Name = "dtBook";
            this.dtBook.Size = new System.Drawing.Size(132, 26);
            this.dtBook.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 263);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 22;
            this.label8.Text = "发证日期";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(381, 263);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(132, 26);
            this.txtPhone.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(287, 267);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 16);
            this.label9.TabIndex = 24;
            this.label9.Text = "联系电话";
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(167, 9);
            this.btnFont.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(100, 31);
            this.btnFont.TabIndex = 7;
            this.btnFont.Text = "打印字体";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // optSex2
            // 
            this.optSex2.AutoSize = true;
            this.optSex2.Location = new System.Drawing.Point(381, 153);
            this.optSex2.Margin = new System.Windows.Forms.Padding(4);
            this.optSex2.Name = "optSex2";
            this.optSex2.Size = new System.Drawing.Size(42, 20);
            this.optSex2.TabIndex = 27;
            this.optSex2.Text = "女";
            this.optSex2.UseVisualStyleBackColor = true;
            // 
            // optSex1
            // 
            this.optSex1.AutoSize = true;
            this.optSex1.Location = new System.Drawing.Point(293, 153);
            this.optSex1.Margin = new System.Windows.Forms.Padding(4);
            this.optSex1.Name = "optSex1";
            this.optSex1.Size = new System.Drawing.Size(42, 20);
            this.optSex1.TabIndex = 26;
            this.optSex1.Text = "男";
            this.optSex1.UseVisualStyleBackColor = true;
            // 
            // FrmPrintTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 600);
            this.Controls.Add(this.optSex2);
            this.Controls.Add(this.optSex1);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dtBook);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtRelation);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtJsName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkWorkStatus2);
            this.Controls.Add(this.chkWorkStatus1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtGh);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSzName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmPrintTest";
            this.Text = "FrmPreview";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPageSet;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtSzName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtGh;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkWorkStatus1;
        private System.Windows.Forms.CheckBox chkWorkStatus2;
        private System.Windows.Forms.TextBox txtJsName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRelation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtBook;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.RadioButton optSex2;
        private System.Windows.Forms.RadioButton optSex1;
    }
}