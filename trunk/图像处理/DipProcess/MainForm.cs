using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DipProcess
{
    public partial class MainForm : Form
    {
        private Bitmap PicMap,TempBitMap ;
        private PICLib.ColorImg ColorImg, TempColorImg;
        private PICLib.GrayImg GrayImg, TempGrayImg;
        /// <summary>
        /// 实例化窗体
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            ////加载测试图像至右侧区域
            //Bitmap temp = new Bitmap(@"X:\图片\照片\12-03-07_1744.jpg");
            //LoadPicOnRight(temp, @"测试一");
            //temp = new Bitmap(@"X:\图片\照片\12-03-07_1744.jpg");
            //LoadPicOnRight(temp, @"测试二");
            //temp = new Bitmap(@"X:\图片\照片\12-03-07_1744.jpg");
            //LoadPicOnRight(temp, @"测试二");

            ////加载测试图像至右侧区域
            //temp = new Bitmap(@"X:\图片\照片\12-03-07_1744.jpg");
            //LoadPicOnLeft(temp, @"测试二");
            //temp = new Bitmap(@"X:\图片\照片\12-03-07_1744.jpg");
            //LoadPicOnLeft(temp, @"测试二");
        }

        #region 加载图像至指定区域
        #region 加载至右侧区域

        /// <summary>
        /// 加载图像至右侧区域(无底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        private void LoadPicOnRight(Bitmap Pic)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            //显示图像
            Temp.pictureBox1.Image = Pic;

            ////底部文字说明
            //Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至右侧区域(有底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        /// <param name="txt">说明文字</param>
        private void LoadPicOnRight(Bitmap Pic,string txt)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            //显示图像
            Temp.pictureBox1.Image = Pic;

            //底部文字说明
            Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至右侧区域(无底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        private void LoadPicOnRight(PICLib.GrayImg Pic)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);

            ////显示图像
            //Temp.pictureBox1.Image = tmp;

            ////底部文字说明
            //Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至右侧区域(有底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        /// <param name="txt">说明文字</param>
        private void LoadPicOnRight(PICLib.GrayImg Pic, string txt)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);

            //显示图像
            Temp.pictureBox1.Image = tmp;

            //底部文字说明
            Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至右侧区域(无底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        private void LoadPicOnRight(PICLib.ColorImg Pic)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);

            ////显示图像
            //Temp.pictureBox1.Image = tmp;

            ////底部文字说明
            //Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至右侧区域(有底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        /// <param name="txt">说明文字</param>
        private void LoadPicOnRight(PICLib.ColorImg Pic, string txt)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 320 && Pic.Height < 240) || (Pic.Width < 240 && Pic.Height < 320))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 320;
                Temp.Height = 252;

                Temp.pictureBox1.Width = 320;
                Temp.pictureBox1.Height = 240;
            }
            else
            {
                Temp.Width = 240;
                Temp.Height = 332;

                Temp.pictureBox1.Width = 240;
                Temp.pictureBox1.Height = 320;
            }

            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);

            //显示图像
            Temp.pictureBox1.Image = tmp;

            //底部文字说明
            Temp.TxtShow.Text = txt;

            flowLayoutOnRight.Controls.Add(Temp);
        }
        #endregion
        
        #region 加载至左侧区域
        /// <summary>
        /// 加载图像至左侧区域(无底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        private void LoadPicOnLeft(Bitmap Pic)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 200 && Pic.Height < 150) || (Pic.Width < 150 && Pic.Height < 200))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 200;
                Temp.Height = 162;

                Temp.pictureBox1.Width = 200;
                Temp.pictureBox1.Height = 150;
            }
            else
            {
                Temp.Width = 150;
                Temp.Height = 212;

                Temp.pictureBox1.Width = 150;
                Temp.pictureBox1.Height = 200;
            }

            //显示图像
            Temp.pictureBox1.Image = Pic;
            Temp.pictureBox1.MouseDoubleClick += new MouseEventHandler(SelectPic);

            //Temp.TxtShow.Text = txt;

            flowLayoutOnLeft.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至左侧区域(无底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        private void LoadPicOnLeft(PICLib.GrayImg Pic)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 200 && Pic.Height < 150) || (Pic.Width < 150 && Pic.Height < 200))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 200;
                Temp.Height = 162;

                Temp.pictureBox1.Width = 200;
                Temp.pictureBox1.Height = 150;
            }
            else
            {
                Temp.Width = 150;
                Temp.Height = 212;

                Temp.pictureBox1.Width = 150;
                Temp.pictureBox1.Height = 200;
            }
            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);
            //显示图像
            Temp.pictureBox1.Image = tmp;
            Temp.pictureBox1.MouseDoubleClick += new MouseEventHandler(SelectPic);

            ////底部文字说明
            //Temp.TxtShow.Text = txt;

            flowLayoutOnLeft.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至左侧区域(有底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        /// <param name="txt">说明文字</param>
        private void LoadPicOnLeft(Bitmap Pic, string txt)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 200 && Pic.Height < 150) || (Pic.Width < 150 && Pic.Height < 200))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 200;
                Temp.Height = 162;

                Temp.pictureBox1.Width = 200;
                Temp.pictureBox1.Height = 150;
            }
            else
            {
                Temp.Width = 150;
                Temp.Height = 212;

                Temp.pictureBox1.Width = 150;
                Temp.pictureBox1.Height = 200;
            }

            //显示图像
            Temp.pictureBox1.Image = Pic;
            
            Temp.pictureBox1.MouseDoubleClick+=new MouseEventHandler(SelectPic);

            //底部文字说明
            Temp.TxtShow.Text = txt;

            flowLayoutOnLeft.Controls.Add(Temp);
        }

        /// <summary>
        /// 加载图像至左侧区域(有底部说明文字)
        /// </summary>
        /// <param name="Pic">传入图像</param>
        /// <param name="txt">说明文字</param>
        private void LoadPicOnLeft(PICLib.GrayImg Pic, string txt)
        {
            DrawPic Temp = new DrawPic();

            if ((Pic.Width < 200 && Pic.Height < 150) || (Pic.Width < 150 && Pic.Height < 200))
            {
                Temp.pictureBox1.Width = Pic.Width;
                Temp.pictureBox1.Height = Pic.Height;
                Temp.Height = Pic.Height + 12;
                Temp.Width = Pic.Width;
            }
            else if (Pic.Height < Pic.Width)
            {
                Temp.Width = 200;
                Temp.Height = 162;

                Temp.pictureBox1.Width = 200;
                Temp.pictureBox1.Height = 150;
            }
            else
            {
                Temp.Width = 150;
                Temp.Height = 212;

                Temp.pictureBox1.Width = 150;
                Temp.pictureBox1.Height = 200;
            }
            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, Pic);
            //显示图像
            Temp.pictureBox1.Image = tmp;
            Temp.pictureBox1.MouseDoubleClick += new MouseEventHandler(SelectPic);

            //底部文字说明
            Temp.TxtShow.Text = txt;
            flowLayoutOnLeft.Controls.Add(Temp);
        }

        /// <summary>
        /// 鼠标双击图片事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPic(object sender, MouseEventArgs e)
        {
            PictureBox tmp = (PictureBox)sender;
            IsClearRightPanel();

            PicMap = (Bitmap)tmp.Image;
            LoadPicOnRight(PicMap);

            PICLib.PicBase.ImgToArray(out GrayImg, PicMap);
            PICLib.PicBase.ImgToArray(out ColorImg,PicMap);
            //MessageBox.Show(sender.ToString() + '\n' + e.ToString());
            //PICLib.PicBase.ImgToArray(out GrayImg, PicMap);
            //PICLib.PicBase.ImgToArray(out ColorImg, PicMap);
        }

        #endregion
        #endregion

        private void  IsClearRightPanel()
        {
            if (AutoClearRight.Checked == true)
                flowLayoutOnRight.Controls.Clear();
                
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            DataSet pic; 
            db.OpenDatabase Op = new DipProcess.db.OpenDatabase();

            Op.ShowDialog();
            pic = Op.ds;
            if ( pic.Tables.Count>0)
            {
                int count =pic.Tables[0].Rows.Count;
                string[] split = { "X", "x", "×" };
                PICLib.GrayImg picin;
                for (int i = 0; i < count; i++)
                {

                    //picin.Img=(byte[])pic.Tables[0].Rows[i]["IrisImg"];
                    //string[] picsize = pic.Tables[0].Rows[i]["Size"].ToString().Split(split, StringSplitOptions.None);
                    //picin.Width = int.Parse(picsize[0]);
                    //picin.Height = int.Parse(picsize[1]);

                    LoadPicOnLeft(ByteToBmp((byte[])pic.Tables[0].Rows[i]["IrisImg"]) , pic.Tables[0].Rows[i]["LibFrom"].ToString() + pic.Tables[0].Rows[i]["FileName"].ToString());
                }
            }
        }

        private Bitmap ByteToBmp(byte[] PixelValues)
        {
            MemoryStream ms = new MemoryStream(PixelValues);
            Bitmap inMap = new Bitmap(Bitmap.FromStream(ms));
            ms.Close();
            return inMap;
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AutoClear.Checked == true)
            {
                flowLayoutOnLeft.Controls.Clear();
                flowLayoutOnRight.Controls.Clear();
            }
            else if (AutoClearLeft.Checked == true)
            {
                flowLayoutOnLeft.Controls.Clear();
            }
            OpenFileDialog PicOpen = new OpenFileDialog();
            PicOpen.Filter = "Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.jpg)|*.jpg|所有合适文件(*.bmp/*.jpg)|*.bmp;*.jpg|所有文件|*.*";
            PicOpen.FilterIndex = 3;
            PicOpen.RestoreDirectory = true;
            if (PicOpen.ShowDialog() == DialogResult.OK)
            {
                //UnLoadAll();
                FileStream fs = new FileStream(PicOpen.FileName, FileMode.Open);
                PicMap = new Bitmap(fs, false);
                fs.Close();
                LoadPicOnLeft(PicMap, "图像");

                PICLib.PicBase.ImgToArray(out GrayImg, PicMap);
                PICLib.PicBase.ImgToArray(out ColorImg, PicMap);
                //LoadSP(Picmap, PictureBoxSizeMode.StretchImage, "原始彩色图像");
            }
        }

        private void 灰度图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            //PICLib.Process pic = new PICLib.Process();
            //pic.GrayPic=PICLib.PicBase.
            //PICLib.PicBase.ArrayToImg(out PicMap, GrayImg);
            LoadPicOnRight(GrayImg , "灰度图");
        }

        private void 灰度均衡化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.InteEqualize(out TempGrayImg, GrayImg);
            LoadPicOnRight(TempGrayImg, "灰度均衡化");
        }

        private void 预处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PicMap == null)
            {
                MessageBox.Show("待分析图片不存在，请加载图片后再试！","提示");
                return;
            }
            if (AutoClearRight.Checked == true)
            {
                flowLayoutOnRight.Controls.Clear();
                Update();
            }
            
            //PICLib.Process pic = new PICLib.Process();
            //pic.GrayPic=PICLib.PicBase.
            //PICLib.PicBase.ImgToArray(out GrayImg, PicMap);
            //PICLib.PicBase.ArrayToImg(out PicMap, GrayImg);
            LoadPicOnRight(GrayImg, "灰度图");
        }

        private void 虹膜定位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.IrisLocator IrisLocTool=PICLib.IrisLocator.GetInstance();

            PICLib.IrisLocation iris = IrisLocTool.findIris(GrayImg);
            PICLib.PicBase.ArrayToImg(out TempBitMap, GrayImg);
            //LoadPicOnRight(PicMap, "确定巩膜位置");

            //Bitmap tmp = Process.Base2Img(gryBase);
            //PICLib.PicBase.ArrayToImg(out tmp, GrayImg);
            Graphics grdevice = Graphics.FromImage(TempBitMap);
            Pen p = new Pen(Color.Green, 1.5f);
            grdevice.DrawEllipse(p, iris.pupil.x0 - iris.pupil.radius, iris.pupil.y0 - iris.pupil.radius, iris.pupil.radius * 2, iris.pupil.radius * 2);
            Pen p1 = new Pen(Color.Red, 2.0f);
            grdevice.DrawEllipse(p1, iris.limbus.x0 - iris.limbus.radius, iris.limbus.y0 - iris.limbus.radius, iris.limbus.radius * 2, iris.limbus.radius * 2);
            LoadPicOnRight(TempBitMap, "定位标记");

            //PICLib.GrayImg UnwrapperIris;
            IrisLocTool.IrisRoll(GrayImg, out GrayImg, iris.pupil, iris.limbus, 256, 128);
            //LoadSP(UnwrapperIris, PictureBoxSizeMode.Normal, "虹膜展开");
            //PICLib.PicBase.ArrayToImg(out PicMap,GrayImg);
            LoadPicOnRight(GrayImg, "虹膜展开");

            //图像增强，做直方图均衡化
            PICLib.Process.InteEqualize(out GrayImg ,GrayImg);
            //LoadSP(UnwrapperIris, PictureBoxSizeMode.Normal, "图像增强，均衡化");
            //PICLib.PicBase.ArrayToImg(out PicMap, GrayImg);
            LoadPicOnRight(GrayImg, "图像增强，均衡化");
        }

        private void 去噪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(0, GrayImg.Height / 8, GrayImg.Width / 4, 7 * GrayImg.Height / 8);
            PICLib.Process.GetSubImg(out GrayImg , GrayImg , rect);
            //LoadSP(gryBase, PictureBoxSizeMode.Zoom, "取出眼睑干扰，取图像左侧1/3");
            //PICLib.PicBase.ArrayToImg(out PicMap, GrayImg);
            LoadPicOnRight(GrayImg, "去除眼睑干扰，取图像左侧1/3");
        }

        private void 特征提取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Gabor CodingTool = PICLib.Gabor.GetInstance();
            CodingTool.generateCode(GrayImg, false);
            GrayImg = CodingTool.ICode.toBaseImage();
            //richTextBox1.AppendText("编码：");
            //richTextBox1.AppendText(CodingTool.ICode.CodeToStr());
            //LoadSP(RcodingMap, PictureBoxSizeMode.Zoom, "虚部奇对称特征编码图像");

            //PICLib.PicBase.ArrayToImg(out PicMap, GrayImg);
            LoadPicOnRight(GrayImg, "虚部奇对称特征编码图像");
            //if (Loading.OriginalIrisID == -1 && Loading.MatchingIrisID == -1)
            //{
            //    Loading.OriginalIrisID = Loading.OperatingIrisID;
            //    //Loading.OriginalIrisCode = CodingTool.ICode;
            //    Loading.OriginalIrisCode = CodingTool.ICode.clone();
            //}
            //else
            //{
            //    Loading.MatchingIrisID = Loading.OperatingIrisID;
            //    //Loading.MatchingIrisCode = CodingTool.ICode;
            //    Loading.MatchingIrisCode = CodingTool.ICode.clone();
            //}
        }

        private void 特征匹配ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 图像预处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void 自动识别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PicMap == null)
            {
                MessageBox.Show("待分析图片不存在，请加载图片后再试！");
                return;
            }
            if (AutoClearRight.Checked == true)
            {
                flowLayoutOnRight.Controls.Clear();
                Update();
            }
            PICLib.PlateLoc LPRtool = PICLib.PlateLoc.GetInstance();
            //定义处理对象
            PICLib.GrayImg objPrcGry, gryBak1;
            PICLib.PicBase.ImgToArray(out objPrcGry, PicMap);    //图像转为图像基

            //克隆一个副本
            PICLib.PicBase.ImgClone(out gryBak1, objPrcGry);

            //对图像进行高通滤波
            PICLib.Wavelets Wavetool = PICLib.Wavelets.GetInstance();
            Wavetool.HighFilter(objPrcGry, 2);

            //改变大小（修正后）
            PICLib.Process.Zoomup(out objPrcGry, objPrcGry, 640, 288);

            //垂直边缘检测
            int[] TemplateArr = new int[9] { 1, 0, -1, 4, 0, -4, 1, 0, -1 };
            PICLib.Process.Template(out objPrcGry, objPrcGry, TemplateArr, 3, 1);

            //车牌区域二值化
            int t1 = PICLib.Process.BinaryImg(objPrcGry);
            PICLib.Process.BinaryImg(objPrcGry, out objPrcGry, t1);

            //从缩略图中定位车牌区域
            Rectangle getPltRect1 = LPRtool.GetLP_1(objPrcGry);

            ////将缩略图中车牌矩形区域转换为原图区域（新增）
            double ZoomX = PicMap.Width / 640.0d;
            double ZoomY = PicMap.Height / 288.0d;
            int NewX = (int)(getPltRect1.Left * ZoomX);
            int NewY = (int)(getPltRect1.Top * ZoomY);
            int NewWidth = (int)(getPltRect1.Width * ZoomX);
            int NewHeight = (int)(getPltRect1.Height * ZoomY);
            Rectangle rect1 = new Rectangle(NewX, NewY, NewWidth, NewHeight);

            //在原图中标记车牌区域（新增）
            Bitmap tmp;
            PICLib.PicBase.ArrayToImg(out tmp, gryBak1);
            Graphics dev = Graphics.FromImage(tmp);
            dev.DrawRectangle(new Pen(Color.Red, 2.0f), rect1);
            dev.Dispose();
            //LoadSP(tmp, PictureBoxSizeMode.StretchImage, "标记位置");

            //截取车牌区域
            PICLib.GrayImg pltImg1;
            PICLib.Process.GetSubImg(out pltImg1, gryBak1, rect1);

            //Canny算子提取车牌边缘
            PICLib.GrayImg result;
            PICLib.Process.Canny(pltImg1, 0.15, 0.15, 0.79, out result);

            /*对车牌进行矫正*/
            PICLib.PAngle Angle = new PICLib.PAngle();
            LPRtool.RectifyLP(out result, result, result, ref Angle);   //校正Canny算子处理后图像,并计算旋转角度
            LPRtool.RectifyLP_2(out pltImg1, pltImg1, Angle);           //校正车牌区域图像
            //LoadSP(result, PictureBoxSizeMode.Normal, "Canny算子");
            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "矫正车牌");

            //精确定位车牌区域范围
            Rectangle rect2 = LPRtool.GetSubImg(result);

            //标记车牌位置
            PICLib.PicBase.ArrayToImg(out tmp, result);
            dev = Graphics.FromImage(tmp);
            dev.DrawRectangle(new Pen(Color.Red, 2.0f), rect2);
            dev.Dispose();
            //LoadSP(tmp, PictureBoxSizeMode.Normal, "二次标记位置");

            //截取车牌区域

            PICLib.Process.GetSubImg(out pltImg1, pltImg1, rect2);  //截取车牌图像
            PICLib.Process.GetSubImg(out result, result, rect2);   //截取canny算子所得图像
            //LoadSP(pltImg1, PictureBoxSizeMode.Normal, "二次截取车牌区域");

            //使用Canny算子处理的结果进行字符切割
            PICLib.GrayImg[] Chars = PICLib.PlateLoc.CutCharByCanny(pltImg1, result);

            ////再过滤，判断哪些是文字，哪些不是
            string PlateStr = "陕K12341";
            Bitmap temp;

            PICLib.RecoChar RecoTool = PICLib.RecoChar.GetInstance();
            for (int i = 0; i < 7; i++)
            {
                //LoadSP(Chars[i], PictureBoxSizeMode.Normal, i.ToString());
                PICLib.PicBase.ArrayToImg(out temp, Chars[i]);
                LoadPicOnRight(temp, i.ToString());


                /*------------------字模匹配-----------------------*/
                PICLib.Process.Zoomup(out Chars[i], Chars[i], 60, 120);
                string c;

                byte[] E;
                double f;
                RecoTool.Eigenvalue(10, 20, out E, Chars[i]);
                if (i == 0)
                    RecoTool.MatchChr(E, out c, out f);
                else if (i == 1)
                    RecoTool.MatchAlph(E, out c, out f);
                else
                    RecoTool.MatchNum(E, out c, out f);

                if (i < 7 /*&& c != ""*/)
                {
                    PlateStr = PlateStr.Remove(i, 1);
                    PlateStr = PlateStr.Insert(i, c);
                }
            }

            MessageBox.Show(PlateStr);
        }

        private void IsAutoClear(object sender, EventArgs e)
        {
            ToolStripMenuItem obj = (ToolStripMenuItem)sender;
            if(obj.Checked==true)
            {
                obj.Checked=false;
            }
            else 
                obj.Checked=true;
        }

        private bool IsPicExist()
        {
            if (PicMap == null)
            {
                MessageBox.Show("待分析图片不存在，请加载图片后再试！");
                return false;
            }
            else
                return true;
        }

        private void 灰度拉伸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            byte MaxBright, MinBright;

            PICLib.Process.Bright_Range(out MaxBright, out MinBright, GrayImg);
            PICLib.Process.ExtendPic(GrayImg, out TempGrayImg, MinBright, MaxBright);
            
            LoadPicOnRight(TempGrayImg, "灰度拉伸");
        }

        private void 灰度均衡化ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.InteEqualize(out TempGrayImg, GrayImg);
            LoadPicOnRight(TempGrayImg, "灰度均衡化");
        }

        private void 灰度反色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.Invert(out TempGrayImg, GrayImg);
            LoadPicOnRight(TempGrayImg, "灰度反色");
        }

        private void 常规方法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            BinaryImgInput Input = new BinaryImgInput();
            Input.ShowDialog();
            if (Input.value < 0)
                return;
            else
                PICLib.Process.BinaryImg(GrayImg, out TempGrayImg, Input.value);
            LoadPicOnRight(TempGrayImg, "输入阈值二值化");
        }

        private void 大类间方差法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.BinaryImg(GrayImg, out TempGrayImg);
            LoadPicOnRight(TempGrayImg, "大类间方差法二值化");
        }

        private void 大津法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            int value = PICLib.Process.BinaryImg(GrayImg);
            PICLib.Process.BinaryImg(GrayImg, out TempGrayImg, value);
            LoadPicOnRight(TempGrayImg, "输入阈值二值化");
        }

        private void 彩色图双向增强ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.ImageEnhance(ColorImg, out TempColorImg);
            LoadPicOnRight(TempColorImg, "彩色双向增强");
        }

        private void 灰度图双向增强ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            PICLib.Process.ImageEnhance(GrayImg, out TempGrayImg);
            LoadPicOnRight(GrayImg, "灰度双向增强");
        }

        private void 灰度图线性拉伸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            byte Max, Min;
            PICLib.Process.Bright_Range(out Max, out Min, GrayImg);
            PICLib.Process.LinerStretch(out TempGrayImg, GrayImg, Min, Max);
            LoadPicOnRight(TempGrayImg, "灰度线性拉伸");
        }

        private void 改变亮度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsClearRightPanel();
            if (ChangeValue.Visible == true)
                ChangeValue.Visible = false;
            else
                ChangeValue.Visible = true;
            Update();
        }

        private void n层小波变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.WaveOnce(GrayImg, 2);
            LoadPicOnRight(GrayImg, "2层小波变换");
        }

        private void 行变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.Transline(GrayImg);
            LoadPicOnRight(GrayImg, "行变换");
        }

        private void 列变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.TransRow(GrayImg);
            LoadPicOnRight(GrayImg, "列变换");
        }

        private void 低通滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.LowFilter(GrayImg, 2);
            LoadPicOnRight(GrayImg, "2层低通滤波");
        }

        private void 高通滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.HighFilter(GrayImg,2);
            LoadPicOnRight(GrayImg, "2层高通滤波");
        }

        private void 行变换ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.LapLasLine(GrayImg);
            LoadPicOnRight(GrayImg, "拉普拉斯行变换");
        }

        private void 列变换ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PICLib.Wavelets wave = new PICLib.Wavelets();
            //wave.get
            wave.LapLasRow(GrayImg);
            LoadPicOnRight(GrayImg, "拉普拉斯列变换");
        }

        private void 对比度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte max =128, Min = 128;
            double sum = 0;
            for (int i = 0; i < GrayImg.Img.Length; i++)
            {
                if (GrayImg.Img[i] > max)
                    max = GrayImg.Img[i];
                if (GrayImg.Img[i] < Min)
                    Min = GrayImg.Img[i];
                sum += GrayImg.Img[i];
            }
            sum /= GrayImg.Width * GrayImg.Height;
            MessageBox.Show("Max:" + max.ToString() + "\nMin:" + Min.ToString() + "\n对比度：" + (max - Min).ToString() + "平均亮度:" + sum.ToString());
        }
    }
}
