using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt4
{
    public partial class Form1 : Form
    {

        public struct Crane
        {
            public int width;
            public int height;
            public int length;
        }
        public struct Winch
        {
            public int width;
            public int height;
            public int x;
        }
        public struct Hook
        {
            public int width;
            public int height;
            public int x;
            public int y;
        }
        Crane crane;
        Winch winch;
        Hook hook;
        public Form1()
        {

            InitializeComponent();
            crane.width = 20;
            crane.height = 350;
            crane.length = 450;
            winch.x = panel1.Width / 5;
            winch.width = crane.width;
            winch.height = winch.width;
            hook.width = 10;
            hook.height = 10;
            hook.x = winch.x + winch.width / 2 - hook.width / 2;
            hook.y = panel1.Height - crane.height + 50 + winch.height;
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.SolidBrush pen = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            Graphics g = panel1.CreateGraphics();
            g.FillRectangle(pen, new Rectangle(panel1.Width / 5 - crane.width, panel1.Height - crane.height, crane.width, crane.height));
            g.FillRectangle(pen, new Rectangle(panel1.Width / 5 - crane.width - 50, panel1.Height - crane.height + 50, crane.length, crane.width));
            pen = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            g.FillRectangle(pen, new Rectangle(winch.x, panel1.Height - crane.height + 50, winch.width, winch.height));
            pen = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            g.FillRectangle(pen, new Rectangle(hook.x, hook.y, hook.width, hook.height));
            pen = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            g.FillRectangle(pen, new Rectangle(hook.x + 4, panel1.Height - crane.height + 50 + winch.height, 2, hook.y - (panel1.Height - crane.height + 50 + winch.height)));
            pen = new System.Drawing.SolidBrush(System.Drawing.Color.Brown);
            pen.Dispose();
            g.Dispose();

        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (winch.x <= panel1.Width / 5) winch.x = panel1.Width / 5;
            else
            {
                winch.x -= 10;
                hook.x -= 10;
            }
            panel1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (winch.x >= crane.length - 50 - 2 * winch.width + panel1.Width / 5) winch.x = crane.length - 2 * winch.width - 50 + panel1.Width / 5;
            else
            {
                winch.x += 10;
                hook.x += 10;
            }
            panel1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (hook.y >= panel1.Height - hook.height) hook.y = panel1.Height - hook.height;
            else
            {
                hook.y += 10;
            }
            panel1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (hook.y <= panel1.Height - crane.height + 50 + winch.height) hook.y = panel1.Height - crane.height + 50 + winch.height;
            else
            {
                hook.y -= 10;
            }
            panel1.Refresh();
        }


    }
}