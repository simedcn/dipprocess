using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;

namespace PICLib
{
    public class MiniArea
    {
        public double min;
        public double max;
        public int Ni;   //出现在此小区间的次数
    }

    public class Coordinate  //坐标
    {
        public Point xStart;
        public Point xEnd;
        public Point yStart;
        public Point yEnd;
        public double xRate;
        public double yRate;
        public Point O;
    }

    public class ZhiFangTu
    {
        private ArrayList yangben = new ArrayList(); //样本数据数组
        public double a;                                    //样本数据中的最小值
        public double b;                                    //样本数据中的最大值
        public ArrayList AllMinArea = new ArrayList(); //所有小区间数组
        public int count_MinArea = 10;                             //分成小区间的个数

        public Coordinate zuobiao = new Coordinate();
        public Graphics g;

        public ArrayList YangBen
        {
            get
            {
                return this.yangben;
            }
            set
            {
                this.yangben = value;
                GetBasicData();
                GetValiantData();
            }
        }
        //
        public void InitGraphic(Graphics g, Coordinate zuobiao)
        {
            this.g = g;
            this.zuobiao = zuobiao;
        }

        //求样本数据落在小区间的个数
        public void NiAtPerMinArea(MiniArea iArea)
        {
            int Ni = 0;
            for (int i = 0; i < this.YangBen.Count; i++)
            {
                double f = (double)YangBen[i];
                if (f <= iArea.max && f >= iArea.min)
                {
                    Ni++;
                }
            }
            iArea.Ni = Ni;

        }
        public void NiAtAllMinArea()
        {
            for (int i = 0; i < AllMinArea.Count; i++)
            {
                NiAtPerMinArea((MiniArea)AllMinArea[i]);
            }
        }
        //平分成若干个区间
        public void Spliter()
        {
            this.AllMinArea.Clear();
            double MiniAreaLength = (this.b - this.a) / count_MinArea;
            for (int i = 0; i < count_MinArea; i++)
            {
                MiniArea temp = new MiniArea();
                temp.min = this.a + i * MiniAreaLength;
                temp.max = this.a + (i + 1) * MiniAreaLength;

                this.AllMinArea.Add(temp);
            }
        }
        //频度最大值
        public int MaxNi()
        {
            int Count = this.AllMinArea.Count;
            int[] array = new int[Count];
            MiniArea tempMiniArea;
            for (int i = 0; i < Count; i++)
            {
                tempMiniArea = (MiniArea)AllMinArea[i];
                array[i] = tempMiniArea.Ni;
            }
            Array.Sort(array);
            int MaxNi = array[Count - 1];
            return MaxNi;
        }
        //
        //绘制坐标
        //
        private void DrawCoordinate()
        {
            Pen myPen = new Pen(System.Drawing.Color.Red, 5);
            //x轴
            myPen.EndCap = LineCap.ArrowAnchor;
            g.DrawLine(myPen, zuobiao.xStart, zuobiao.xEnd);
            //y轴
            myPen.EndCap = LineCap.NoAnchor;
            myPen.StartCap = LineCap.ArrowAnchor;
            g.DrawLine(myPen, zuobiao.yStart, zuobiao.yEnd);
            myPen.Dispose();
        }
        //
        private void DrawXYDangWei()
        {

            // Create font and brush.
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF((float)(zuobiao.xEnd.X - 30), (float)(zuobiao.xEnd.Y + 20));

            // Draw string to screen.
            g.DrawString("X", drawFont, drawBrush, drawPoint);

            drawPoint = new PointF((float)(zuobiao.yStart.X - 30), zuobiao.yStart.Y);
            g.DrawString("Y", drawFont, drawBrush, drawPoint);

            drawPoint = new PointF((float)zuobiao.O.X + 10, (float)zuobiao.O.Y + 10);
            g.DrawString("O", drawFont, drawBrush, drawPoint);

        }
        //绘制直方
        private void DrawZhiFang()
        {
            double x, y, width, height;
            MiniArea tempArea;
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            for (int i = 0; i < AllMinArea.Count; i++)
            {
                tempArea = (MiniArea)AllMinArea[i];
                x = zuobiao.O.X + tempArea.min * zuobiao.xRate;
                y = (float)(zuobiao.O.Y - (((float)tempArea.Ni / YangBen.Count) * zuobiao.yRate));
                width = (float)((tempArea.max - tempArea.min) * zuobiao.xRate);
                height = ((double)tempArea.Ni / YangBen.Count) * zuobiao.yRate;
                g.FillRectangle(myBrush, (float)x, (float)y, (float)width, (float)height);
            }
            myBrush.Dispose();
        }
        //绘制概率密度曲线
        public void DrawGailvMiDuCurve()
        {

            MiniArea tempArea;
            double x, y;
            ArrayList tempList = new ArrayList();

            PointF p;
            for (int i = 0; i < this.count_MinArea; i++)
            {
                tempArea = (MiniArea)AllMinArea[i];
                if (tempArea.Ni > 0)
                {
                    x = zuobiao.O.X + ((tempArea.min + tempArea.max) / 2) * zuobiao.xRate;
                    y = (float)(zuobiao.O.Y - (((float)tempArea.Ni / YangBen.Count) * zuobiao.yRate));
                    p = new PointF((float)x, (float)y);
                    tempList.Add(p);
                }
            }
            int Count = tempList.Count;
            PointF[] array = new PointF[Count];
            for (int j = 0; j < Count; j++)
            {
                p = (PointF)tempList[j];
                array[j] = p;
            }
            Pen Pen = new Pen(Color.Yellow, 3);
            g.DrawCurve(Pen, array, 0.6F);
            Pen.Dispose();
        }

        public void GetBasicData()
        {
            //对样本数据进行排序
            YangBen.Sort();

            //获取最大值
            a = (double)YangBen[0];
            int count = YangBen.Count;
            b = (double)YangBen[count - 1];
        }

        public void GetValiantData()
        {

            //平分区间
            Spliter();
            //求频度
            NiAtAllMinArea();
            //原点坐标
        }
        public void ClearAll(Color color)
        {
            g.Clear(color);
        }
        public void DrawAll(bool bShowZhiFang, bool bShowGailvMiDu)
        {
            if (YangBen.Count > 0)
            {
                GetBasicData();
                GetValiantData();
                DrawCoordinate();
                DrawXYDangWei();

                if (bShowZhiFang == true)
                {
                    DrawZhiFang();
                }
                if (bShowGailvMiDu == true)
                {
                    DrawGailvMiDuCurve();
                }
            }

        }


    }


}
