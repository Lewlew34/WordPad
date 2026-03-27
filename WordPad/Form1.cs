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
    }
}
