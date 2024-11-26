using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class Menu : QuanLyPhongTro.Form1
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            QuanLyNguoiThue quanLyNguoiThue = new QuanLyNguoiThue();
            this.Hide();
            quanLyNguoiThue.ShowDialog();
            this.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            QuanLyThietBi quanLyThietBi = new QuanLyThietBi();
            this.Hide();
            quanLyThietBi.ShowDialog();
            this.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            QuanLyPhong quanLyPhong = new QuanLyPhong();
            this.Hide();
            quanLyPhong.ShowDialog();
            this.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            QuanLyHopDong quanLyHopDong = new QuanLyHopDong();
            this.Hide();
            quanLyHopDong.ShowDialog();
            this.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            QuanLyHoaDon quanLyHoaDon = new QuanLyHoaDon();
            this.Hide();
            quanLyHoaDon.ShowDialog();
            this.Show();
        }
    }
}
