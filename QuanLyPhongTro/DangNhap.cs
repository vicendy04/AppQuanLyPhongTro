using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class DangNhap : QuanLyPhongTro.Form1
    {
        public DangNhap()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
