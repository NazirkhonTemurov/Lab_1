using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
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

            textBox2.Tag = "a";
            textBox2.Tag = "b";
            textBox3.Tag = "e";
            textBox4.Tag = "formula";


            foreach (TextBox textBox in new TextBox[] { textBox1, textBox2, textBox3 })
            {
                textBox.Text = "";
               
                textBox.Validating += (s, a) => {

                    bool approvedDecimalPoint = false;
                    textBox.BackColor = Color.White;
                    if ((s as TextBox).Text == string.Empty)
                    {
                        a.Cancel = false;
                        return;
                    }
                    if ((s as TextBox).Text == ",")
                    {
                        if (!((s as TextBox).Text.Contains(",")))
                            approvedDecimalPoint = true;

                    }
                    else if ((s as string) == "-")
                    {
                        if (!((s as TextBox).Text.Contains("-")))
                            approvedDecimalPoint = true;
                        if (!((s as TextBox).Text.Length > 0))
                            approvedDecimalPoint = true;
                    }

                    if (!(char.IsDigit((s as TextBox).Text, (s as TextBox).Text.Length - 1) || approvedDecimalPoint))
                    {
                        a.Cancel = true;
                        textBox.BackColor = Color.Red;
                    }

                    /*string buffer = textBox.Text;
                    for (int i=0;i<textBox.Text.Length;++i)
                    {
                        if (i == 0 && textBox.Text[i] == '-') continue;
                        if (!char.IsDigit(textBox.Text[i]))
                        {
                            if (i + 1 != buffer.Length) buffer.Remove(i, i + 1);
                            else buffer.Remove(i);
                        }
                    }
                    textBox.Text = buffer;*/
                };

            }
        }

        public static bool CheckTextBoxesValues(TextBox[] UITextBoxes, string a, string b)
        {
            string message = string.Empty;
            double.TryParse(a, out double dblA);
            double.TryParse(b, out double dblB);
            foreach (TextBox textBox in UITextBoxes)
            {
                if (textBox.Text == string.Empty)
                {
                    if (!message.Contains("-Поле не может быть пустым."))
                        message += "-Поле не может быть пустым.\n";
                    continue;

                }
                if ((string)textBox.Tag == "e")
                {

                    double.TryParse(textBox.Text, out double dblE);

                    if (dblE <= 0 || dblE >= (dblB - dblA))
                    {
                        if (!message.Contains("-Значение поля \"e\"  должно соответствовать этому условию: 0<e<(a-b)"))
                            message += "-Значение поля \"e\"  должно соответствовать этому условию: 0<e<(a-b)\n";
                        continue;
                    }
                }
                if (textBox.Text == "0" && (string)textBox.Tag != "a" && (string)textBox.Tag != "b")
                {
                    if (!message.Contains("-Значение поля не может быть нулевым"))
                        message += "-Значение поля не может быть нулевым.\n";
                    continue;
                }

                if (double.TryParse(textBox.Text, out var numberStyles) == true && numberStyles < 0 && (string)textBox.Tag != "a" && (string)textBox.Tag != "b")
                {
                    if (!message.Contains("-Значение поля не может быть отрицательным."))
                        message += "-Значение поля не может быть отрицательным.\n";
                    continue;
                }

                if ((string)textBox.Tag == "formula")
                {
                    if (!textBox.Text.Contains('x'))
                    {
                        if (!message.Contains("-F(x) должен иметь свойство x."))
                            message += "-F(x) должен иметь свойство x.\n";
                        continue;
                    }

                    try
                    {
                        org.matheval.Expression expression = new org.matheval.Expression(textBox.Text.ToLower());
                        expression.Bind("x", dblA);
                        double value = expression.Eval<double>();

                        expression.Bind("x", dblB);
                        value = expression.Eval<double>();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Значение было недопустимо малым или недопустимо большим")
                            && !message.Contains("-Для этого F(x), значения a и b не могут быть отрицательными или равными нулю"))
                            message += "-Для этого F(x), значения a и b не могут быть отрицательными или равными нулю.\n";
                        else if (!message.Contains(ex.Message.ToString()))
                            message += "F(x)= " + ex.Message.ToString() + "\n";
                    }
                }

            }
            if (message != string.Empty)
            {
                MessageBox.Show($"{message}Введите допустимое значение!", "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }



        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            x.Clear();
            y.Clear();
            formsPlot1.Plot.Clear();

            if (CheckTextBoxesValues(
                new TextBox[] { textBox1, textBox2, textBox3, textBox4 },
                textBox1.Text, textBox2.Text))
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
            formsPlot1.Plot.AddScatter(
                new double[] { xs },
                new double[] { ys },
                color: System.Drawing.Color.FromName("Green"),
                markerSize: 7);
            formsPlot1.Plot.AddArrow(Math.Round(xs, 5),
                 Math.Round(ys, 5), xs - 3, ys,
                color: System.Drawing.Color.FromName("Red"));
            formsPlot1.Refresh();


        }

       
    }
}
