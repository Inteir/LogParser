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
using System.Text.RegularExpressions;

namespace LogParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        String path = "C:\\1\test log.log";
        Boolean folder = false;
        

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folder = true;
                    path = fbd.SelectedPath;
                    label4.Text = path;
                   // 

                  //  System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Log files (*.log)|*.log";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                folder = false;
                string selectedFileName = openFileDialog1.FileName;
                path = selectedFileName;
                label4.Text = path;
                //...
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (folder)
            {
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i <files.Length; i++)
                {
                    if (files[i].ToLower().Contains(".log"))
                    {
                        processFile(files[i]);
                    }
                }
            }
            else
            {
                processFile(path);
            }
           
        }

        private void processFile(String filePath)
        {
            var lines = File.ReadAllLines(filePath);
            for (var i = 0; i < lines.Length; i += 1)
            {
                var line = lines[i];
                // Process line
                if (radioButton1.Checked)
                {
                    if (line.ToLower().Contains("error"))
                    {
                        printLine(line);
                    }
                }
                if (radioButton2.Checked)
                {
                    if (line.Contains(textBox1.Text))
                    {
                        printLine(line);
                    }
                }
                if (radioButton3.Checked)
                {
                    if (Regex.Matches(line, textBox2.Text, RegexOptions.IgnoreCase).Count > 0)
                    {
                        printLine(line);
                    }
                }
            }
        }

        private void printLine(String line)
        {
           richTextBox1.AppendText("\r\n" + line);
            richTextBox1.ScrollToCaret();
            //
        }
    }
}
