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
            public int hanging;
            public char allowed_blocks;
            public float max_weight;
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
            hook.max_weight = 200;
            textBox1.Text = hook.max_weight.ToString();
            hook.allowed_blocks = 'r';
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
                block.level = 1;
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
            label1.Text = " ";
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
            label1.Text = " ";
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
            label1.Text = " ";
            for (int i=0; i<blocks.Count; i++)
            {
                if (hook.x +hook.width >=blocks[i].x && hook.x<=blocks[i].x+blocks[i].width && hook.y + hook.height >= blocks[i].y && hook.y <= blocks[i].y + blocks[i].height)
                {
                    return i;
                }
            }
            return -1;
        }

        private int collision_block_blocks(Block block)
        {
            label1.Text = " ";
            for (int i = 0; i < blocks.Count; i++)
            {
                if (i == hook.hanging) i++;
                if (block.x + block.width > blocks[i].x && block.x < blocks[i].x + blocks[i].width && block.y + block.height > blocks[i].y && block.y < blocks[i].y + blocks[i].height)
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
                    int nr = collision_block_blocks(block);
                    if(nr!=-1 )
                    {
                        winch.x -= arg;
                        block.x -= arg;
                        hook.x -= arg;
                    }
                }
                else
                {
                    block.y += arg;
                    int nr = collision_block_blocks(block);
                    if (nr != -1)
                    {
                        block.y -= arg;
                        hook.y -= arg;
                    }
                    if (block.y+block.height>panel1.Height)
                    {
                        block.y = panel1.Height - block.height;
                        hook.y -= arg;
                    }
                    
                }
                blocks[hook.hanging] = block;


            }
        }

        private int block_below()
        {

            for(int i=0;i<blocks.Count;i++)
            {
                if (i == hook.hanging) i++;
                if(blocks[hook.hanging].y + blocks[hook.hanging].height==blocks[i].y && blocks[hook.hanging].x + blocks[hook.hanging].width > blocks[i].x && blocks[hook.hanging].x < blocks[i].x + blocks[i].width)
                return i;
            }
            return -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (button5.Text =="Hang")
            {
                hook.hanging = collision_hook_blocks();
                if (hook.hanging != -1)
                {
                    if (blocks[hook.hanging].below)
                    {
                        hook.hanging = -1;
                        label1.Text = "You can't hang this block!";
                    }
                    else if(blocks[hook.hanging].shape!=hook.allowed_blocks)
                    {
                        hook.hanging = -1;
                        label1.Text = "You can't hang blocks other than choosen!";
                    }
                    else if(blocks[hook.hanging].weight >hook.max_weight)
                    {
                        label1.Text = "To heavy! " + blocks[hook.hanging].weight.ToString();
                        hook.hanging = -1;

                    }
                    else 
                    {
                        int nr = block_below();
                        if (nr != -1)
                        {
                            Block block = blocks[nr];
                            block.below = false;
                            blocks[nr] = block;
                        }
                        button5.Text = "Put on";

                    }
                }
            }
            else
            {
                int nr = block_below();

                if (nr != -1)
                {
                    Block block = blocks[nr];
                    if(blocks[nr].shape==blocks[hook.hanging].shape)
                    {
                        if(blocks[nr].level>=3)
                        {
                            label1.Text = "You cannot put block on third block";

                        }
                        else
                        {
                            block.below = true;
                            blocks[nr] = block;
                            block = blocks[hook.hanging];
                            block.level = blocks[nr].level + 1;
                            blocks[hook.hanging] = block;
                            hook.hanging = -1;
                            button5.Text = "Hang";
                        }

                    }
                    else
                    {
                        label1.Text = "You cannot put block on blocks with different shape.";
                    }

                }
                else if(blocks[hook.hanging].y + blocks[hook.hanging].height == panel1.Height)
                {
                    hook.hanging = -1;
                    button5.Text = "Hang";
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(hook.hanging!=-1)
            {
                label1.Text = "You first have to put current block on!";
            }
            else
            {
                if (button6.Text == "Rectangles")
                {
                    button6.Text = "Circles";
                    hook.allowed_blocks = 'c';
                }
                else
                {
                    button6.Text = "Rectangles";
                    hook.allowed_blocks = 'r';
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if(hook.hanging!=-1)
            {
                label1.Text = "You first have to put current block on!";
                textBox1.Text = hook.max_weight.ToString();
            }
            else hook.max_weight = float.Parse(textBox1.Text);
        }
    }
}