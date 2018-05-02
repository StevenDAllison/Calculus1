using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Calculus1
{
    public partial class Form1 : Form
    {
        public int filedownload;
        class row
        {
            public double time;
            public double voltage;
            public double current;
            public double voltageDerivative;
            public double resistance;
        }

        List<row> table = new List<row>();
                
        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            filedownload = 0;
        }

        void tableSort()
        {
            table = table.OrderBy(x => x.time).ToList();
        }

        void calculateresist()
        {
            for (int i = 0; i < table.Count; i++)
            {                
                table[i].resistance = table[i].voltage / table[i].current;
            }
        }

        void derivative()
        {
            for (int i=1; i < table.Count; i++)
            {
                double dV = table[i].voltage - table[i - 1].voltage;
                double dt = table[i].time - table[i - 1].time;
                table[i].voltageDerivative = dV / dt;
            }
        }
            
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "CSV Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] l = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(l[0]);
                            table.Last().voltage = double.Parse(l[1]);
                            table.Last().current = double.Parse(l[2]);
                        }
                    }
                    MessageBox.Show(openFileDialog1.FileName + " opened correctly.");
                    filedownload = 1;
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " failed to open.");
                    filedownload = 0;
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format.");
                    filedownload = 0;
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                    filedownload = 0;
                }
            }
        }

        private void vTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;            
            Series series1 = new Series
            {
                Name = "Voltage (V)",
                Color = Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].voltage);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "Voltage (V)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            if (filedownload = 0)
            {
                MessageBox.Show("Download a data set and choose a graph type.");
            }            
        }

        private void itToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            
            Series series1 = new Series
            {
                Name = "Current (A)",
                Color = Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].current);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "Current (A)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dVdtTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            derivative();
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;

            Series series1 = new Series
            {
                Name = "dV/dt (V/s)",
                Color = Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            chart1.Series.Add(series1);
            for (int i = 1; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].voltageDerivative);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "dV/dt (V/s)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void vITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calculateresist();
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;

            Series series1 = new Series
            {
                Name = "R (Ω)",
                Color = Color.Blue,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].resistance);
            }
            chart1.ChartAreas[0].AxisX.Title = "time (s)";
            chart1.ChartAreas[0].AxisY.Title = "R (Ω)";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }
    }
}
