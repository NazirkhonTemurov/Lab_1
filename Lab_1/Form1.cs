using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;

namespace Lab_1
{
    public partial class Form1 : Form
    {
        private List<double> x = new List<double>();
        private List<double> y = new List<double>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlot1.Plot.Title("f(x)");
        }

        private void name_Tick(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double.TryParse(textBox1.Text, out var StartPoint);
            double.TryParse(textBox2.Text, out var EndPoint);
            double.TryParse(textBox3.Text, out var Accuracy);
            double a = StartPoint;
            double b = EndPoint;
            double x1, x2;
            double Middle = 0, Result;

            //метод чтобы найти точки минимума
            double IncrementStep = Accuracy / 10;

            int count = 0;
            /*try
            {*/
            while (true)
            {
                ++count;

                x1 = (StartPoint + EndPoint - IncrementStep) / 2;
                x2 = (StartPoint + EndPoint + IncrementStep) / 2;
                try
                {
                    if (F(x1) <= F(x2))
                        EndPoint = x2;
                    else
                        StartPoint = x1;

                    Middle = (EndPoint - StartPoint) / 2;

                    if (Middle <= Accuracy)
                    {
                        Result = (StartPoint + EndPoint) / 2;
                        formsPlot1.Plot.Clear();
                        formsPlot1.Refresh();
                        Show(a, b, Result);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    break; ;
                }
            }

        }
        

        private double F(double X)
        {
            org.matheval.Expression expression = new org.matheval.Expression(textBox4.Text.ToLower());
            expression.Bind("x", X);
            decimal value = expression.Eval<decimal>();
            /*System.Windows.Forms.MessageBox.Show(value.ToString());*/
            return (double)value;
        }

        private void отчиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Show(double StartPoint, double EndPoint, double Result)
        {
            double xs = Result;
            double ys = F(Result);
            for (double i = StartPoint; i <= EndPoint; i += 0.1)
            {
                x.Add(i);
                y.Add(F(i));
            }
            formsPlot1.Plot.Title($"Точка минимум - min[{Math.Round(xs, 5)} ; {Math.Round(ys, 5)}]");
            formsPlot1.Plot.AddScatter(x.ToArray(), y.ToArray(), markerShape: MarkerShape.none, lineWidth: 3);
            /*formsPlot1.Plot.AddScatter(
                new double[] { xs },
                new double[] { ys },
                color: System.Drawing.Color.FromName("Green"),
                markerSize: 7);
            formsPlot1.Plot.AddArrow(xs,
                 ys, xs - 3, ys,
                color: System.Drawing.Color.FromName("Red"));*/
            formsPlot1.Refresh();


        }

    }
}
