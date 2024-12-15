using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class QuanLyPhong : Form
    {
        TaoXML taoXML = new TaoXML();
        string fileXML = "\\Phong.xml";

        public QuanLyPhong()
        {
            InitializeComponent();
        }

        private void QuanLyPhong_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            string sql = "select * from Phong";
            taoXML.taoXML(sql, "Phong", fileXML);
            dgvPhong.DataSource = taoXML.loadDataGridView(fileXML);
        }

        void ResetFields()
        {
            tb_IdPhong.Text = "";
            tb_tenphong.Text = "";
            tb_giaphong.Text = "";
            cb_tang.Text = "";
            cb_trangthai.Text = "";
            // Reset other fields as necessary
        }
        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPhong.Rows[e.RowIndex];
                tb_IdPhong.Text = row.Cells["IdPhong"].Value.ToString();
                tb_tenphong.Text = row.Cells["TenPhong"].Value.ToString();
                tb_giaphong.Text = row.Cells["GiaPhong"].Value.ToString();
                cb_trangthai.Text = row.Cells["TrangThai"].Value.ToString();
                cb_tang.Text = row.Cells["IdTang"].Value.ToString();
                // Load other fields as necessary
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_them_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_tenphong.Text) || string.IsNullOrWhiteSpace(tb_giaphong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<Phong>";
                xml += "<IdPhong>" + taoXML.GetNextId(fileXML, "IdPhong") + "</IdPhong>";
                xml += "<TenPhong>" + tb_tenphong.Text.Trim() + "</TenPhong>";
                xml += "<GiaPhong>" + tb_giaphong.Text.Trim() + "</GiaPhong>";
                xml += "<TrangThai>" + cb_trangthai.Text.Trim() + "</TrangThai>";
                xml += "<IdTang>" + cb_tang.Text.Trim() + "</IdTang>";
                xml += "</Phong>";

                taoXML.Them(Application.StartupPath + fileXML, xml);
                taoXML.Them_Database("Phong", fileXML);

                LoadData();
                ResetFields();
                MessageBox.Show("Thêm phòng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void bt_sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_IdPhong.Text) || string.IsNullOrWhiteSpace(tb_tenphong.Text) || string.IsNullOrWhiteSpace(tb_giaphong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<IdPhong>" + tb_IdPhong.Text + "</IdPhong>";
                xml += "<TenPhong>" + tb_tenphong.Text.Trim() + "</TenPhong>";
                xml += "<GiaPhong>" + tb_giaphong.Text.Trim() + "</GiaPhong>";
                xml += "<TrangThai>" + cb_trangthai.Text.Trim() + "</TrangThai>";
                xml += "<IdTang>" + cb_tang.Text.Trim() + "</IdTang>";

                string sql = "//Phong[IdPhong='" + tb_IdPhong.Text + "']";
                taoXML.sua(Application.StartupPath + fileXML, sql, xml, "Phong");
                taoXML.Sua_Database("Phong", fileXML, "IdPhong", tb_IdPhong.Text);

                LoadData();
                ResetFields();
                MessageBox.Show("Cập nhật thông tin thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void bt_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_IdPhong.Text))
                {
                    MessageBox.Show("Vui lòng chọn phòng cần xóa!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string xml = "//Phong[IdPhong='" + tb_IdPhong.Text + "']";
                    taoXML.xoa(fileXML, xml);
                    taoXML.Xoa_Database("Phong", "IdPhong", tb_IdPhong.Text);
                    LoadData();
                    ResetFields();
                    MessageBox.Show("Xóa phòng thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvPhong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPhong.Rows[e.RowIndex];
                tb_IdPhong.Text = row.Cells["IdPhong"].Value.ToString();
                tb_tenphong.Text = row.Cells["TenPhong"].Value.ToString();
                tb_giaphong.Text = row.Cells["GiaPhong"].Value.ToString();
                cb_trangthai.Text = row.Cells["TrangThai"].Value.ToString();
                cb_tang.Text = row.Cells["IdTang"].Value.ToString();
                // Load other fields as necessary
            }
        }
    }
}
