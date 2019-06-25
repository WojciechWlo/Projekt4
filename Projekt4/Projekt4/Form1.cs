﻿using System;
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
            public int hanging;
        }
        public struct Block
        {
            public int width;
            public int height;
            public int x;
            public int y;
            public int weight;
            public char shape;
            public int level;
            public bool grounded;
            public bool below;
        }
        Crane crane;
        Winch winch;
        Hook hook;
        List<Block> blocks = new List<Block>();
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
            hook.hanging = -1;
            hook.x = winch.x + winch.width / 2 - hook.width / 2;
            hook.y = panel1.Height - crane.height + 50 + winch.height;
            int []weights = { 100, 150, 200, 100, 100, 200, 300, 150 };
            char[] shapes = { 'r', 'r', 'c', 'r', 'r', 'c', 'c', 'c' };
            for(int i=0;i<8; i++)
            {
                Block block = new Block();
                block.width = 40;
                block.height = 40;
                block.y = panel1.Height - block.height;
                block.x = 130 + i * 40 + 10 * i;
                block.grounded = true;
                block.level = 0;
                block.weight = weights[i];
                block.shape = shapes[i];
                blocks.Add(block);

            }
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.SolidBrush pen = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            Pen pen2 = new Pen(Color.Brown, 1);
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
            for (int i = 0; i < blocks.Count; i++)
            {
                if(blocks[i].shape =='r')g.FillRectangle(pen, new Rectangle(blocks[i].x, blocks[i].y, blocks[i].width, blocks[i].height));
                else g.DrawEllipse(pen2, new Rectangle(blocks[i].x, blocks[i].y, blocks[i].width, blocks[i].height));
            }
            pen2.Dispose();
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
                move_block(-10, 'x');
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
                move_block(10, 'x');
            }
            panel1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (hook.y >= panel1.Height - hook.height) hook.y = panel1.Height - hook.height;
            else
            {
                hook.y += 10;
                move_block(10, 'y');
            }
            panel1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (hook.y <= panel1.Height - crane.height + 50 + winch.height) hook.y = panel1.Height - crane.height + 50 + winch.height;
            else
            {
                hook.y -= 10;
                move_block(-10, 'y');
            }
            panel1.Refresh();
        }

        private int collision_hook_blocks()
        {
            for(int i=0; i<blocks.Count; i++)
            {
                if (hook.x +hook.width >=blocks[i].x && hook.x<=blocks[i].x+blocks[i].width && hook.y + hook.height >= blocks[i].y && hook.y <= blocks[i].y + blocks[i].height)
                {
                    return i;
                }
            }
            return -1;
        }

        private void move_block(int arg, char dir)
        {
            if(hook.hanging!=-1)
            {
                Block block = blocks[hook.hanging];
                if (dir == 'x')
                {
                    block.x += arg;
                }
                else
                {
                    block.y += arg;
                    if(block.y+block.height>panel1.Height)
                    {
                        block.y = panel1.Height - block.height;
                        hook.y -= arg;
                    }
                    
                }
                blocks[hook.hanging] = block;


            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(button5.Text =="Hang")
            {
                hook.hanging = collision_hook_blocks();
                if (hook.hanging != -1)
                {
                    if (blocks[hook.hanging].below)
                    {

                    }
                    else
                    {
                        button5.Text = "Put on";

                    }
                }
            }
            else
            {

            }

        }
    }
}