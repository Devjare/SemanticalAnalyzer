using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            dialog.Filter = "All Files (*.KAAS)|*.kaas";
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
                        richTextBox1.AppendText(temp.GetString(b));
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                }

            }
        }
        
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)

        {
           
        }

        
    

        Dictionary<String, Object> SymbolsTable { get; set; }
        private void BtnCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void Compile()
        {
            txtLog.Clear();
            var evaluator = new Evaluador();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var diagnostics = evaluator.Evaluar(richTextBox1.Text);
            
            stopwatch.Stop();            
            var secondsElapsed = stopwatch.ElapsedMilliseconds;
            
            MessageBox.Show($"Tiempo total Compilacion: {secondsElapsed}ms, \n\r" +
                $"Tiempo Analisis Lexico: {evaluator.LexTimeTaken}ms \n\r" +
                $"Tiempo Analisis Sintactico: {evaluator.SintaxTimeTaken}ms \n\r");

            this.SymbolsTable = evaluator.TablaSimbolos;
            
    
            foreach (var diagnostic in diagnostics)
            {
                txtLog.Text += diagnostic + Environment.NewLine;
            }
            ShowSymbolsTable();
            var presentadorCuadruplos = new PersentadorCuadruplos(evaluator.TablaSintaxis);
            MostrarTablaCuadruplos(presentadorCuadruplos.Procesar());
        }

        private void MostrarTablaCuadruplos(Dictionary<String, String> dict)
        {
            dgvCuadruplos.Rows.Clear();
            foreach (var item in dict)
            {
                dgvCuadruplos.Rows.Add(item.Key, item.Value);
            }
        }

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        //metodos para agregar el arbol de carpetas
        private void PopulateTreeView()
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(@"../..");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }
// evento del arbol
        private void TreeView1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void BtnSymbols_Click(object sender, EventArgs e)
        {
            ShowSymbolsTable();
        }

        private void ShowSymbolsTable()
        {
            String data = "";

            if (SymbolsTable == null)
            {
                return;
            }

            dgvSimbolos.Rows.Clear();
            foreach (var item in SymbolsTable)
            {
                dgvSimbolos.Rows.Add(item.Key, (item.Value as Token).Value);
            }
        }

        private void BtnShowLog_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2Collapsed)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void TxtCode_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Compile();
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "prueba.txt";
            // filtros
            save.Filter = "All Files(*.KAAS) | *.kaas";
            if (save.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenNewFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception ocurred: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenNewFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception ocurred: {ex.Message}");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                AddLineNumbers();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TreeNode tNode;
            tNode = treeView1.Nodes.Add("HYDRA");

            treeView1.Nodes[0].Nodes.Add("Programas");
            treeView1.Nodes[0].Nodes[0].Nodes.Add("Archivos");

            treeView1.Nodes[0].Nodes.Add("Compilador");
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Lexico");
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Sintactico");
            treeView1.Nodes[0].Nodes[1].Nodes.Add("Semantico");

            treeView1.Nodes[0].Nodes.Add("Interfaz");
            treeView1.Nodes[0].Nodes[2].Nodes.Add("Visual Studio");
            treeView1.Nodes[0].Nodes[2].Nodes[0].Nodes.Add("C#");

            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void LineNumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            richTextBox1.Select();
            LineNumberTextBox.DeselectAll();
        }

        public int getWidth()
        {
            int w = 25;
            // get total lines of richTextBox1    
            int line = richTextBox1.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)richTextBox1.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)richTextBox1.Font.Size;
            }
            else
            {
                w = 50 + (int)richTextBox1.Font.Size;
            }

            return w;
        }

        public void AddLineNumbers()
        {
            // create & set Point pt to (0,0)    
            Point pt = new Point(0, 0);
            // get First Index & First Line from richTextBox1    
            int First_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int First_Line = richTextBox1.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from richTextBox1    
            int Last_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int Last_Line = richTextBox1.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox    
            LineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value    
            LineNumberTextBox.Text = "";
            LineNumberTextBox.Width = getWidth();
            // now add each line number to LineNumberTextBox upto last line    
            for (int i = First_Line; i <= Last_Line + 2; i++)
            {
                LineNumberTextBox.Text += i + 1 + "\n";
            }
        }
    }
}