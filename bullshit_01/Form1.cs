using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bullshit_01
{
    public partial class Form1 : Form
    {
        int[,] array = new int[1024, 512];
        Random r;

        public Form1()
        {
            InitializeComponent();
        }

        public int mod(int number, int div)
        {
            while (number < 0) number += div;
            while (number >= div) number -= div;
            return number;
        }

        public int get_rand(int dep, Random r)
        {
            return r.Next(-dep / 4, dep / 4);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            r = new Random(Convert.ToInt32(textBox1.Text));
            for (int i = 0; i < 1024; i++)
                for (int j = 0; j < 512; j++)
                    array[i, j] = 0;
            int xamount = 2;
            int yamount = 1;
            int dist = 512;
            while (true)
            {
                for(int csx = 0; csx < xamount; csx++)
                    for (int csy = 0; csy < yamount; csy++)
                    {
                        int cx = csx * dist + dist / 2;
                        int cy = csy * dist + dist / 2;
                        array[cx, cy] = (
                            array[mod(cx - dist / 2, 1024), mod(cy - dist / 2, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx - dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy - dist / 2, 512)]) / 4 + get_rand(dist, r);
                    }
                for (int csx = 0; csx < xamount; csx++)
                    for (int csy = 0; csy < yamount; csy++)
                    {
                        int cx = csx * dist + dist / 2;
                        int cy = csy * dist + dist / 2;
                        array[mod(cx - dist / 2, 1024), mod(cy, 512)] = (
                            array[mod(cx - dist / 2 - dist / 2, 1024), mod(cy, 512)] +
                            array[mod(cx - dist / 2 + dist / 2, 1024), mod(cy, 512)] +
                            array[mod(cx - dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx - dist / 2, 1024), mod(cy - dist / 2, 512)]) / 4 + get_rand(dist, r);
                        array[mod(cx + dist / 2, 1024), mod(cy, 512)] = (
                            array[mod(cx + dist / 2 - dist / 2, 1024), mod(cy, 512)] +
                            array[mod(cx + dist / 2 + dist / 2, 1024), mod(cy, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy - dist / 2, 512)]) / 4 + get_rand(dist, r);
                        array[mod(cx, 1024), mod(cy - dist / 2, 512)] = (
                            array[mod(cx - dist / 2, 1024), mod(cy - dist / 2, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy - dist / 2, 512)] +
                            array[mod(cx, 1024), mod(cy - dist / 2 + dist / 2, 512)] +
                            array[mod(cx, 1024), mod(cy - dist / 2 - dist / 2, 512)]) / 4 + get_rand(dist, r);
                        array[mod(cx, 1024), mod(cy + dist / 2, 512)] = (
                            array[mod(cx - dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx + dist / 2, 1024), mod(cy + dist / 2, 512)] +
                            array[mod(cx, 1024), mod(cy + dist / 2 + dist / 2, 512)] +
                            array[mod(cx, 1024), mod(cy + dist / 2 - dist / 2, 512)]) / 4 + get_rand(dist, r);
                    }
                if (dist == 1) break;
                xamount *= 2;
                yamount *= 2;
                dist /= 2;
            }
            int min = Int32.MaxValue;
            for (int i = 0; i < 1024; i++)
                for (int j = 0; j < 512; j++)
                    if (array[i, j] < min)
                        min = array[i, j];
            for (int i = 0; i < 1024; i++)
                for (int j = 0; j < 512; j++)
                    array[i, j] -= min;
            MessageBox.Show("DONE");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            int water = Convert.ToInt32(textBox2.Text);
            int snow = Convert.ToInt32(textBox3.Text);
            int offsetx = -Convert.ToInt32(textBox4.Text);
            int offsety = -Convert.ToInt32(textBox5.Text);
            for (int i = 0; i < 1024; i++)
                for (int j = 0; j < 512; j++)
                {
                    Color c = new Color();
                    if (array[mod(i + offsetx, 1024), mod(j + offsety, 512)] <= water)
                        c = Color.FromArgb(0, 0, 255 - water + array[mod(i + offsetx, 1024), mod(j + offsety, 512)]);
                    else if (array[mod(i + offsetx, 1024), mod(j + offsety, 512)] == water + 1)
                        c = Color.SandyBrown;
                    else if (array[mod(i + offsetx, 1024), mod(j + offsety, 512)] >= water + 2 && array[mod(i + offsetx, 1024), mod(j + offsety, 512)] <= snow) c = Color.FromArgb((int)(((float)(array[mod(i + offsetx, 1024), mod(j + offsety, 512)] - water + 2) / (float)(snow - water + 2)) * (float)255), 180, 0);
                    else c = Color.FromArgb(Math.Min(240 + array[mod(i + offsetx, 1024), mod(j + offsety, 512)] / 5, 255), Math.Min(230 + array[mod(i + offsetx, 1024), mod(j + offsety, 512)]/5, 255), 255);
                    if (checkBox1.Checked)
                        if (array[mod(i + offsetx, 1024), mod(j + offsety, 512)] % 10 == 0)
                            c = Color.Black;
                    SolidBrush b = new SolidBrush(c);
                    g.FillRectangle(b,i,j,1,1);
                }
            if (checkBox2.Checked)
                g.DrawRectangle(new Pen(Color.Red,1), Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox7.Text), 128, 128);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(array, Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox7.Text), r, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
            f.Show();
        }
    }
}
