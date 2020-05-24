using System;
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
        int[] konxX1 = new int[1000];
        int[] konxY1 = new int[1000];
        int[] iiX1 = new int[1000];
        int[] iiY1 = new int[1000];
        int[] konxX2 = new int[1000];
        int[] konxY2 = new int[1000];
        int[] iiX2 = new int[1000];
        int[] iiY2 = new int[1000];

        Bitmap gray;
        int[,] graynya, sumimage, thesum, mean, thresholdnya, gray2, sumimage2, output;
        int s = 35, t = 28;

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
        private int[,] rgb2gray(Bitmap obj) {
            int[,] mat = new int[obj.Width, obj.Height];

            for (int x = 0; x < obj.Width; x++)
                for (int y = 0; y < obj.Height; y++){
                    Color w = obj.GetPixel(x, y);
                    int wr = w.R;
                    int wg = w.G;
                    int wb = w.B;
                    int xg = (int)((wr + wg + wb) / 3);
                    mat[x, y] = xg;
                }

            return mat;
        }
        private Bitmap imshow(int[,] mat) {
            Bitmap obj = new Bitmap(mat.GetLength(0), mat.GetLength(1));
            for (int x = 0; x < obj.Width; x++)
                for (int y = 0; y < obj.Height; y++)
                {
                    int xg = mat[x, y];
                    Color new_c = Color.FromArgb(xg, xg, xg);
                    obj.SetPixel(x, y, new_c);
                }
            return obj;
        }
        private int[,] im2bw(int[,] mat)
        {
            int[,] matr = new int[mat.GetLength(0), mat.GetLength(1)];
            for (int x = 0; x < mat.GetLength(0); x++)
                for (int y = 0; y < mat.GetLength(1); y++) 
                    
                    if (mat[x,y] > 127)
                        matr[x, y] = 255;
                    else
                        matr[x, y] = 0;
                    
            return matr;
        }
        private int[,] ones(int x, int y) {
            int[,] miniMat = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    miniMat[i, j] = 255;
            return miniMat;
        }
        private int[,] imrode(int[,] mat, int[,] s)
        {
            int[,] re = new int[mat.GetLength(0), mat.GetLength(1)];
            int cpx = s.GetLength(0) / 2 + 1;
            int cpy = s.GetLength(1) / 2 + 1;
            
            for (int i = s.GetLength(0); i < re.GetLength(0) - s.GetLength(0); i++)
                for (int j = s.GetLength(1); j < re.GetLength(1) - s.GetLength(1); j++)
                {
                    if (mat[i, j] == 255)
                    {
                        re[i, j] = 255;
                        int x = i - cpx;
                        int y = j - cpy;

                        for (int k = 0; k < s.GetLength(0); k++)
                            for (int l = 0; l < s.GetLength(1); l++)
                            {
                                if (mat[x + k, y + l] != s[k, l])
                                {
                                    re[i, j] = 0;
                                    s.GetLength(0);
                                    break;
                                }
                            }
                    }
                }
            return re;
        }
        private int[,] imdilate(int[,] mat, int[,] s) {
            int[,] rd = new int[mat.GetLength(0), mat.GetLength(1)];
            int cpx = s.GetLength(0) / 2 + 1;
            int cpy = s.GetLength(1) / 2 + 1;
            
            for (int i = s.GetLength(0); i < rd.GetLength(0) - s.GetLength(0); i++)
                for (int j = s.GetLength(1); j < rd.GetLength(1) - s.GetLength(1); j++)
                {
                    if (mat[i, j] == 255)
                    {
                        int x = i - cpx;
                        int y = j - cpy;

                        for (int k = 0; k < s.GetLength(0); k++)
                            for (int l = 0; l < s.GetLength(1); l++)
                                rd[x + k, y + l] = s[k, l];
                    }

                }
            return rd;
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
                    dataGridView1.Rows[i].Cells[j].Value = mat1[i, j].ToString();
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
            var seriesx = new Series("Proyeksi-X Gambar1");

            for (int i = 0; i < objBitmap1.Width; i++)
            {
                konxX1[i] = 0;
                for (int j = 0; j < objBitmap1.Height; j++)
                {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konxX1[i] = konxX1[i] + mat1[i, j];
                    iiX1[i] = i + 1;
                }
            }
            seriesx.Points.DataBindXY(iiX1, konxX1);
            chart2.Series.Add(seriesx);
            
            seriesx = new Series("Proyeksi-X Gambar2");

            for (int i = 0; i < objBitmap1.Width; i++)
            {
                konxX2[i] = 0;
                for (int j = 0; j < objBitmap1.Height; j++)
                {
                    if (mat2[i, j] == 0)
                        mat2[i, j] = 1;
                    else
                        mat2[i, j] = 0;
                    konxX2[i] = konxX2[i] + mat2[i, j];
                    iiX2[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(iiX2, konxX2);
            chart4.Series.Add(seriesx);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var seriesx = new Series("Proyeksi-Y Gambar1");

            for (int i = 0; i < objBitmap1.Height; i++)
            {
                konxY1[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++)
                {
                    if (mat1[i, j] == 0)
                        mat1[i, j] = 1;
                    else
                        mat1[i, j] = 0;
                    konxY1[i] = konxY1[i] + mat1[i, j];
                    iiY1[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(iiY1, konxY1);
            chart3.Series.Add(seriesx);

            konxY2 = new int[1000];
            iiY2 = new int[1000];
            seriesx = new Series("Proyeksi-Y Gambar2");

            for (int i = 0; i < objBitmap1.Height; i++)
            {
                konxY2[i] = 0;
                for (int j = 0; j < objBitmap1.Width; j++)
                {
                    if (mat2[i, j] == 0)
                        mat2[i, j] = 1;
                    else
                        mat2[i, j] = 0;
                    konxY2[i] = konxY2[i] + mat2[i, j];
                    iiY2[i] = i + 1;
                }
            }

            seriesx.Points.DataBindXY(iiY2, konxY2);
            chart5.Series.Add(seriesx);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            float difX = 0;
            float difY = 0;
            richTextBox1.Text = "";
            for (int i = 0; i < 100; i++) {
                difX += Math.Abs(konxX1[i] - konxX2[i]);
                difY += Math.Abs(konxY1[i] - konxY2[i]);
            }
            difX /= 100;
            difY /= 100;
            richTextBox1.Text += "Proses Matching :\n";
            richTextBox1.Text += "Jarak Proyeksi X = " + difX + "\n";
            richTextBox1.Text += "Jarak Proyeksi Y = " + difY + "\n";
            richTextBox1.Text += "Jarak Total = " + (difY+difX) + "\n";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int[,] X = rgb2gray(objBitmap);
            int[,] x = im2bw(X);
            objBitmap1 = new Bitmap(imshow(x));
            pictureBox2.Image = objBitmap1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] konx = new int[1000];
            int[] ii = new int[1000];
            var seriesx = new Series("Proyeksi-Horizontal");

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
        }

        private void button23_Click(object sender, EventArgs e)
        {
            objBitmap2 = new Bitmap(objBitmap1);
            int[,] X = rgb2gray(objBitmap2);
            int[,] x = im2bw(X);
            int[,] s = ones(13, 13);
            int[,] y = imrode(x, s);
            objBitmap2 = new Bitmap(imshow(y));
            pictureBox3.Image = objBitmap2;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            objBitmap2 = new Bitmap(objBitmap1);
            int[,] X = rgb2gray(objBitmap2);
            int[,] x = im2bw(X);
            int[,] s = ones(13, 13);
            int[,] z = imdilate(x, s);
            objBitmap2 = new Bitmap(imshow(z));
            pictureBox4.Image = objBitmap2; 
        }

        private void button19_Click(object sender, EventArgs e)
        {
            objBitmap2 = new Bitmap(objBitmap1);
            int[,] X = rgb2gray(objBitmap2);
            int[,] x = im2bw(X);
            int[,] s = ones(13, 13);
            int[,] y = imrode(x, s);
            int[,] z = imdilate(y, s);
            objBitmap2 = new Bitmap(imshow(z));
            pictureBox5.Image = objBitmap2;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            objBitmap2 = new Bitmap(objBitmap1);
            int[,] X = rgb2gray(objBitmap2);
            int[,] x = im2bw(X);
            int[,] s = ones(13, 13);
            int[,] z = imdilate(x, s);
            int[,] y = imrode(z, s);
            objBitmap2 = new Bitmap(imshow(y));
            pictureBox6.Image = objBitmap2;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            objBitmap1 = new Bitmap(objBitmap);
            for(int x = 0; x < objBitmap1.Width; x++)
                for(int y = 0; y < objBitmap1.Height; y++)
                {
                    Color w = objBitmap1.GetPixel(x, y);
                    int wr = w.R; int wg = w.G; int wb = w.B;
                    int xg = (int)((wr + wg + wb) / 3);
                    if (xg > 127)
                        xg = 255;
                    else
                        xg = 0;
                    Color new_c = Color.FromArgb(xg, xg, xg);
                    objBitmap1.SetPixel(x, y, new_c);
                }
            pictureBox3.Image = objBitmap1;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            gray = new Bitmap(objBitmap);
            gray2 = new int[gray.Width, gray.Height];
            sumimage2 = new int[gray.Width, gray.Height];
            output = new int[gray.Width, gray.Height];

            graynya = new int[gray.Width + 1, gray.Height + 1];
            sumimage = new int[gray.Width + 1, gray.Height + 1];
            thesum = new int[gray.Width + 1, gray.Height + 1];
            mean = new int[gray.Width + 1, gray.Height + 1];
            thresholdnya = new int[gray.Width + 1, gray.Height + 1];
            for (int x = 0; x < gray.Width; x++)
                for (int y = 0; y < gray.Height; y++)
                {
                    Color w = gray.GetPixel(x, y);
                    int a = w.A;
                    int r = w.R;
                    int g = w.G;
                    int b = w.B;
                    int avg = ((r + g + b) / 3);
                    graynya[x + 1, y + 1] = avg;
                    sumimage[x + 1, y + 1] = avg;
                    gray2[x, y] = avg;
                    gray.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            pictureBox4.Image = gray;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int sum;
            for (int x = 0; x < gray.Width; x++)
            {
                sum = 0;
                for (int y = 0; y < gray.Height; y++)
                {
                    sum = sum + gray2[x, y];
                    if (x == 0)
                    {
                        sumimage2[x, y] = sum;
                    }
                    else
                    {

                        sumimage2[x, y] = sumimage2[x - 1, y] + sum;
                    }

                }
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            int x1, x2, y1, y2, count;
            int sum;
            for (int x = s / 2 + 1; x < gray.Width - s / 2; x++)
            {

                for (int y = s / 2 + 1; y < gray.Height - s / 2; y++)
                {
                    x1 = x - s / 2;
                    x2 = x + s / 2;
                    y1 = y - s / 2;
                    y2 = y + s / 2;
                    count = (x2 - x1) * (y2 - y1);
                    sum = sumimage2[x2, y2] - sumimage2[x2, y1 - 1] - sumimage2[x1 - 1, y2] + sumimage2[x1 - 1, y1 - 1];
                    if (gray2[x, y] * count <= (sum * (100 - t) / 100))
                    {
                        output[x, y] = 0;
                    }
                    else
                    {
                        output[x, y] = 255;
                    }

                }
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < gray.Width; x++)
                for (int y = 0; y < gray.Height; y++)
                {
                    if (x < s / 2 + 1 || x > (gray.Width - s / 2 - 1) || y < s / 2 + 1 || y > (gray.Height - s / 2 - 1))
                    {
                        gray.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        gray.SetPixel(x, y, Color.FromArgb(output[x, y], output[x, y], output[x, y]));
                    }

                }
            pictureBox4.Image = gray;
        }

    }
}
