namespace DipProcess
{
    partial class DrawPic
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SavePic = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearPic = new System.Windows.Forms.ToolStripMenuItem();
            this.TxtShow = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SavePic,
            this.ClearPic});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // SavePic
            // 
            this.SavePic.Name = "SavePic";
            this.SavePic.Size = new System.Drawing.Size(94, 22);
            this.SavePic.Text = "保存";
            this.SavePic.Click += new System.EventHandler(this.SavePic_Click);
            // 
            // ClearPic
            // 
            this.ClearPic.Name = "ClearPic";
            this.ClearPic.Size = new System.Drawing.Size(94, 22);
            this.ClearPic.Text = "清除";
            // 
            // TxtShow
            // 
            this.TxtShow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TxtShow.Location = new System.Drawing.Point(0, 240);
            this.TxtShow.Name = "TxtShow";
            this.TxtShow.Size = new System.Drawing.Size(320, 12);
            this.TxtShow.TabIndex = 1;
            this.TxtShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DrawPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxtShow);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DrawPic";
            this.Size = new System.Drawing.Size(320, 252);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label TxtShow;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SavePic;
        private System.Windows.Forms.ToolStripMenuItem ClearPic;
    }
}
