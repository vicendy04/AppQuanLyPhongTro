﻿using System;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class QuanLyThietBi : Form
    {
        TaoXML taoXML = new TaoXML();
        string fileXML = "\\ThietBi.xml";

        public QuanLyThietBi()
        {
            InitializeComponent();
            LoadData();
            LoadComboBox();
        }

        void LoadData()
        {
            string sql = "select * from ThietBi";
            taoXML.taoXML(sql, "ThietBi", fileXML);
            dataGridView1.DataSource = taoXML.loadDataGridView(fileXML);
        }

        void LoadComboBox()
        {
            string sql = "select * from Phong";
            taoXML.taoXML(sql, "Phong", "\\Phong.xml");
            cbbIdPhong.DataSource = taoXML.loadDataGridView("\\Phong.xml");
            cbbIdPhong.DisplayMember = "IdPhong";
            cbbIdPhong.ValueMember = "IdPhong";
        }

        void ResetFields()
        {
            txbIdThietBi.Text = "";
            txbTenThietBi.Text = "";
            if (cbbIdPhong.Items.Count > 0)
                cbbIdPhong.SelectedIndex = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbTenThietBi.Text) ||
                    cbbIdPhong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<ThietBi>";
                xml += "<IdThietBi>" + taoXML.GetNextId(fileXML, "IdThietBi") + "</IdThietBi>";
                xml += "<TenThietBi>" + txbTenThietBi.Text.Trim() + "</TenThietBi>";
                xml += "<IdPhong>" + cbbIdPhong.SelectedValue.ToString() + "</IdPhong>";
                xml += "</ThietBi>";

                taoXML.Them(Application.StartupPath + fileXML, xml);
                taoXML.Them_Database("ThietBi", fileXML);

                LoadData();
                ResetFields();
                MessageBox.Show("Thêm thiết bị thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbTenThietBi.Text) ||
                    cbbIdPhong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<IdThietBi>" + txbIdThietBi.Text + "</IdThietBi>";
                xml += "<TenThietBi>" + txbTenThietBi.Text.Trim() + "</TenThietBi>";
                xml += "<IdPhong>" + cbbIdPhong.SelectedValue.ToString() + "</IdPhong>";

                string sql = "//ThietBi[IdThietBi='" + txbIdThietBi.Text + "']";
                taoXML.sua(Application.StartupPath + fileXML, sql, xml, "ThietBi");
                taoXML.Sua_Database("ThietBi", fileXML, "IdThietBi", txbIdThietBi.Text);
                LoadData();
                ResetFields();
                MessageBox.Show("Cập nhật thông tin thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txbIdThietBi.Text))
                {
                    MessageBox.Show("Vui lòng chọn thiết bị cần xóa!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa thiết bị này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string xml = "//ThietBi[IdThietBi=" + txbIdThietBi.Text + "]";
                    taoXML.xoa(fileXML, xml);
                    taoXML.Xoa_Database("ThietBi", "IdThietBi", txbIdThietBi.Text);
                    LoadData();
                    ResetFields();
                    MessageBox.Show("Xóa thiết bị thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txbIdThietBi.Text = row.Cells["IdThietBi"].Value.ToString();
                txbTenThietBi.Text = row.Cells["TenThietBi"].Value.ToString();
                cbbIdPhong.SelectedValue = row.Cells["IdPhong"].Value.ToString();
            }
        }
    }
}