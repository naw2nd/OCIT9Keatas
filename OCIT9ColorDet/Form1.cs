﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace OCIT9ColorDet
{
    public partial class Form1 : Form
    {
        Bitmap objBitmap;
        Bitmap objBitmap1;
        Bitmap objBitmap2;
        Bitmap objBitmap3;
        float[][] h = new float[4][];
        int[,] mat1 = new int[1000, 1000];
        int[,] mat2 = new int[1000, 1000];
        int row = 0, col = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void histogramR_G_B()
        {
            float[] hr = new float[256];
            float[] hg = new float[256];
            float[] hb = new float[256];

            chart1.Series["Series1"].Points.Clear();
            chart2.Series["Series1"].Points.Clear();
            chart3.Series["Series1"].Points.Clear();

            for (int i = 0; i < 256; i++) {
                hr[i] = 0; hg[i] = 0; hb[i] = 0;
            } 
            for (int x = 0; x < objBitmap.Width; x++)
                for (int y = 0; y < objBitmap.Height; y++)
                {
                    Color w = objBitmap.GetPixel(x, y);
                    int xr = w.R; int xg = w.G; int xb = w.B;
                    hr[xr]++; hg[xg]++; hb[xb]++;
                }
            for (int i = 0; i < 256; i++) {
                chart1.Series["Series1"].Points.AddXY(i, hr[i]);
                chart2.Series["Series1"].Points.AddXY(i, hg[i]);
                chart3.Series["Series1"].Points.AddXY(i, hb[i]);
            }
                    
        }
        private float[] histogramRGB(Bitmap obj, int no)
        {
            float[] h = new float[384];

            switch (no)
            {
                case 1:
                    chart1.Series["Series1"].Points.Clear();
                    break;
                case 2:
                    chart2.Series["Series1"].Points.Clear();
                    break;
                case 3:
                    chart3.Series["Series1"].Points.Clear();
                    break;
                case 5:
                    chart5.Series["Series1"].Points.Clear();
                    break;
            }

            for (int i = 0; i < 384; i++)
            {
                h[i] = 0;
            }
            for (int x = 0; x < obj.Width; x++)
                for (int y = 0; y < obj.Height; y++)
                {
                    Color w = obj.GetPixel(x, y);
                    int xr = w.R / 2;
                    h[xr]++;
                    int xg = (w.G / 2) + 128;
                    h[xg]++;
                    int xb = (w.B / 2) + 256;
                    h[xb]++;
                }
            for (int i = 0; i < 384; i++)
            {
                switch (no)
                {
                    case 1:
                        chart1.Series["Series1"].Points.AddXY(i, h[i]);
                        break;
                    case 2:
                        chart2.Series["Series1"].Points.AddXY(i, h[i]);
                        break;
                    case 3:
                        chart3.Series["Series1"].Points.AddXY(i, h[i]);
                        break;
                    case 5:
                        chart5.Series["Series1"].Points.AddXY(i, h[i]);
                        break;
                }
            }
            return h;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = objBitmap;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            objBitmap1 = new Bitmap(objBitmap);
            for (int x = 0; x < objBitmap1.Width; x++)
                for (int y = 0; y < objBitmap1.Height; y++)
                {
                    Color w = objBitmap.GetPixel(x, y);
                    int r = w.R;
                    int g = w.G;
                    int b = w.B;
                    Color wb;
                    if (r > 102 && r < 160 && g > 70 && g < 100 && b > 0 && b > 65)
                        wb = Color.FromArgb(r, g, b);
                    else
                        wb = Color.FromArgb(0, 0, 0);
                    objBitmap1.SetPixel(x, y, wb);
                }
            pictureBox2.Image = objBitmap1;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            objBitmap1 = new Bitmap(objBitmap);
            for (int x = 0; x < objBitmap1.Width; x++)
                for (int y = 0; y < objBitmap1.Height; y++)
                {
                    Color w = objBitmap.GetPixel(x, y);
                    int r = w.R;
                    int g = w.G;
                    int b = w.B;
                    int d = (int)(r - 144 + g - 89 + b - 65);
                    Color wb;
                    if (d > 85)
                        wb = Color.FromArgb(r, g, b);
                    else
                        wb = Color.FromArgb(0, 0, 0);
                    objBitmap1.SetPixel(x, y, wb);
                }
            pictureBox3.Image = objBitmap1;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int nPallet = 6;
            Bitmap mini = new Bitmap(objBitmap, new Size(nPallet, nPallet));
            Bitmap pallet = new Bitmap(objBitmap);
            int devider = objBitmap.Height / nPallet;
            for (int i = 0; i < 6; i++)
            {
                for (int y = i * devider; y < (i + 1) * devider; y++)
                {
                    for (int x = 0; x < objBitmap.Width; x++)
                    {
                        pallet.SetPixel(x, y, mini.GetPixel(i,i));
                    }
                }
           }
            palletBox.Image = pallet;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            histogramR_G_B();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //load img1
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap = new Bitmap(new Bitmap(openFileDialog1.FileName), new Size(50, 30));
                pictureBox1.Image = objBitmap;
                h[0]=histogramRGB(objBitmap,1);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Load IMG2
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap1 = new Bitmap(new Bitmap(openFileDialog1.FileName), new Size(50, 30));
                pictureBox2.Image = objBitmap1;
                h[1]=histogramRGB(objBitmap1,2);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //load img3
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap2 = new Bitmap(new Bitmap(openFileDialog1.FileName), new Size(50, 30));
                pictureBox3.Image = objBitmap2;
                h[2]=histogramRGB(objBitmap2, 3);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //interseksi
            float[] g = new float[384];
            chart4.Series["Series1"].Points.Clear();
            for (int i = 0; i < 384; i++)
            {
                g[i] = h[0][i];
                if (h[1][i] < g[i])
                    g[i] = h[1][i];
                if(h[2][i] < g[i])
                    g[i] = h[2][i];
                chart4.Series["Series1"].Points.AddXY(i, g[i]);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //load img4
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap3 = new Bitmap(new Bitmap(openFileDialog1.FileName), new Size(50, 30));
                pictureBox4.Image = objBitmap3;
                h[3]=histogramRGB(objBitmap3, 5);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //matching
            float[] d = new float[3];
            float di = 0;
            richTextBox1.Text = "";
            for (int i = 0; i < 3; i++)
            {
                d[i] = 0;
                for(int j = 0; j < 384; j++)
                {
                    di += Math.Abs(h[3][j] - h[i][j]);
                }
                di /= 384;
                d[i] = di;
                richTextBox1.Text +=(i+" "+d[i]+"\n");
            }
            if (d[0] < d[1] && d[0] < d[2])
                richTextBox1.Text += "HIJAU";
            else if (d[1] < d[0] && d[1] < d[2])
                richTextBox1.Text += "MERAH";
            else
                richTextBox1.Text += "CAMPURAN";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            objBitmap1 = new Bitmap(objBitmap);
            for (int x = 0; x < objBitmap1.Width; x++)
                for (int y = 0; y < objBitmap1.Height; y++) {
                    Color w = objBitmap1.GetPixel(x, y);
                    int wr = w.R; int wg = w.G; int wb = w.B;
                    int xg = (int)((wr + wg + wb) / 3);
                    if (xg > 127)
                        xg = 255;
                    else
                        xg = 0;
                    mat1[x, y] = (int)xg / 255;
                    Color new_c = Color.FromArgb(xg, xg, xg);
                    objBitmap1.SetPixel(x, y, new_c);
                }
            pictureBox2.Image = objBitmap1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objBitmap1.Width; i++) {
                DataGridViewColumn newCol = new DataGridViewColumn();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                newCol.CellTemplate = cell;
                newCol.HeaderText = (i + 1).ToString();
                newCol.Name = "Oke";
                newCol.Visible = true;
                newCol.Width = 25;
                dataGridView1.Columns.Add(newCol);
                dataGridView1.Rows.Add(cell);
            }
            dataGridView1.Rows.Add(objBitmap1.Height);
            for (int i = 0; i < objBitmap1.Height; i++) {
                for (int j = 0; j < objBitmap1.Width; j++) {
                    dataGridView1.Rows[i].Cells[j].Value = mat1[j, i].ToString();
                }
            }
        }

        private void button15_Click(object sender, EventArgs e){
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            int[] kony = new int[1000];
            int[] jj = new int[1000];
            var series1 = new Series("Proyeksi X");
            var series2 = new Series("Proyeksi Y");

            for (int i = 0; i < objBitmap1.Height; i++){
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++){
                    konx[i] = konx[i] + mat1[i, j];
                    ii[i] = i + 1;
                }
                dataGridView2.Rows.Add();
                dataGridView2["Column1", i].Value = konx[i].ToString();
            }

            for (int j = 0; j < objBitmap1.Width; j++){
                kony[j] = 0;
                for (int i = 0; i < objBitmap1.Height; i++) {
                    kony[j] = kony[j] + mat1[i, j];
                    jj[j] = j + 1;
                }
                dataGridView2.Rows.Add();
                dataGridView2["Column2", j].Value = kony[j].ToString();
            }

            series1.Points.DataBindXY(ii, konx);
            chart1.Series.Add(series1);
            series2.Points.DataBindXY(ii, kony);
            chart1.Series.Add(series2);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            var seriesx = new Series("Proyeksi-Vertikal");

            for (int i = 0; i < objBitmap1.Height; i++)
            {
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++)
                {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konx[i] = konx[i] + mat1[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart3.Series.Add(seriesx);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                objBitmap = new Bitmap(objBitmap, new Size(100, 100));
                for (int x = 0; x < objBitmap.Width; x++)
                    for (int y = 0; y < objBitmap.Height; y++)
                    {
                        Color w = objBitmap.GetPixel(x, y);
                        int wr = w.R; int wg = w.G; int wb = w.B;
                        int xg = (int)((wr + wg + wb) / 3);
                        if (xg > 127)
                            xg = 255;
                        else
                            xg = 0;
                        mat1[x, y] = (int)xg / 255;
                        Color new_c = Color.FromArgb(xg, xg, xg);
                        objBitmap.SetPixel(x, y, new_c);
                    }
                pictureBox1.Image = objBitmap;

                objBitmap1 = new Bitmap(new Bitmap(openFileDialog1.FileName), new Size(100, 100));
                for (int x = 0; x < objBitmap1.Width; x++)
                    for (int y = 0; y < objBitmap.Height; y++)
                    {
                        Color w = objBitmap1.GetPixel(x, y);
                        int wr = w.R; int wg = w.G; int wb = w.B;
                        int xg = (int)((wr + wg + wb) / 3);
                        if (xg > 127)
                            xg = 255;
                        else
                            xg = 0;
                        mat2[x, y] = (int)xg / 255;
                        Color new_c = Color.FromArgb(xg, xg, xg);
                        objBitmap1.SetPixel(x, y, new_c);
                    }
                pictureBox2.Image = objBitmap1;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            var seriesx = new Series("Proyeksi-X Gambar1");

            for (int i = 0; i < objBitmap1.Width; i++)
            {
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Height; j++)
                {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konx[i] = konx[i] + mat1[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart2.Series.Add(seriesx);

            konx = new int[1000];
            ii = new int[1000];
            seriesx = new Series("Proyeksi-X Gambar2");

            for (int i = 0; i < objBitmap1.Width; i++)
            {
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Height; j++)
                {
                    if (mat2[i, j] == 0)
                        mat2[i, j] = 1;
                    else
                        mat2[i, j] = 0;
                    konx[i] = konx[i] + mat2[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart4.Series.Add(seriesx);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            var seriesx = new Series("Proyeksi-Y Gambar1");

            for (int i = 0; i < objBitmap1.Height; i++)
            {
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++)
                {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konx[i] = konx[i] + mat1[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart3.Series.Add(seriesx);

            konx = new int[1000];
            ii = new int[1000];
            seriesx = new Series("Proyeksi-Y Gambar2");

            for (int i = 0; i < objBitmap1.Height; i++)
            {
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++)
                {
                    if (mat2[i, j] == 0)
                        mat2[i, j] = 1;
                    else
                        mat2[i, j] = 0;
                    konx[i] = konx[i] + mat2[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart5.Series.Add(seriesx);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            float dif = 0;
            richTextBox1.Text = "";
            for (int i = 0; i < 100; i++) {
                for(int j = 0; j < 100; j++){
                    if (mat1[i, j] != mat2[i, j])
                        dif++;
                }
            }
            dif /= 100;
            dif = 100 - dif;
            richTextBox1.Text += "Kecocokan :\n"+dif+"%";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            var seriesx = new Series("Proyeksi-Horizontal");           

            for (int i = 0; i < objBitmap1.Width; i++){
                konx[i] = 0;
                for (int j = 0; j < objBitmap1.Height; j++) {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konx[i] = konx[i] + mat1[i, j];
                    ii[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(ii, konx);
            chart2.Series.Add(seriesx);
        }
    }
}