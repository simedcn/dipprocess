using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DipProcess.db
{
    public partial class OpenDatabase : Form
    {

         public DataSet ds = new DataSet();
        private SQLDMO.ApplicationClass sqlApp = new SQLDMO.ApplicationClass();
        private SQLDMO.NameList sqlServers;
        private SQLDMO.SQLServer sr = new SQLDMO.SQLServerClass();
        private SQLDMO._Database CurrDb;
        private SQLDMO._Table CurrTb;

        private string ConnString = "";

        public OpenDatabase()
        {
            InitializeComponent();
            sqlServers = sqlApp.ListAvailableSQLServers();
        }

        public OpenDatabase(ref DataSet DsIn)
        {
            InitializeComponent();
            sqlServers = sqlApp.ListAvailableSQLServers();
            ds = DsIn;
        }

        /// <summary>
        /// 身份认证方式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Certify_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Certify.SelectedIndex == 0)
            {
                UserName.ReadOnly = true;
                Password.ReadOnly = true;
                sr.LoginSecure = false;
            }
            else
            {
                UserName.ReadOnly = false;
                Password.ReadOnly = false;
                sr.LoginSecure=true;
            }
        }


        /// <summary>
        /// 浏览数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DbSource_MouseDown(object sender, MouseEventArgs e)
        {
            //DbSource.Items.Clear();
           
            if (DbSource.Items.Count == 0)
            {
                try
                {
                    for (int i = 0; i < sqlServers.Count; i++)
                    {
                        object sr = sqlServers.Item(i + 1);
                        //添加到服务器的列表中去
                        DbSource.Items.Add(sr.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 选择数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DbName_Click(object sender, EventArgs e)
        {
            sr = new SQLDMO.SQLServerClass();

            if (DbName.Items.Count < 1)
            {
                //DbName.Items.Clear();
                if (UserName.Text == "" || Password.Text == "")
                {
                    if (Password.Text == "" && UserName.Text == "")
                    {
                        try
                        {
                            sr.Connect(DbSource.Text.ToString(), null, null);
                            foreach (SQLDMO.Database db in sr.Databases)
                            {
                                if (db.Name != null)
                                    DbName.Items.Add(db.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else if (Password.Text == "")
                    {
                        try
                        {
                            sr.Connect(DbSource.Text.ToString(), UserName.Text, null);
                            foreach (SQLDMO.Database db in sr.Databases)
                            {
                                if (db.Name != null)
                                    DbName.Items.Add(db.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                }
                //else if (UserName.Text == "")
                //{
                //    MessageBox.Show("请填写用户名，并重新登陆！");
                //}
                else
                {
                    try
                    {
                        try
                        {
                            sr.Connect(DbSource.Text.ToString(), UserName.Text, Password.Text);
                        }
                        catch
                        {
                        }
                        foreach (SQLDMO.Database db in sr.Databases)
                        {
                            if (db.Name != null)
                                DbName.Items.Add(db.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }




        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestConnect_Click(object sender, EventArgs e)
        {

            //SQLDMO.Application sqlApp = new SQLDMO.ApplicationClass();
            if (ConnOption.SelectedIndex == 0)
            {
                SQLDMO.SQLServer sr = new SQLDMO.SQLServerClass();
                DbName.Items.Clear();
                if (UserName.Text == "" || Password.Text == "")
                {
                    if (Password.Text == "" && UserName.Text == "")
                    {
                        try
                        {
                            sr.Connect(DbSource.Text.ToString(), null, null);
                            MessageBox.Show("测试连接成功！");
                            sr.DisConnect();
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else if (Password.Text == "")
                    {
                        try
                        {
                            sr.Connect(DbSource.Text.ToString(), UserName.Text, null);
                            MessageBox.Show("测试连接成功！");
                            sr.DisConnect();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("连接不成功，出现如下错误：" + ex.ToString());
                        }
                    }
                    else if (UserName.Text == "")
                    {
                        MessageBox.Show("请填写用户名，并重新测试！");
                    }
                }
                else
                {

                    try
                    {
                        sr.Connect(DbSource.Text.ToString(), UserName.Text, Password.Text);
                        MessageBox.Show("测试连接成功！");
                        sr.DisConnect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("连接不成功，出现如下错误：" + ex.Message.ToString());
                    }
                }
            }
            else if (ConnOption.SelectedIndex == 1)
            {
                SqlConnection conn = new SqlConnection(ConnString);
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        MessageBox.Show("连接成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }


        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connecting_Click(object sender, EventArgs e)
        {
            if (ConnOption.SelectedIndex == 0)
            {
                try
                {
                    sr.Connect(this.DbSource.Text.ToString(), this.UserName.Text, this.Password.Text);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message.ToString());
                    //return;
                }

                for (int i = 0; i < sr.Databases.Count; i++)
                {
                    if (sr.Databases.Item(i + 1, "dbo").Name == this.DbName.Text.ToString())
                    {
                        SQLDMO._Database db = sr.Databases.Item(i + 1, "dbo");
                        this.DbTables.Items.Clear();
                        for (int j = 0; j < db.Tables.Count; j++)
                        {
                            this.DbTables.Items.Add(db.Tables.Item(j + 1, "dbo").Name);
                        }
                        CurrDb = db;
                        break;
                    }
                }
                MessageBox.Show("连接成功！");
                this.Width = 612;
                //this.StartPosition = FormStartPosition.CenterParent;
                panel1.Visible = true;
                //this.Location.X -= this.Width / 2;
                //this.Location.Offset(-168, 0);
                this.SetDesktopLocation((Screen.GetWorkingArea(this).Width - Width) / 2, (Screen.GetWorkingArea(this).Height - Height) / 2);
                //this.UpdateBounds();
                DbTables.SelectedIndex = 0;
            }
            else if (ConnOption.SelectedIndex == 1)
            {
                SqlConnection conn = new SqlConnection(ConnString);
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        MessageBox.Show("连接成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                //MessageBox.Show("数据库文件连接");
            }
        }


        /// <summary>
        /// 取消数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        /// <summary>
        /// 查询数据库指定数据表字段列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1 .Items.Clear();
            string TableName = DbName.SelectedText.ToString();
            CurrTb = CurrDb.Tables.Item(DbTables.SelectedIndex + 1, "dbo");
            foreach (SQLDMO.Column curr in CurrTb.Columns)
            {
                this.listBox1.Items.Add(curr.Name.ToString());
            }

            CreateQueryString();
        
        }

        private void CreateQueryString()
        {
            if (Certify.Text != "Windows 身份认证")
            {
                ConnString = @"Data Source=" + DbSource.Text.ToString() + ";Initial Catalog=" + DbName.Text.ToString() + ";User ID=" + UserName.Text.ToString() + ";Password=" + Password.Text.ToString();
            }
            else
            {

                ConnString = @"Data Source=" + DbSource.Text.ToString() + ";Initial Catalog=" + DbName.Text.ToString() + ";Integrated Security=True";
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据表中选中字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            Query.Text = "Select  top(10) ";
            foreach(Object tmp in listBox1.SelectedItems)
            {
                listBox2.Items.Add(tmp);
                Query.Text += "[" + tmp.ToString() + "],";
            }
            listBox1.SelectedItems.Clear();
            

            Query.Text = Query.Text.Remove(Query.Text.Length - 1);
            Query.Text += " from [" + DbTables.Text.ToString() + "]";
        }

        /// <summary>
        /// 查询数据表中全部字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            foreach (Object tmp in listBox1.Items)
            {
                listBox2.Items.Add(tmp);
            }
            listBox1.SelectedItems.Clear();
            Query.Text = "Select top(10) * from [" + DbTables.Text.ToString() + "] order by newid()";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection ConnAcc = new SqlConnection(ConnString); //Sql链接类的实例化
            SqlDataAdapter da = new SqlDataAdapter(Query.Text, ConnAcc);
            //DataSet ds = new DataSet();
            //try
            {
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                panel1.Visible = false;
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string DbPath = "";
            OpenFileDialog db = new OpenFileDialog();
            db.Filter = "数据库文件*.mdf|*.mdf";
            db.RestoreDirectory = true;
            db.ShowDialog();
            DbPath.Text = db.FileName;
            ConnString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=" + DbPath.Text + ";Integrated Security=True;Connect Timeout=30;User Instance=true";
        }

      
    }
}
