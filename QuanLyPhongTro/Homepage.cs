using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class Homepage : QuanLyPhongTro.Form1
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap formDangNhap = new DangNhap();

            formDangNhap.Show();
            this.Hide();
        }
    }
}
