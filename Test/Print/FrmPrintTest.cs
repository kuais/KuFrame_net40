using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Test
{
    public partial class FrmPrintTest : Form
    {
        PrintDocument printDocument;
        StringReader lineReader = null;
        PageSettings pageSettings;
        Font printFont;
        Color printColor;
        Image bg;
        bool isPreview = false;

        public FrmPrintTest()
        {
            InitializeComponent();
            optSex1.Click += OptSex_Click;
            optSex2.Click += OptSex_Click;

            bg = Image.FromFile("bg.jpg");
            printDocument = new PrintDocument();
            printDocument.OriginAtMargins = true;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("p", InchFromMm(12500), InchFromMm(16000));
            printDocument.PrintPage += PrintDocument_PrintPage;
            pageSettings = new PageSettings(printDocument.PrinterSettings);
            pageSettings.Margins = new Margins(0, 0, 0, 0);
            printFont = this.Font;
            printColor = this.ForeColor;

            #region sample
            txtID.Text = "90011234";
            txtSzName.Text = "关羽";
            txtPosition.Text = "白帝厅9排1行1列";
            txtJsName.Text = "关平";
            txtPhone.Text = "123456789";
            txtRelation.Text = "义子";
            dtGh.Text = "2016-12-06";
            dtBook.Text = "2018-12-06";
            optSex1.Checked = true;
            chkWorkStatus1.Checked = true;
            #endregion
        }

        private void OptSex_Click(object sender, EventArgs e)
        {
            RadioButton opt = sender as RadioButton;
            if (opt.Name.EndsWith("1"))
                optSex2.Checked = false;
            else
                optSex1.Checked = false;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;                //获得绘图对象
            if (isPreview)
                g.DrawImage(bg, 0, 0);              //打印背景图
            float left,top = 0;                     //绘制字符串的纵向位置
            string text = null;                     //行字符串
            SolidBrush brush = new SolidBrush(printColor);       //刷子
            //编号
            left = InchFromMm(3500) + pageSettings.Margins.Left;
            top = InchFromMm(2000) + pageSettings.Margins.Top;
            text = txtID.Text;
            g.DrawString(text, printFont, brush, left, top);
            //位置
            top = InchFromMm(3300) + pageSettings.Margins.Top; ;
            text = txtPosition.Text;
            g.DrawString(text, printFont, brush, left, top);
            //逝者姓名
            top = InchFromMm(4600) + pageSettings.Margins.Top; ;
            text = txtSzName.Text;
            g.DrawString(text, printFont, brush, left, top);
            //安放时间
            top = InchFromMm(6000) + pageSettings.Margins.Top; ;
            text = dtGh.Text;
            g.DrawString(text, printFont, brush, left, top);
            //持证人
            top = InchFromMm(11200) + pageSettings.Margins.Top; ;
            text = txtJsName.Text;
            g.DrawString(text, printFont, brush, left, top);
            //发证日期
            top = InchFromMm(13500) + pageSettings.Margins.Top; ;
            text = dtBook.Text;
            g.DrawString(text, printFont, brush, left, top);
            //性别
            left = InchFromMm(9000) + pageSettings.Margins.Left; ;
            top = InchFromMm(4600) + pageSettings.Margins.Top; ;
            text = (optSex1.Checked) ? "男" : "女";
            g.DrawString(text, printFont, brush, left, top);
            //与逝者关系
            top = InchFromMm(11200) + pageSettings.Margins.Top; ;
            text = txtRelation.Text;
            g.DrawString(text, printFont, brush, left, top);
            //联系电话
            top = InchFromMm(13500) + pageSettings.Margins.Top; ;
            text = txtPhone.Text;
            g.DrawString(text, printFont, brush, left, top);
            //退休、离休
            top = InchFromMm(6000) + pageSettings.Margins.Top; ;
            text = "✔";
            if (chkWorkStatus1.Checked)
            {
                left = InchFromMm(7800) + pageSettings.Margins.Left;
                g.DrawString(text, printFont, brush, left, top);
            }
            if (chkWorkStatus2.Checked)
            {
                left = InchFromMm(10400) + pageSettings.Margins.Left;
                g.DrawString(text, printFont, brush, left, top);
            }

            e.HasMorePages = false;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            lineReader = new StringReader("90010001");
            isPreview = true;
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = printDocument;
            dlg.ShowDialog();
            isPreview = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            lineReader = new StringReader("90010001");
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDocument;
            DialogResult dRet = dlg.ShowDialog();
            if (dRet == DialogResult.OK)
            {
                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    printDocument.PrintController.OnEndPrint(printDocument, new PrintEventArgs());
                }
            }
        }

        private void btnPageSet_Click(object sender, EventArgs e)
        {
            PageSetupDialog dlg = new PageSetupDialog();
            dlg.PageSettings = pageSettings;
            dlg.EnableMetric = false;
            dlg.AllowOrientation = false;
            dlg.ShowDialog();
        }

        private int InchFromMm(int v)
        {
            return (int)(v / 25.4);
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.AllowVerticalFonts = false;
            dlg.ShowColor = true;
            dlg.Font = printFont;
            dlg.Color = printColor;
            DialogResult dRet = dlg.ShowDialog();
            if (dRet == DialogResult.OK)
            {
                printFont = dlg.Font;
                printColor = dlg.Color;
            }
        }
    }
}
