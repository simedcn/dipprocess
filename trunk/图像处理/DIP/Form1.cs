using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DIP
{
    public partial class Form1 : Form
    {
        PICLib.Pic Trans;
        PICLib.UseGDIPlus GDIProcess;
        public Form1()
        {
            InitializeComponent();
            Trans = new PICLib.Pic();
            GDIProcess = new PICLib.UseGDIPlus();
        }

        private void AdaptePictureSize(PictureBox Pb)
        {
            if (Pb.Image.Height > Pb.Height || Pb.Image.Width > Pb.Width)
                Pb.SizeMode = PictureBoxSizeMode.Zoom;
            else
                Pb.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void SpliteRGB()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox2.Image = Trans.splite(0);
                AdaptePictureSize(pictureBox2);
                Update();

                pictureBox3.Image = Trans.splite(1);
                AdaptePictureSize(pictureBox3);
                Update();

                pictureBox4.Image = Trans.splite(2);
                AdaptePictureSize(pictureBox4);
                Update();

            }
            else
                MessageBox.Show("原始图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "JPEG文件 (*.JPG,*.JPEG)|*.JPG;*.JPEG|位图文件(*.BMP)|*.BMP|All files (*.*)|*.*";
            op.ShowDialog();
            if (op.FileName != "")
            {
                this.pictureBox1.Image = Bitmap.FromFile(op.FileName);
                AdaptePictureSize(pictureBox1);
                Update();

                Trans.SetPic((Bitmap)pictureBox1.Image.Clone());
                GDIProcess.SetPic((Bitmap)pictureBox1.Image.Clone());
                SpliteRGB();
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if ((Bitmap)((PictureBox)sender).Image != null)
            {
                if (!splitContainer1.Panel2.Controls.Contains(pictureBox5))
                {
                    splitContainer1.Panel2.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(pictureBox5);
                    splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
                }
                pictureBox5.Image = (Bitmap)((PictureBox)sender).Image.Clone();
                
                //Trans.SetPic((Bitmap)pictureBox4.Image);
                //GDIProcess.SetPic((Bitmap)pictureBox4.Image);
                AdaptePictureSize(pictureBox5);
                Update();
            }
            else
                MessageBox.Show("源图像不存在,请加载原图后再进行此项操作!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        private void 打开面板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = false;
        }

        private void 白平衡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox5.Image != null)
            {
                pictureBox5.Image= Trans.WhiteBalance();
            }
        }

        private void 直方图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.pictureBox5.Image = Trans.ZhiFangtu(3);
            //AdaptePictureSize(pictureBox5);

            ////pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            //Update();
            int[,] result=Trans.ZhiFangtu2();
            //result.CopyTo(re, 0);
            GraphicBar br=new GraphicBar (Color.Black,Color.White,result,3);
            splitContainer1.Panel2.Controls.Clear();
            br.Dock = DockStyle.Top;
            splitContainer1.Panel2.Controls.Add(br);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
            }
            else if(comboBox1.SelectedIndex==0)
            {
                if (pictureBox1.Image != null)
                {
                    //pictureBox2.Image = Trans.ZhiFangtu(0);
                    //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    ////AdaptePictureSize(pictureBox2);
                    //Update();
                    //pictureBox3.Image = Trans.ZhiFangtu(1);
                    //AdaptePictureSize(pictureBox3);
                    //Update();
                    //pictureBox4.Image = Trans.ZhiFangtu(2);
                    //AdaptePictureSize(pictureBox4);
                    //Update();
                    int[,] result = Trans.ZhiFangtu2();
                    //result.CopyTo(re, 0);
                    GraphicBar br = new GraphicBar(Color.Blue, Color.White, result, 0);
                    GraphicBar gr = new GraphicBar(Color.Green, Color.White, result, 1);
                    GraphicBar rr = new GraphicBar(Color.Red, Color.White, result, 2);

                    this.panel1.Controls.Clear();

                    br.Dock = DockStyle.Top;
                    gr.Dock = DockStyle.Top;
                    rr.Dock = DockStyle.Top;
                    //br.SetPic(Trans.splite(0));

                    this.panel1.Controls.Add(br);
                    panel1.Controls.Add(gr);
                    panel1.Controls.Add(rr);

                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                //panel1.Invalidate();
                //SpliteRGB();

                if (pictureBox1.Image != null)
                {
                //    panel1.Controls.Clear();
                //    GraphicBar br = new GraphicBar((Bitmap)Trans.splite(0));
                //    br.Dock = DockStyle.Top;
                //    panel1.Controls.Add(br);

                //    GraphicBar gr = new GraphicBar((Bitmap)Trans.splite(1));
                //    gr.Dock = DockStyle.Top;
                //    panel1.Controls.Add(gr);

                //    GraphicBar rr = new GraphicBar((Bitmap)Trans.splite(2));
                //    rr.Dock = DockStyle.Top;
                //    panel1.Controls.Add(rr);
                    //Update();
                    panel1.Controls.Clear();
                    panel1.Controls.Add(pictureBox2);
                    panel1.Controls.Add(pictureBox3);
                    panel1.Controls.Add(pictureBox4);
                    panel1.Controls.Add(label2);
                    panel1.Controls.Add(label3);
                    panel1.Controls.Add(label4);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox5.Image;
            Trans.SetPic((Bitmap)pictureBox5.Image);
            GDIProcess.SetPic((Bitmap)pictureBox5.Image);
        }

        private void 彩色补偿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox5.Image != null)
                pictureBox5.Image = Trans.ColorCompensate();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBoxMe aboutme = new AboutBoxMe();
            aboutme.ShowDialog();
        }
    }
}
