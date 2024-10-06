using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zhaoxi.Calculator
{
    public partial class MainForm : Form
    {

        // 算式
        private string exp = "";

        public string Exp
        {
            get { return exp; }
            set { exp = value; }
        }

        public MainForm()
        {
            InitializeComponent();

            this.Load += MainForm_Load;
            this.SizeChanged += MainForm_SizeChanged;
        }

        // 窗体默认宽高
        int normalWidth = 0;
        int normalHeight = 0;
        // 需要记录的控件位置以及宽高
        Dictionary<string, Rect> normalControl = new Dictionary<string, Rect>();

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 记录相关对象以及原始尺寸
            normalWidth = this.btnsPanel.Width;
            normalHeight = this.btnsPanel.Height;
            // 通过父 Panel 进行控件遍历
            foreach (Control item in this.btnsPanel.Controls)
            {
                normalControl.Add(item.Name, new Rect(item.Left, item.Top, item.Width, item.Height));
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // 根据原始比例进行新尺寸的计算
            int w = this.btnsPanel.Width;
            int h = this.btnsPanel.Height;
            foreach (Control item in this.btnsPanel.Controls)
            {
                int newX = (int)(w * 1.0 / normalWidth * normalControl[item.Name].X);
                int newY = (int)(h * 1.0 / normalHeight * normalControl[item.Name].Y);
                int newW = (int)(w * 1.0 / normalWidth * normalControl[item.Name].Width);
                int newH = (int)(h * 1.0 / normalHeight * normalControl[item.Name].Height);

                item.Left = newX;
                item.Top = newY;
                item.Width = newW;
                item.Height = newH;
            }
        }

        /// <summary>
        /// 点击数字按钮
        /// </summary>
        /// <param name="sender">触发这个（事件）方法的对象</param>
        /// <param name="e">鼠标动作参数</param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 获取到触发这个事件的对象，获取Text属性值
            Button button = sender as Button;

            if (button.Text == ".")
            {
                if (string.IsNullOrEmpty(exp))
                    exp += "0";
                exp += button.Text;
            }
            else
            {
                exp += button.Text;
            }
            this.label1.Text = ExchangeExp(exp);
        }

        /// <summary>
        /// 点击运算符按钮
        /// </summary>
        /// <param name="sender">触发这个（事件）方法的对象</param>
        /// <param name="e">鼠标动作参数</param>
        private void button2_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string op = button.Tag.ToString();

            /*// 多个数值计算
            if (!string.IsNullOrEmpty(Number2))
            {
                // 执行计算，将计算结果给Number1
                button14_Click(null, null);
            }*/

            exp += op;
            this.label1.Text = ExchangeExp(exp);
        }

        /// <summary>
        /// 点击等于号按钮
        /// </summary>
        /// <param name="sender">触发这个（事件）方法的对象</param>
        /// <param name="e">鼠标动作参数</param>
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                exp = new DataTable().Compute(exp, "").ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("捕获到异常: " + ex.Message);
                exp = "表达式有误!";
            }
            this.label1.Text = ExchangeExp(exp);
        }

        private string ExchangeExp(string exp)
        {
            string showExp = exp;
            showExp = showExp.Replace('*', '×');
            showExp = showExp.Replace('/', '÷');
            return showExp;
        }

        /// <summary>
        /// 点击清空按钮
        /// </summary>
        private void button17_Click(object sender, EventArgs e)
        {
            exp = "";
            this.label1.Text = ExchangeExp(exp) + "0";
        }
    }

    class Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rect(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }
    }
}
