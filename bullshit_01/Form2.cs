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
    public partial class Form2 : Form
    {
        int[,] array = new int[1024, 512];
        int sx; int sy;
        int[,] na = new int[4096, 2048];
        Random r;
        int water;
        int snow;

        public int mod(int number, int div)
        {
            while (number < 0) number += div;
            while (number >= div) number -= div;
            return number;
        }

        public int get_rand(int dep)
        {
            return r.Next(-dep / 4, dep / 4);
        }

        public Form2(int[,] arr, int startx, int starty, Random t, int water, int snow)
        {
            InitializeComponent();
            this.array = arr;
            this.sx = startx;
            this.sy = starty;
            this.r = t;
            this.water = water;
            this.snow = snow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4096; i++)
                for (int j = 0; j < 2048; j++)
                    if (i % 4 == 0 && j % 4 == 0)
                        na[i, j] = array[i / 4, j / 4];
                    else
                        na[i, j] = 0;
            int xamount = 1024;
            int yamount = 512;
            int dist = 4;
            while (true)
            {
                for (int csx = 0; csx < xamount; csx++)
                    for (int csy = 0; csy < yamount; csy++)
                    {
                        int cx = csx * dist + dist / 2;
                        int cy = csy * dist + dist / 2;
                        na[cx, cy] = (
                            na[mod(cx - dist / 2, 4096), mod(cy - dist / 2, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx - dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy - dist / 2, 2048)]) / 4 + get_rand(dist);
                    }
                for (int csx = 0; csx < xamount; csx++)
                    for (int csy = 0; csy < yamount; csy++)
                    {
                        int cx = csx * dist + dist / 2;
                        int cy = csy * dist + dist / 2;
                        na[mod(cx - dist / 2, 4096), mod(cy, 2048)] = (
                            na[mod(cx - dist / 2 - dist / 2, 4096), mod(cy, 2048)] +
                            na[mod(cx - dist / 2 + dist / 2, 4096), mod(cy, 2048)] +
                            na[mod(cx - dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx - dist / 2, 4096), mod(cy - dist / 2, 2048)]) / 4 + get_rand(dist);
                        na[mod(cx + dist / 2, 4096), mod(cy, 2048)] = (
                            na[mod(cx + dist / 2 - dist / 2, 4096), mod(cy, 2048)] +
                            na[mod(cx + dist / 2 + dist / 2, 4096), mod(cy, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy - dist / 2, 2048)]) / 4 + get_rand(dist);
                        na[mod(cx, 4096), mod(cy - dist / 2, 2048)] = (
                            na[mod(cx - dist / 2, 4096), mod(cy - dist / 2, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy - dist / 2, 2048)] +
                            na[mod(cx, 4096), mod(cy - dist / 2 + dist / 2, 2048)] +
                            na[mod(cx, 4096), mod(cy - dist / 2 - dist / 2, 2048)]) / 4 + get_rand(dist);
                        na[mod(cx, 4096), mod(cy + dist / 2, 2048)] = (
                            na[mod(cx - dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx + dist / 2, 4096), mod(cy + dist / 2, 2048)] +
                            na[mod(cx, 4096), mod(cy + dist / 2 + dist / 2, 2048)] +
                            na[mod(cx, 4096), mod(cy + dist / 2 - dist / 2, 2048)]) / 4 + get_rand(dist);
                    }
                if (dist == 1) break;
                xamount *= 2;
                yamount *= 2;
                dist /= 2;
            }
            int min = Int32.MaxValue;
            for (int i = 0; i < 4096; i++)
                for (int j = 0; j < 2048; j++)
                    if (na[i, j] < min)
                        min = na[i, j];
            for (int i = 0; i < 4096; i++)
                for (int j = 0; j < 2048; j++)
                    na[i, j] -= min;
            MessageBox.Show("DONE");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            for (int i = 0; i < 512; i++)
                for (int j = 0; j < 512; j++)
                {
                    Color c = new Color();
                    if (na[i + sx * 4, j + sy * 4] <= water)
                        c = Color.FromArgb(0, 0, 255 - water + na[i + sx * 4, j + sy * 4]);
                    else if (na[i + sx * 4, j + sy * 4] == water + 1)
                        c = Color.SandyBrown;
                    else if (na[i + sx * 4, j + sy * 4] >= water + 2 && na[i + sx * 4, j + sy * 4] <= snow) c = Color.FromArgb((int)(((float)(na[i + sx * 4, j + sy * 4] - water + 2) / (float)(snow - water + 2)) * (float)255), 180, 0);
                    else c = Color.FromArgb(Math.Min(240 + na[i + sx * 4, j + sy * 4] / 5, 255), Math.Min(230 + na[i + sx * 4, j + sy * 4] / 5, 255), 255);
                    SolidBrush b = new SolidBrush(c);
                    g.FillRectangle(b, i, j, 1, 1);
                }
        }
    }
}
