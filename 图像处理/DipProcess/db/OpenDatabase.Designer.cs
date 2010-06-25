namespace DipProcess.db
{
    partial class OpenDatabase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.Query = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.DbTables = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Connecting = new System.Windows.Forms.Button();
            this.TestConnect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ConnOption = new System.Windows.Forms.TabControl();
            this.SQL = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Certify = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DbName = new System.Windows.Forms.ComboBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.DbSource = new System.Windows.Forms.ComboBox();
            this.SQLExp = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.DbPath = new System.Windows.Forms.TextBox();
            this.ACCESS = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ConnOption.SuspendLayout();
            this.SQL.SuspendLayout();
            this.SQLExp.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 357);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.Query);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.listBox2);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Location = new System.Drawing.Point(264, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 336);
            this.panel1.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 289);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 31);
            this.label9.TabIndex = 9;
            this.label9.Text = "查询语句";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(259, 280);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(48, 43);
            this.button5.TabIndex = 8;
            this.button5.Text = "执行查询";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(53, 277);
            this.Query.Multiline = true;
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(200, 53);
            this.Query.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(129, 154);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = ">>";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(129, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 21);
            this.button3.TabIndex = 5;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(199, 61);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(108, 208);
            this.listBox2.TabIndex = 4;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(19, 61);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(104, 208);
            this.listBox1.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.DbTables);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(2, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(323, 51);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(244, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DbTables
            // 
            this.DbTables.FormattingEnabled = true;
            this.DbTables.Location = new System.Drawing.Point(90, 19);
            this.DbTables.Name = "DbTables";
            this.DbTables.Size = new System.Drawing.Size(132, 20);
            this.DbTables.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "数据表：";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(351, 308);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "确定";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(440, 308);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "返回";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(358, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "查询结果显示";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(268, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(325, 242);
            this.dataGridView1.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Cancel);
            this.groupBox3.Controls.Add(this.Connecting);
            this.groupBox3.Controls.Add(this.TestConnect);
            this.groupBox3.Location = new System.Drawing.Point(6, 288);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(256, 56);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(169, 20);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Connecting
            // 
            this.Connecting.Location = new System.Drawing.Point(88, 20);
            this.Connecting.Name = "Connecting";
            this.Connecting.Size = new System.Drawing.Size(75, 23);
            this.Connecting.TabIndex = 1;
            this.Connecting.Text = "确定";
            this.Connecting.UseVisualStyleBackColor = true;
            this.Connecting.Click += new System.EventHandler(this.Connecting_Click);
            // 
            // TestConnect
            // 
            this.TestConnect.Location = new System.Drawing.Point(6, 20);
            this.TestConnect.Name = "TestConnect";
            this.TestConnect.Size = new System.Drawing.Size(75, 23);
            this.TestConnect.TabIndex = 0;
            this.TestConnect.Text = "测试连接";
            this.TestConnect.UseVisualStyleBackColor = true;
            this.TestConnect.Click += new System.EventHandler(this.TestConnect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ConnOption);
            this.groupBox2.Location = new System.Drawing.Point(6, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 271);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // ConnOption
            // 
            this.ConnOption.Controls.Add(this.SQL);
            this.ConnOption.Controls.Add(this.SQLExp);
            this.ConnOption.Controls.Add(this.ACCESS);
            this.ConnOption.Location = new System.Drawing.Point(4, 9);
            this.ConnOption.Name = "ConnOption";
            this.ConnOption.SelectedIndex = 0;
            this.ConnOption.Size = new System.Drawing.Size(250, 256);
            this.ConnOption.TabIndex = 3;
            // 
            // SQL
            // 
            this.SQL.BackColor = System.Drawing.Color.Transparent;
            this.SQL.Controls.Add(this.label7);
            this.SQL.Controls.Add(this.UserName);
            this.SQL.Controls.Add(this.label1);
            this.SQL.Controls.Add(this.label5);
            this.SQL.Controls.Add(this.label2);
            this.SQL.Controls.Add(this.Certify);
            this.SQL.Controls.Add(this.label4);
            this.SQL.Controls.Add(this.DbName);
            this.SQL.Controls.Add(this.Password);
            this.SQL.Controls.Add(this.DbSource);
            this.SQL.Location = new System.Drawing.Point(4, 21);
            this.SQL.Name = "SQL";
            this.SQL.Padding = new System.Windows.Forms.Padding(3);
            this.SQL.Size = new System.Drawing.Size(242, 231);
            this.SQL.TabIndex = 0;
            this.SQL.Text = "SQLServer";
            this.SQL.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "用户名：";
            // 
            // UserName
            // 
            this.UserName.Location = new System.Drawing.Point(101, 144);
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Size = new System.Drawing.Size(100, 21);
            this.UserName.TabIndex = 13;
            this.UserName.Text = "sa";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源名称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "身份认证：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库名称：";
            // 
            // Certify
            // 
            this.Certify.FormattingEnabled = true;
            this.Certify.Items.AddRange(new object[] {
            "Windows 身份认证",
            "SQL Server 身份认证"});
            this.Certify.Location = new System.Drawing.Point(90, 97);
            this.Certify.Name = "Certify";
            this.Certify.Size = new System.Drawing.Size(133, 20);
            this.Certify.TabIndex = 11;
            this.Certify.Text = "Windows 身份认证";
            this.Certify.SelectedIndexChanged += new System.EventHandler(this.Certify_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "密  码：";
            // 
            // DbName
            // 
            this.DbName.FormattingEnabled = true;
            this.DbName.Location = new System.Drawing.Point(90, 60);
            this.DbName.Name = "DbName";
            this.DbName.Size = new System.Drawing.Size(133, 20);
            this.DbName.TabIndex = 10;
            this.DbName.Text = "IrisDB";
            this.DbName.Click += new System.EventHandler(this.DbName_Click);
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(101, 181);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.ReadOnly = true;
            this.Password.Size = new System.Drawing.Size(100, 21);
            this.Password.TabIndex = 8;
            this.Password.Text = "03091007";
            // 
            // DbSource
            // 
            this.DbSource.FormattingEnabled = true;
            this.DbSource.Location = new System.Drawing.Point(91, 24);
            this.DbSource.Name = "DbSource";
            this.DbSource.Size = new System.Drawing.Size(132, 20);
            this.DbSource.TabIndex = 9;
            this.DbSource.Text = "(local)";
            this.DbSource.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DbSource_MouseDown);
            // 
            // SQLExp
            // 
            this.SQLExp.BackColor = System.Drawing.Color.Transparent;
            this.SQLExp.Controls.Add(this.button1);
            this.SQLExp.Controls.Add(this.label3);
            this.SQLExp.Controls.Add(this.DbPath);
            this.SQLExp.Location = new System.Drawing.Point(4, 21);
            this.SQLExp.Name = "SQLExp";
            this.SQLExp.Padding = new System.Windows.Forms.Padding(3);
            this.SQLExp.Size = new System.Drawing.Size(242, 231);
            this.SQLExp.TabIndex = 1;
            this.SQLExp.Text = "SQLExpress";
            this.SQLExp.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(161, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据库路径：";
            // 
            // DbPath
            // 
            this.DbPath.Location = new System.Drawing.Point(31, 110);
            this.DbPath.Name = "DbPath";
            this.DbPath.Size = new System.Drawing.Size(110, 21);
            this.DbPath.TabIndex = 7;
            // 
            // ACCESS
            // 
            this.ACCESS.Location = new System.Drawing.Point(4, 21);
            this.ACCESS.Name = "ACCESS";
            this.ACCESS.Size = new System.Drawing.Size(242, 231);
            this.ACCESS.TabIndex = 2;
            this.ACCESS.Text = "ACCESS";
            this.ACCESS.UseVisualStyleBackColor = true;
            // 
            // OpenDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 357);
            this.Controls.Add(this.groupBox1);
            this.Name = "OpenDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择数据库";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ConnOption.ResumeLayout(false);
            this.SQL.ResumeLayout(false);
            this.SQL.PerformLayout();
            this.SQLExp.ResumeLayout(false);
            this.SQLExp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Certify;
        private System.Windows.Forms.ComboBox DbName;
        private System.Windows.Forms.ComboBox DbSource;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Connecting;
        private System.Windows.Forms.Button TestConnect;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox DbTables;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl ConnOption;
        private System.Windows.Forms.TabPage SQL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.TabPage SQLExp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DbPath;
        private System.Windows.Forms.TabPage ACCESS;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox Query;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button7;
    }
}