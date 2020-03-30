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

        String path = "";
        Boolean isFolder = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            //<exe name> <path> <mode{0,1,2}> <optional string>
            //LogParser.exe c:\\1 1 error
            if (args.Length >= 3)
            {
                //args[0] is exe path
                path = args[1];
                label4.Text = path;

                int mode = Int16.Parse(args[2]);
                if (mode == 0)
                {
                    radioButton1.Checked = true;
                }
                else if (mode == 1)
                {
                    radioButton2.Checked = true;
                    textBox1.Text = args[3];
                }
                else if (mode == 2)
                {
                    radioButton3.Checked = true;
                    textBox2.Text = args[3];
                } else
                {
                    return;
                }

                button3_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    isFolder = true;
                    path = fbd.SelectedPath;
                    label4.Text = path;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                isFolder = false;
                path = openFileDialog1.FileName;
                label4.Text = path;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            StreamWriter writetext = new StreamWriter("ERRORSreport.txt", false);
            writetext.Close();

            if (isFolder || Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                int i = 0;
                foreach (string file in files)
                {
                    printLine("processing " + file);
                    label1.Text = (i + 1) + " / " + files.Length;
                    processFile(files[i]);
                    i++;
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
            foreach(string line in lines)
            {
                progressBar1.Value += 1;

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
