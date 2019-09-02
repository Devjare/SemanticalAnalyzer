﻿using ExpressionEvaluator.CodeAnalysis;
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

namespace SemanticalAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCode_TextChanged(object sender, EventArgs e)
        {
            filesTabControl.TabPages[0].Text = "New File... *";
            
        }

        private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenNewFile();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception ocurred: {ex.Message}");
            }
        }

        private void OpenNewFile()
        {

            var dialog = new OpenFileDialog();
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.Multiselect = true;


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.FileName;
                // string[] arrAllFiles = dialog.FileNames; //used when Multiselect = true           

                // Open the stream and read it back.
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        txtCode.AppendText(temp.GetString(b));
                        txtCode.AppendText(Environment.NewLine);
                    }
                }

            }
        }
        
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtLineNumbers_Repaint();
            }
            else if (e.KeyCode == Keys.Back)
            {
                TxtLineNumbers_Repaint();
            }
        }

        private void TxtLineNumbers_Repaint()
        {
            txtLineNumbers.Clear();
            var lines = txtCode.Lines;

            for (int i = 1; i <= lines.Length + 1; i++)
            {
                txtLineNumbers.AppendText(i.ToString());
                txtLineNumbers.AppendText(Environment.NewLine);
            }

            txtLineNumbers.Update();
        }

        private void BtnCompile_Click(object sender, EventArgs e)
        {

            var evaluator = new Evaluator();
            var diagnostics = evaluator.Evaluate(txtCode.Text);

            foreach (var diagnostic in diagnostics)
            {
                Console.WriteLine($"{diagnostic}");
            }

            if (splitContainer1.Panel2Collapsed)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }
    }
}
