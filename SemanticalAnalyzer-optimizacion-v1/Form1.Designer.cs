﻿namespace SemanticalAnalyzer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Node5", new System.Windows.Forms.TreeNode[] {
            treeNode13});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tblLayoutLevel0 = new System.Windows.Forms.TableLayoutPanel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panelLog = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tablePanel = new System.Windows.Forms.Panel();
            this.tcResults = new System.Windows.Forms.TabControl();
            this.tbCuadruplos = new System.Windows.Forms.TabPage();
            this.dgvCuadruplos = new System.Windows.Forms.DataGridView();
            this.dgvcResultado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOperando2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOperando1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcOperador = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpTablaSimbolos = new System.Windows.Forms.TabPage();
            this.dgvSimbolos = new System.Windows.Forms.DataGridView();
            this.dgvcValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCode = new System.Windows.Forms.Panel();
            this.filesTabControl = new System.Windows.Forms.TabControl();
            this.newFileTabPage = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtLineNumbers = new System.Windows.Forms.TextBox();
            this.panelFileManager = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.tblLayoutLevel0.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tablePanel.SuspendLayout();
            this.tcResults.SuspendLayout();
            this.tbCuadruplos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuadruplos)).BeginInit();
            this.tpTablaSimbolos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimbolos)).BeginInit();
            this.panelCode.SuspendLayout();
            this.filesTabControl.SuspendLayout();
            this.newFileTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panelFileManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblLayoutLevel0
            // 
            this.tblLayoutLevel0.BackColor = System.Drawing.Color.Indigo;
            this.tblLayoutLevel0.ColumnCount = 1;
            this.tblLayoutLevel0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutLevel0.Controls.Add(this.splitContainer1, 0, 2);
            this.tblLayoutLevel0.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tblLayoutLevel0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutLevel0.Location = new System.Drawing.Point(0, 0);
            this.tblLayoutLevel0.Name = "tblLayoutLevel0";
            this.tblLayoutLevel0.RowCount = 3;
            this.tblLayoutLevel0.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutLevel0.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutLevel0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutLevel0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayoutLevel0.Size = new System.Drawing.Size(1331, 719);
            this.tblLayoutLevel0.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Indigo;
            this.statusStrip1.Location = new System.Drawing.Point(0, 719);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1331, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panelLog
            // 
            this.panelLog.BackColor = System.Drawing.Color.Transparent;
            this.panelLog.Controls.Add(this.tabControl1);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLog.Location = new System.Drawing.Point(0, 0);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(1323, 266);
            this.panelLog.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabOutput);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1323, 266);
            this.tabControl1.TabIndex = 0;
            // 
            // tabOutput
            // 
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(1315, 239);
            this.tabOutput.TabIndex = 1;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // tabLog
            // 
            this.tabLog.BackColor = System.Drawing.Color.DarkOrchid;
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(1315, 240);
            this.tabLog.TabIndex = 0;
            this.tabLog.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.DarkRed;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1309, 234);
            this.txtLog.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panelFileManager);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1325, 365);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.Indigo;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panelCode);
            this.splitContainer3.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer3.Panel1MinSize = 75;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tablePanel);
            this.splitContainer3.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer3.Size = new System.Drawing.Size(1084, 363);
            this.splitContainer3.SplitterDistance = 802;
            this.splitContainer3.TabIndex = 0;
            // 
            // tablePanel
            // 
            this.tablePanel.Controls.Add(this.tcResults);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.Size = new System.Drawing.Size(278, 363);
            this.tablePanel.TabIndex = 0;
            // 
            // tcResults
            // 
            this.tcResults.Controls.Add(this.tpTablaSimbolos);
            this.tcResults.Controls.Add(this.tbCuadruplos);
            this.tcResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcResults.Location = new System.Drawing.Point(0, 0);
            this.tcResults.Name = "tcResults";
            this.tcResults.SelectedIndex = 0;
            this.tcResults.Size = new System.Drawing.Size(278, 363);
            this.tcResults.TabIndex = 0;
            // 
            // tbCuadruplos
            // 
            this.tbCuadruplos.Controls.Add(this.dgvCuadruplos);
            this.tbCuadruplos.Location = new System.Drawing.Point(4, 22);
            this.tbCuadruplos.Name = "tbCuadruplos";
            this.tbCuadruplos.Padding = new System.Windows.Forms.Padding(3);
            this.tbCuadruplos.Size = new System.Drawing.Size(271, 381);
            this.tbCuadruplos.TabIndex = 1;
            this.tbCuadruplos.Text = "Cuadruplos";
            this.tbCuadruplos.UseVisualStyleBackColor = true;
            // 
            // dgvCuadruplos
            // 
            this.dgvCuadruplos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCuadruplos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCuadruplos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcOperador,
            this.dgvcOperando1,
            this.dgvcOperando2,
            this.dgvcResultado});
            this.dgvCuadruplos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCuadruplos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvCuadruplos.Location = new System.Drawing.Point(3, 3);
            this.dgvCuadruplos.Name = "dgvCuadruplos";
            this.dgvCuadruplos.RowHeadersVisible = false;
            this.dgvCuadruplos.Size = new System.Drawing.Size(265, 375);
            this.dgvCuadruplos.TabIndex = 1;
            // 
            // dgvcResultado
            // 
            this.dgvcResultado.HeaderText = "Resultado";
            this.dgvcResultado.Name = "dgvcResultado";
            // 
            // dgvcOperando2
            // 
            this.dgvcOperando2.HeaderText = "Operando 2";
            this.dgvcOperando2.Name = "dgvcOperando2";
            // 
            // dgvcOperando1
            // 
            this.dgvcOperando1.HeaderText = "Operando 1";
            this.dgvcOperando1.Name = "dgvcOperando1";
            // 
            // dgvcOperador
            // 
            this.dgvcOperador.HeaderText = "Operador";
            this.dgvcOperador.Name = "dgvcOperador";
            // 
            // tpTablaSimbolos
            // 
            this.tpTablaSimbolos.Controls.Add(this.dgvSimbolos);
            this.tpTablaSimbolos.Location = new System.Drawing.Point(4, 22);
            this.tpTablaSimbolos.Name = "tpTablaSimbolos";
            this.tpTablaSimbolos.Padding = new System.Windows.Forms.Padding(3);
            this.tpTablaSimbolos.Size = new System.Drawing.Size(270, 337);
            this.tpTablaSimbolos.TabIndex = 0;
            this.tpTablaSimbolos.Text = "TablaSimbolos";
            this.tpTablaSimbolos.UseVisualStyleBackColor = true;
            // 
            // dgvSimbolos
            // 
            this.dgvSimbolos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSimbolos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSimbolos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcKey,
            this.dgvcValue});
            this.dgvSimbolos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSimbolos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSimbolos.Location = new System.Drawing.Point(3, 3);
            this.dgvSimbolos.Name = "dgvSimbolos";
            this.dgvSimbolos.RowHeadersVisible = false;
            this.dgvSimbolos.Size = new System.Drawing.Size(264, 331);
            this.dgvSimbolos.TabIndex = 0;
            // 
            // dgvcValue
            // 
            this.dgvcValue.HeaderText = "Value";
            this.dgvcValue.Name = "dgvcValue";
            // 
            // dgvcKey
            // 
            this.dgvcKey.HeaderText = "Key";
            this.dgvcKey.Name = "dgvcKey";
            // 
            // panelCode
            // 
            this.panelCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelCode.Controls.Add(this.filesTabControl);
            this.panelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCode.Location = new System.Drawing.Point(0, 0);
            this.panelCode.Name = "panelCode";
            this.panelCode.Size = new System.Drawing.Size(802, 363);
            this.panelCode.TabIndex = 4;
            // 
            // filesTabControl
            // 
            this.filesTabControl.Controls.Add(this.newFileTabPage);
            this.filesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesTabControl.Location = new System.Drawing.Point(0, 0);
            this.filesTabControl.Multiline = true;
            this.filesTabControl.Name = "filesTabControl";
            this.filesTabControl.SelectedIndex = 0;
            this.filesTabControl.Size = new System.Drawing.Size(802, 363);
            this.filesTabControl.TabIndex = 1;
            // 
            // newFileTabPage
            // 
            this.newFileTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.newFileTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newFileTabPage.Controls.Add(this.splitContainer4);
            this.newFileTabPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newFileTabPage.Location = new System.Drawing.Point(4, 22);
            this.newFileTabPage.Name = "newFileTabPage";
            this.newFileTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.newFileTabPage.Size = new System.Drawing.Size(794, 337);
            this.newFileTabPage.TabIndex = 0;
            this.newFileTabPage.Text = "New File";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.BackColor = System.Drawing.Color.LightGray;
            this.splitContainer4.Panel1.Controls.Add(this.txtLineNumbers);
            this.splitContainer4.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer4.Panel2.Controls.Add(this.txtCode);
            this.splitContainer4.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.Size = new System.Drawing.Size(786, 329);
            this.splitContainer4.SplitterDistance = 32;
            this.splitContainer4.TabIndex = 0;
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Font = new System.Drawing.Font("Courier New", 12.25F);
            this.txtCode.Location = new System.Drawing.Point(0, 0);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCode.Size = new System.Drawing.Size(750, 329);
            this.txtCode.TabIndex = 0;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCode_KeyDown_1);
            // 
            // txtLineNumbers
            // 
            this.txtLineNumbers.BackColor = System.Drawing.Color.White;
            this.txtLineNumbers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLineNumbers.Enabled = false;
            this.txtLineNumbers.Font = new System.Drawing.Font("Courier New", 12.25F);
            this.txtLineNumbers.Location = new System.Drawing.Point(0, 0);
            this.txtLineNumbers.Multiline = true;
            this.txtLineNumbers.Name = "txtLineNumbers";
            this.txtLineNumbers.Size = new System.Drawing.Size(32, 329);
            this.txtLineNumbers.TabIndex = 1;
            this.txtLineNumbers.Text = "1";
            // 
            // panelFileManager
            // 
            this.panelFileManager.BackColor = System.Drawing.Color.White;
            this.panelFileManager.Controls.Add(this.treeView1);
            this.panelFileManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFileManager.Location = new System.Drawing.Point(0, 0);
            this.panelFileManager.Name = "panelFileManager";
            this.panelFileManager.Size = new System.Drawing.Size(233, 363);
            this.panelFileManager.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode8.Name = "Node2";
            treeNode8.Text = "Node2";
            treeNode9.Name = "Node1";
            treeNode9.Text = "Node1";
            treeNode10.Name = "Node3";
            treeNode10.Text = "Node3";
            treeNode11.Name = "Node4";
            treeNode11.Text = "Node4";
            treeNode12.Name = "root";
            treeNode12.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode12.Text = "Node0";
            treeNode13.Name = "Node6";
            treeNode13.Text = "Node6";
            treeNode14.Name = "Node5";
            treeNode14.Text = "Node5";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode14});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(233, 363);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TreeView1_MouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.DarkOrchid;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 79);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.panelLog);
            this.splitContainer1.Size = new System.Drawing.Size(1325, 637);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(134, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(65, 64);
            this.button3.TabIndex = 9;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(64, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 64);
            this.button2.TabIndex = 8;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 64);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Indigo;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.679245F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.35849F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.735849F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.37736F));
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1325, 70);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(209, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(154, 55);
            this.button4.TabIndex = 10;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Indigo;
            this.ClientSize = new System.Drawing.Size(1331, 741);
            this.Controls.Add(this.tblLayoutLevel0);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tblLayoutLevel0.ResumeLayout(false);
            this.panelLog.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tablePanel.ResumeLayout(false);
            this.tcResults.ResumeLayout(false);
            this.tbCuadruplos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuadruplos)).EndInit();
            this.tpTablaSimbolos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimbolos)).EndInit();
            this.panelCode.ResumeLayout(false);
            this.filesTabControl.ResumeLayout(false);
            this.newFileTabPage.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panelFileManager.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblLayoutLevel0;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panelFileManager;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panelCode;
        private System.Windows.Forms.TabControl filesTabControl;
        private System.Windows.Forms.TabPage newFileTabPage;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TextBox txtLineNumbers;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Panel tablePanel;
        private System.Windows.Forms.TabControl tcResults;
        private System.Windows.Forms.TabPage tpTablaSimbolos;
        private System.Windows.Forms.DataGridView dgvSimbolos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcValue;
        private System.Windows.Forms.TabPage tbCuadruplos;
        private System.Windows.Forms.DataGridView dgvCuadruplos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOperador;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOperando1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOperando2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcResultado;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}
