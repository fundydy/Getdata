using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Getdata
{
    public partial class Form1 : Form
    {       
        private Point startPt, endPt,dataPt,linep1,linep2;
        private int step = 0;
        private float wzoom;
        private float hzoom;
        private double sx,sy,ex,ey,px1,py1,px2,py2,lengthp;
        private Pen pen1 = new Pen(Color.Red, 3);
        private Pen pen2 = new Pen(Color.BlueViolet, 2);
        private Graphics g;
        public Form1()
        {
            InitializeComponent();
            checklength.Enabled = false;
        }

        private Bitmap img;
        private Bitmap img1;
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string imgname = openFileDialog1.FileName;
            img = new Bitmap(imgname,true);
            wzoom = (float)pictureBox1.Width/(float)img.Width;
            hzoom = (float)pictureBox1.Height / (float)img.Height;
            img1 = ZoomP(img, wzoom, hzoom);
            pictureBox1.Image = img1;
            toolStripStatusLabel1.Text = "图片打开成功";
        }

        private static Bitmap ZoomP(Bitmap a, float s, float v)
        {
            Bitmap bmp = new Bitmap((int)(a.Width * s), (int)(a.Height * v), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.ScaleTransform(s, v);
            g.DrawImage(a, 0, 0, a.Width, a.Height);
            a = bmp;
            return a;
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 转数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 getdata=new Form2();
            if (getdata.ShowDialog() == DialogResult.OK)
            {
                sx = getdata.sx;
                sy = getdata.sy;
                ex = getdata.ex;
                ey = getdata.ey;
            }
            toolStripStatusLabel1.Text = "请点击获取坐标轴左上角点";
            pictureBox1.Cursor=Cursors.Cross;
            step = 1;

        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void checklength_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checklength_CheckStateChanged(object sender, EventArgs e)
        {
            if (checklength.CheckState == CheckState.Checked)
            {
                step = 4;
            }
            else
            {
                step = 3;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel2.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        private void pictureBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void 数据另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "逗号分隔文件|*.csv|文本文件|*.txt";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string localFilePath = saveFileDialog1.FileName.ToString(); //获得文件路径 
                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径
                StreamWriter sw = new StreamWriter(localFilePath, true);
                sw.WriteLine("x,y");
                for (int i = 0; i < dataGridView1.RowCount-1; i++)
                {
                    sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value+","+ dataGridView1.Rows[i].Cells[1].Value + "," + dataGridView1.Rows[i].Cells[2].Value);
                }
                sw.Close();
                toolStripStatusLabel1.Text = "数据已输出到:" + localFilePath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = pictureBox1.CreateGraphics();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (step == 3 && e.KeyCode == Keys.Enter)
            {
                toolStripStatusLabel1.Text = "数据获取完成";
                step = 0;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {            
            switch (step)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        startPt = e.Location;
                        toolStripStatusLabel1.Text = "请点击获取坐标轴右下角点";
                        step = 2;
                        break;
                    }
                case 2:
                    {
                        endPt = e.Location;
                        Rectangle rect = new Rectangle(startPt.X, startPt.Y, endPt.X - startPt.X, endPt.Y - startPt.Y);
                        g.DrawRectangle(pen1, rect);
                        toolStripStatusLabel1.Text = "请依次点击获取数据点，回车键结束获取";
                        checklength.Enabled = true;
                        step = 3;
                        break;
                    }
                case 3:
                    {
                        dataPt = e.Location;
                        Rectangle rect = new Rectangle(dataPt.X-3, dataPt.Y-3, 6, 6);
                        g.DrawEllipse(pen1, rect);
                        double dy = ey-((double) (endPt.Y - dataPt.Y))/(endPt.Y - startPt.Y)*(ey - sy);
                        double dx = sx+((double)(dataPt.X-startPt.X)) / (endPt.X - startPt.X) * (ex - sx);
                        dataGridView1.Rows.Add(dx, dy,"","");
                        break;
                    }
                case 4:
                    {
                        dataPt = e.Location;
                        Rectangle rect = new Rectangle(dataPt.X - 3, dataPt.Y - 3, 6, 6);
                        g.DrawEllipse(pen1, rect);
                        double dy = ey - ((double)(endPt.Y - dataPt.Y)) / (endPt.Y - startPt.Y) * (ey - sy);
                        double dx = sx + ((double)(dataPt.X - startPt.X)) / (endPt.X - startPt.X) * (ex - sx);
                        linep1 = dataPt;
                        px1 = dx;
                        py1 = dy;
                        step = 5;
                        break;
                    }
                case 5:
                    {
                        dataPt = e.Location;
                        linep2 = dataPt;
                        Rectangle rect = new Rectangle(dataPt.X - 3, dataPt.Y - 3, 6, 6);
                        g.DrawEllipse(pen1, rect);
                        double dy = ey - ((double)(endPt.Y - dataPt.Y)) / (endPt.Y - startPt.Y) * (ey - sy);
                        double dx = sx + ((double)(dataPt.X - startPt.X)) / (endPt.X - startPt.X) * (ex - sx);
                        px2 = dx;
                        py2 = dy;
                        lengthp = Math.Sqrt(Math.Pow((px2 - px1), 2) + Math.Pow((py2 - py1), 2));
                        g.DrawLine(pen2,linep1,linep2);
                        dataGridView1.Rows.Add("", "", lengthp, "");
                        step = 4;
                        break;
                    }
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (img != null&& this.WindowState != FormWindowState.Minimized)
            {
                g = pictureBox1.CreateGraphics();
                wzoom = (float)pictureBox1.Width / (float)img.Width;
            hzoom = (float)pictureBox1.Height / (float)img.Height;
            img1 = ZoomP(img, wzoom, hzoom);
            pictureBox1.Image = img1;
            }           
        }
    }
}
