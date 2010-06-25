using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace PICLib
{
    public partial class Form1 : Form
    {
        private Pic Trans = new Pic();
        private UseGDIPlus GDIProcess = new UseGDIPlus();
        public Form1()
        {
            InitializeComponent();
        }

        private void AdaptePictureSize(PictureBox Pb)
        {
            if (Pb.Image.Height > 240 || Pb.Image.Width > 320)
                Pb.SizeMode = PictureBoxSizeMode.Zoom;
            else
                Pb.SizeMode = PictureBoxSizeMode.Normal;
        }


        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        private void OpenFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "JPEG文件 (*.JPG,*.JPEG)|*.JPG;*.JPEG|位图文件(*.BMP)|*.BMP|All files (*.*)|*.*";
            op.ShowDialog();
            if (op.FileName!="")
            {
                this.pictureBox1.Image = Bitmap.FromFile(op.FileName);
                AdaptePictureSize(pictureBox1);
                Update();

                Trans.SetPic((Bitmap)pictureBox1.Image.Clone());
                GDIProcess.SetPic((Bitmap)pictureBox1.Image.Clone());
                SpliteRGB();
            }
            
        }


        private void 灰度图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.Gray();
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            label2.Text = "灰度化";
            return;
        }


        private void 反色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.Invert();
                AdaptePictureSize(pictureBox2);
                Update();

            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            label2.Text = "取反色";
        }
        
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image = pictureBox3.Image = pictureBox4.Image = pictureBox5.Image = pictureBox6.Image = null;
        }


        private void 亮度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.Brightness(50);
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            label2.Text = "亮度增强";
            return;
        }

        private void 提取a通道ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                pictureBox2.Image = Trans.ChangeA(100);
                AdaptePictureSize(pictureBox2); 
                Update();            
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
            label2.Text = "a通道修改";

            return;
        }

        private void 取对数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.UseLog(46);
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            label2.Text = "取对数运算";
        }


        private void 图像平滑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox3.Image = GDIProcess.SmoothPic();
                AdaptePictureSize(pictureBox3);
                Update();
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            label3.Text = "图像平滑";
            return;
        }


        private void 图像锐化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox3.Image = GDIProcess.PicSharp(0.55);
                AdaptePictureSize(pictureBox3);
                Update();
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            label3.Text = "图像锐化";
            return;
        }


        private void 矩阵乘法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] rectange = { 
                                  { 1, 3, 1 },
                                  { 3, 9, 3 }, 
                                  { 1, 3, 1 } 
                                };;
            if (rectange != null)
            {
                pictureBox3.Image = GDIProcess.RectangeChange(rectange);
                AdaptePictureSize(pictureBox3);
                Update();
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            //textBox3.Text = "卷积运算";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double[,] Value = new double[3, 3];
            Value[0, 0] = double.Parse(textBox1.Text.Trim());
            Value[0, 1] = double.Parse(textBox2.Text.Trim());
            Value[0, 2] = double.Parse(textBox3.Text.Trim());

            Value[1, 0] = double.Parse(textBox9.Text.Trim());
            Value[1, 1] = double.Parse(textBox8.Text.Trim());
            Value[1, 2] = double.Parse(textBox7.Text.Trim());

            Value[2, 0] = double.Parse(textBox6.Text.Trim());
            Value[2, 1] = double.Parse(textBox5.Text.Trim());
            Value[2, 2] = double.Parse(textBox4.Text.Trim());

            label3.Text = "3 X 3 模板卷积运算";
            if (pictureBox1.Image != null)
            {
                pictureBox3.Image = GDIProcess.RectangeChange(Value);
                AdaptePictureSize(pictureBox3);
                Update();
                MessageBox.Show("运算完成!");
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutme = new AboutBox1();
            aboutme.ShowDialog();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                pictureBox2.Image = Trans.ChangeA((byte)trackBar1.Value);
                AdaptePictureSize(pictureBox2);
                Update();

            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            label2.Text = "a通道修改";
            return;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.Brightness(trackBar2.Value);
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            label2.Text = "亮度增强";
            return;
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.UseLog(trackBar3.Value);
                AdaptePictureSize(pictureBox2);
                Update();
            }
            else
                MessageBox.Show("原始图像不存,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            label2.Text = "取对数运算";
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox3.Image = GDIProcess.PicSharp((double)trackBar4.Value/1000);
                AdaptePictureSize(pictureBox3);
                Update();
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            label3.Text = "图像锐化";
            return;
        }


        private void ResetOldPic(object sender, EventArgs e)
        {
            if ((Bitmap)((PictureBox)sender).Image != null)
            {
                pictureBox1.Image = (Bitmap)((PictureBox)sender).Image.Clone();
                //sender.ToString();
                //e.ToString();
                Trans.SetPic((Bitmap)pictureBox1.Image);
                GDIProcess.SetPic((Bitmap)pictureBox1.Image);
                Update();
            }
            else
                MessageBox.Show("源图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        private void SpliteRGB()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox4.Image = Trans.splite(0);
                AdaptePictureSize(pictureBox4);
                Update();

                pictureBox5.Image = Trans.splite(1);
                AdaptePictureSize(pictureBox5);
                Update();

                pictureBox6.Image = Trans.splite(2);
                AdaptePictureSize(pictureBox6);
                Update();

            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double[,] Value = new double[5, 5];
            Value[0, 0] = double.Parse(t11.Text.Trim());
            Value[0, 1] = double.Parse(t12.Text.Trim());
            Value[0, 2] = double.Parse(t13.Text.Trim());
            Value[0, 3] = double.Parse(t14.Text.Trim());
            Value[0, 4] = double.Parse(t15.Text.Trim());
            
            Value[1, 0] = double.Parse(t21.Text.Trim());
            Value[1, 1] = double.Parse(t22.Text.Trim());
            Value[1, 2] = double.Parse(t23.Text.Trim());
            Value[1, 3] = double.Parse(t24.Text.Trim());
            Value[1, 4] = double.Parse(t25.Text.Trim());

            Value[2, 0] = double.Parse(t31.Text.Trim());
            Value[2, 1] = double.Parse(t32.Text.Trim());
            Value[2, 2] = double.Parse(t33.Text.Trim());
            Value[2, 3] = double.Parse(t34.Text.Trim());
            Value[2, 4] = double.Parse(t35.Text.Trim());

            Value[3, 0] = double.Parse(t41.Text.Trim());
            Value[3, 1] = double.Parse(t42.Text.Trim());
            Value[3, 2] = double.Parse(t43.Text.Trim());
            Value[3, 3] = double.Parse(t44.Text.Trim());
            Value[3, 4] = double.Parse(t45.Text.Trim());

            Value[4, 0] = double.Parse(t51.Text.Trim());
            Value[4, 1] = double.Parse(t52.Text.Trim());
            Value[4, 2] = double.Parse(t53.Text.Trim());
            Value[4, 3] = double.Parse(t54.Text.Trim());
            Value[4, 4] = double.Parse(t55.Text.Trim());


            label3.Text = "5 X 5 模板卷积运算";
            if (pictureBox1.Image != null)
            {
                pictureBox3.Image = GDIProcess.RectangeChange(Value);
                AdaptePictureSize(pictureBox3);
                Update();
                MessageBox.Show("运算完成!");
            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void 参数调整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

    }
}
