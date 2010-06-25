using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TestDataBase
{
    /// <summary>
    /// 获取数据库服务器列表
    /// </summary>
    public partial class Form1 : Form
    {
        private DataSet ds = new DataSet();
        private SQLDMO.ApplicationClass sqlApp = new SQLDMO.ApplicationClass(); 
        private SQLDMO.NameList sqlServers;
        private SQLDMO.SQLServer sr = new SQLDMO.SQLServerClass();
        private SQLDMO._Database CurrDb;
        private SQLDMO._Table CurrTb;

        private string ConnString = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            sqlServers = sqlApp.ListAvailableSQLServers(); 
            if(sqlServers != null)
            {
                for(int i=0;i<sqlServers.Count;i++) 
                { 
                    object srv = sqlServers.Item( i + 1);
                    //添加到服务器的列表中去
                    this.listBox1.Items.Add(srv.ToString());                         
                }
            }
        }

        /// <summary>
        /// 获取指定服务器数据库列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                sr.Connect(this.listBox1.SelectedItem.ToString(), "sa", "sa");
            }
            catch
            { 
            }
            if (!ds.Tables.Contains("db"))
            {
                //创建一个DataTable
                DataTable tmp = new DataTable("db");
                ds.Tables.Add(tmp);
                tmp.Columns.Add("Name");
                tmp.Columns.Add("Owner");
                tmp.Columns.Add("Size");
                tmp.Columns.Add("CreatDate");
            }
            DataTable dt = ds.Tables["db"];
            dt.Clear();
            foreach(SQLDMO.Database db in sr.Databases)
            {
                if(db.Name != null)
                {
                    DataRow dr = dt.NewRow();
                    //获取数据库的名称
                    dr["Name"] = db.Name;
                    //获取数据库的所有者
                    dr["Owner"] = db.Owner;
                    //获取数据库的大小
                    dr["Size"] = db.Size;
                    //获取数据库的创建日期
                    dr["CreatDate"] = db.CreateDate;
                    dt.Rows.Add(dr);
                }
            }
            //绑定数据
            this.dataGridView1.DataSource = dt;
            //this.Result.DataBind();

        }

        /// <summary>
        /// 获取指定数据库数据表列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //CurrDb = sr.Databases[ds.Tables["db"].Rows[Result.CurrentRow.Index]["Name"].ToString()];
            try
            {
                sr.Connect(this.listBox1.SelectedItem.ToString(), "sa", "sa");
            }
            catch
            { 
            }
            //创建一个DataTable
            if (!ds.Tables.Contains("dt"))
            {
                DataTable tmp = new DataTable("dt");
                ds.Tables.Add(tmp);
                tmp.Columns.Add("dbName");
                tmp.Columns.Add("Name");
                tmp.Columns.Add("Owner");
                tmp.Columns.Add("CreatDate");
                tmp.Columns.Add("PrimaryKey");
            }
            DataTable dt = ds.Tables["dt"];
            dt.Clear();
            for(int j=0;j<sr.Databases.Count;j++) 
            { 
                if(sr.Databases.Item(j+1,"dbo").Name == ds.Tables["db"].Rows[this.dataGridView1.CurrentRow.Index]["Name"].ToString()) 
                { 
                    SQLDMO._Database db= sr.Databases.Item(j+1,"dbo");
                    CurrDb = db;
                    for(int i=0;i<db.Tables.Count;i++)
                    {
                        if (!db.SystemObject)
                        {
                            DataRow dr = dt.NewRow();
                            //表所属数据库
                            dr["DbName"] = db.Name;
                            //获取表名
                            dr["Name"] = db.Tables.Item(i + 1, "dbo").Name;
                            //获取表的所有者
                            dr["Owner"] = db.Tables.Item(i + 1, "dbo").Owner;
                            //获取表的创建日期
                            dr["CreatDate"] = db.Tables.Item(i + 1, "dbo").CreateDate;
                            //获取表的主键
                            dr["PrimaryKey"] = db.Tables.Item(i + 1, "dbo").PrimaryKey;
                            dt.Rows.Add(dr);
                        }
                    }  
                    //绑定数据
                    this.dataGridView1.DataSource = dt;
                }

           
            }
        }

        /// <summary>
        /// 获取指定数据表字段信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            int TableIndex = this.dataGridView1.CurrentRow.Index;
            string TableName = ds.Tables["dt"].Rows[TableIndex]["Name"].ToString();
            CurrTb = CurrDb.Tables.Item(TableIndex + 1, "dbo");
            foreach (SQLDMO.Column curr in CurrTb.Columns)
            {
                //ListViewItem it = new ListViewItem(curr.Name.ToString(), curr.Datatype.ToString(), curr.AllowNulls.ToString());
                ListViewItem it = new ListViewItem(new string[] { curr.Name.ToString(), curr.Datatype.ToString(), curr.AllowNulls.ToString() });
                this.listView1.Items.Add(it);

            }
            CreateQueryString();
        }

        /// <summary>
        /// 生成查询语句
        /// </summary>
        private void CreateQueryString()
        {
            ConnString = @"Data Source=" + listBox1.SelectedItem.ToString() + ";Initial Catalog=" + CurrDb.Name.ToString() + ";User ID=" + "sa" + ";Password=" + "sa";
            string Query = "SELECT top(10) ";
            for(int i=1;i<listView1.Items.Count;i++)
            {
                Query += "[" + listView1.Items[i].Text.ToString() + "] ,";
            }
            Query = Query.Remove(Query.Length - 1);
            Query += "from " + CurrTb.Name.ToString();

            //MessageBox.Show(ConnString + '\n' + Query);
            textBox1.Text = Query;
        }

        /// <summary>
        /// 指定特定查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection ConnAcc = new SqlConnection(ConnString); //Sql链接类的实例化
            SqlDataAdapter da = new SqlDataAdapter(textBox1.Text.Trim(), ConnAcc);
            DataSet temp = new DataSet();
            try
            {
                da.Fill(temp);
                this.Result.DataSource = temp.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return;
        }
    }
}
