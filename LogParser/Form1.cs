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
        
        String path = "C:\\1";
        Boolean folder = true;
        

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folder = true;
                    path = fbd.SelectedPath;
                    label4.Text = path;
                    // 
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

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            StreamWriter writetext = new StreamWriter("ERRORSreport.txt", false);
            writetext.Close();

            if (folder)
            {
                string[] files = Directory.GetFiles(path);
                int logFilesCount = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].ToLower().Contains(".log"))
                    {
                        logFilesCount++;
                    }
                }

                int j = 0;
                for (int i = 0; i <files.Length; i++)
                {
                    if (files[i].ToLower().Contains(".log"))
                    {
                        j++;
                        printLine("processing " + files[i]);
                        label1.Text = j + " / " + logFilesCount;
                        processFile(files[i]);
                    }
                }
            }
            else
            {
                label1.Text = "";
                processFile(path);
            }
           
        }

        private void processFile(String filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            progressBar1.Maximum = lines.Length;
            progressBar1.Value = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                progressBar1.Value += 1;
                string line = lines[i];
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

            using (StreamWriter writetext = new StreamWriter("ERRORSreport.txt", true))
            {
                writetext.WriteLine(line);
            }
        }
    }
}
