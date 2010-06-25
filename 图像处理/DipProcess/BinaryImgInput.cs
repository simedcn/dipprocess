using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DipProcess
{
    public partial class BinaryImgInput : Form
    {
        public int value;
        public BinaryImgInput()
        {
            InitializeComponent();
            value = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = byte.Parse(textBox1.Text.ToString());
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
