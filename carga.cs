using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemanticalAnalyzer
{
    public partial class Carga : Form
    {
        double total = 0.0;
        public Carga()
        {
            InitializeComponent();
           
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = progressBar1.Minimum;

            total += 0.25;
            if (total == 2 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos.";
            }

            if (total == 3 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos..";
            }

            if (total == 4 && i < progressBar1.Maximum)
            {
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                progressBar1.PerformStep();
                label1.Text = "cargando complementos...";
            }

            if (total == 5 && i < progressBar1.Maximum)
            {
                label1.Text = "sistema cargado";

                for (int isf = i; isf < progressBar1.Maximum; isf = isf + progressBar1.Step)
                {
                    //esta instrucción avanza la posición actual de la barra
                    progressBar1.PerformStep();
                    label1.Text = "sistema cargado";

                }


            }

            if (total == 6)
            {
                Form1 ventana = new Form1();
                this.Hide();

                ventana.Show();
            }
        }

        private void carga_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;

            progressBar1.Value = 0;

            progressBar1.Step = 5;

            timer1.Start();
        }

        private void carga_DoubleClick(object sender, EventArgs e)
        {
            Form1 ventana = new Form1();
            this.Hide();
            ventana.Show();
            
        }
    }
}
