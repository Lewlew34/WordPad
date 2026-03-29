using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordPad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                var result = MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e); 
                }
            }
            richTextBox1.Clear();
            currentFilePath = string.Empty;
            this.Text = "My Editor - New Document";
            richTextBox1.Focus();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt|My Word|*.rft";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.EndsWith(".txt"))
                {
                    richTextBox1.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                }
                else
                {
                    richTextBox1.LoadFile(ofd.FileName, RichTextBoxStreamType.RichText);
                }
            }
        }

        private string currentFilePath = string.Empty; // Biến giữ đường dẫn file đang mở/lưu
        private void SaveFileToDisk(string path)
        {
            if (path.EndsWith(".txt"))
            {
                richTextBox1.SaveFile(path, RichTextBoxStreamType.PlainText);
            }
            else
            {
                // RichTextBox mặc định dùng đuôi .rtf
                richTextBox1.SaveFile(path, RichTextBoxStreamType.RichText);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveFileToDisk(currentFilePath);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Text Files|*.txt|My Word|*.rft";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = sfd.FileName;
                SaveFileToDisk(currentFilePath);
                this.Text = "My Editor - " + Path.GetFileName(currentFilePath);
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FontDialog();
            fd.ShowColor = true;
            fd.ShowApply = true;
            fd.Apply += new EventHandler(XuLyApplyFont);
            if (fd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = fd.Font;
                richTextBox1.SelectionColor = fd.Color;
            }
        }
        private void XuLyApplyFont(object sender, EventArgs e)
        {
            var fd = sender as FontDialog;
            richTextBox1.SelectionFont = fd.Font;
            richTextBox1.SelectionColor = fd.Color;
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = cd.Color;
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void insertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(ofd.FileName);
                    Clipboard.SetImage(img);
                    richTextBox1.Paste();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting image: " + ex.Message);
                }
            }
        }

        private void addBulletsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBullet = true;
        }

        private void removeBulletsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBullet = false;
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 0;
        }

        private void ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 5;
        }

        private void ptsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 10;
        }

        private void ptsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 15;
        }

        private void ptsToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 20;
        }
        // Hàm hiển thị hộp thoại nhập liệu
        public static string Prompt(string text)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = text
            };

            Label lbl = new Label() { Left = 10, Top = 10, Text = text };
            TextBox txt = new TextBox() { Left = 10, Top = 40, Width = 260 };
            Button ok = new Button() { Text = "OK", Left = 200, Width = 70, Top = 70 };

            ok.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(lbl);
            prompt.Controls.Add(txt);
            prompt.Controls.Add(ok);

            prompt.ShowDialog();

            return txt.Text;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            string find = Prompt("Nhập từ cần tìm:");

            if (string.IsNullOrEmpty(find)) return;

            int start = richTextBox1.SelectionStart + richTextBox1.SelectionLength;

            int index = richTextBox1.Find(find, start, RichTextBoxFinds.None);

            if (index == -1)
            {
                MessageBox.Show("Không tìm thấy!");
            }
        }

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string find = Prompt("Nhập từ cần tìm:");
            if (string.IsNullOrEmpty(find)) return;

            string replace = Prompt("Nhập từ thay thế:");

            int start = richTextBox1.SelectionStart + richTextBox1.SelectionLength;

            int index = richTextBox1.Find(find, start, RichTextBoxFinds.None);

            if (index != -1)
            {
                richTextBox1.SelectedText = replace;
            }
            else
            {
                MessageBox.Show("Không tìm thấy!");
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument1;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = richTextBox1.Text;

            Font font = richTextBox1.Font;
            float lineHeight = font.GetHeight(e.Graphics);

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            foreach (string line in text.Split('\n'))
            {
                e.Graphics.DrawString(line, font, Brushes.Black, x, y);
                y += lineHeight;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Thông tin nhóm\n\n" +
                "Lê Thành Huy - 2311553353 - 23BITV01\n\n" +
                "Nguyễn Nhật Long - 2311552884 - 23BITV01\n\n" +
                "Đặng Anh Khoa - 2311553740 - 23BITV01\n\n");
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.Focus();
        }
    }
}
