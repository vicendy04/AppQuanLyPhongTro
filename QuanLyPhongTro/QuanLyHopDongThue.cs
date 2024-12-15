using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyPhongTro
{
    public partial class QuanLyHopDongThue : Form
    {
        TaoXML taoXML = new TaoXML();
        string fileXML = "\\HopDongThue.xml";
        public QuanLyHopDongThue()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            string sql = "select * from HopDongThue";
            taoXML.taoXML(sql,"HopDongThue", fileXML);
            dgvHopDongThue.DataSource = taoXML.loadDataGridView(fileXML);
        }

        private void QuanLyHopDongThue_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        void ResetFields()
        {
            tb_IdHopDong.Text = "";
            tb_NguoiThue.Text = "";
            tb_Phong.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            datetimeNgayThue.Value = DateTime.Now;
        }

        private void btt_them_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_NguoiThue.Text) ||
                    string.IsNullOrWhiteSpace(tb_Phong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<HopDong>";
                xml += "<IdHopDong>" + taoXML.GetNextId(fileXML, "IdHopDong") + "</IdHopDong>";
                xml += "<Phong>" + tb_Phong.Text.Trim() + "</Phong>";
                xml += "<NguoiThue>" + tb_NguoiThue.Text.Trim() + "</NguoiThue>";
                xml += "<ThoiHanThue>" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "</ThoiHanThue>";
                xml += "<NgayThue>" + datetimeNgayThue.Value.ToString("yyyy-MM-dd") + "</NgayThue>";
                xml += "</HopDong>";

                taoXML.Them(Application.StartupPath + fileXML, xml);
                taoXML.Them_Database("HopDong", fileXML);

                LoadData();
                ResetFields();
                MessageBox.Show("Thêm hợp đồng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btt_sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_IdHopDong.Text) ||
                    string.IsNullOrWhiteSpace(tb_NguoiThue.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<IdHopDong>" + tb_IdHopDong.Text + "</IdHopDong>";
                xml += "<IdHopDong>" + taoXML.GetNextId(fileXML, "IdHopDong") + "</IdHopDong>";
                xml += "<Phong>" + tb_Phong.Text.Trim() + "</Phong>";
                xml += "<NguoiThue>" + tb_NguoiThue.Text.Trim() + "</NguoiThue>";
                xml += "<ThoiHanThue>" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "</ThoiHanThue>";
                xml += "<NgayThue>" + datetimeNgayThue.Value.ToString("yyyy-MM-dd") + "</NgayThue>";

                string sql = "//HopDong[IdHopDong='" + tb_IdHopDong.Text + "']";
                taoXML.sua(Application.StartupPath + fileXML, sql, xml, "HopDong");
                taoXML.Sua_Database("HopDong", fileXML, "IdHopDong", tb_IdHopDong.Text);

                LoadData();
                ResetFields();
                MessageBox.Show("Cập nhật thông tin thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btt_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_IdHopDong.Text))
                {
                    MessageBox.Show("Vui lòng chọn hợp đồng cần xóa!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa hợp đồng này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string xml = "//HopDong[IdHopDong='" + tb_IdHopDong.Text + "']";
                    taoXML.xoa(fileXML, xml);
                    taoXML.Xoa_Database("HopDong", "IdHopDong", tb_IdHopDong.Text);
                    LoadData();
                    ResetFields();
                    MessageBox.Show("Xóa hợp đồng thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvHopDongThue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHopDongThue.Rows[e.RowIndex];
                tb_IdHopDong.Text = row.Cells["IdHopDong"].Value.ToString();
                tb_NguoiThue.Text = row.Cells["IdNguoiThue"].Value.ToString();
                tb_Phong.Text = row.Cells["IdPhong"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgayKetThucThue"].Value);
                datetimeNgayThue.Value = Convert.ToDateTime(row.Cells["NgayThue"].Value);
            }
        }
    }
}
