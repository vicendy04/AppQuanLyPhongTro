using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;

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
                // More comprehensive input validation
                if (string.IsNullOrWhiteSpace(tb_NguoiThue.Text) ||
                    string.IsNullOrWhiteSpace(tb_Phong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                // Validate that inputs can be parsed to integers
                if (!int.TryParse(tb_Phong.Text.Trim(), out int phong) ||
                    !int.TryParse(tb_NguoiThue.Text.Trim(), out int nguoiThue))
                {
                    MessageBox.Show("Mã phòng và mã người thuê phải là số!");
                    return;
                }

                // Validate date range
                if (datetimeNgayThue.Value > dateTimePicker1.Value)
                {
                    MessageBox.Show("Ngày kết thúc thuê phải sau ngày thuê!");
                    return;
                }

                string xml = "<HopDong>";
                xml += "<IdHopDong>" + (string.IsNullOrWhiteSpace(tb_IdHopDong.Text) ? Guid.NewGuid().ToString() : tb_IdHopDong.Text.Trim()) + "</IdHopDong>";
                xml += "<Phong>" + tb_Phong.Text.Trim() + "</Phong>";
                xml += "<NguoiThue>" + tb_NguoiThue.Text.Trim() + "</NguoiThue>";
                xml += "<ThoiHanThue>" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "</ThoiHanThue>";
                xml += "<NgayThue>" + datetimeNgayThue.Value.ToString("yyyy-MM-dd") + "</NgayThue>";
                xml += "</HopDong>";
                
                taoXML.Them(Application.StartupPath + fileXML, xml);
                taoXML.Them_Database("HopDongThue", fileXML);

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
                // More comprehensive input validation
                if (string.IsNullOrWhiteSpace(tb_IdHopDong.Text) ||
                    string.IsNullOrWhiteSpace(tb_NguoiThue.Text) ||
                    string.IsNullOrWhiteSpace(tb_Phong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                // Validate that inputs can be parsed to integers
                if (!int.TryParse(tb_Phong.Text.Trim(), out int phong) ||
                    !int.TryParse(tb_NguoiThue.Text.Trim(), out int nguoiThue))
                {
                    MessageBox.Show("Mã phòng và mã người thuê phải là số!");
                    return;
                }

                // Validate date range
                if (datetimeNgayThue.Value > dateTimePicker1.Value)
                {
                    MessageBox.Show("Ngày kết thúc thuê phải sau ngày thuê!");
                    return;
                }

                string xml = "<HopDong>";
                xml += "<IdHopDong>" + tb_IdHopDong.Text.Trim() + "</IdHopDong>";
                xml += "<Phong>" + tb_Phong.Text.Trim() + "</Phong>";
                xml += "<NguoiThue>" + tb_NguoiThue.Text.Trim() + "</NguoiThue>";
                xml += "<ThoiHanThue>" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "</ThoiHanThue>";
                xml += "<NgayThue>" + datetimeNgayThue.Value.ToString("yyyy-MM-dd") + "</NgayThue>";
                xml += "</HopDong>";

                string sql = "//HopDongThue[IdHopDong='" + tb_IdHopDong.Text + "']";
                taoXML.sua(Application.StartupPath + fileXML, sql, xml, "HopDongThue");
                taoXML.Sua_Database("HopDongThue", fileXML, "IdHopDong", tb_IdHopDong.Text);

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
                    string xml = "//HopDongThue[IdHopDong='" + tb_IdHopDong.Text + "']";
                    taoXML.xoa(fileXML, xml);
                    taoXML.Xoa_Database("HopDongThue", "IdHopDong", tb_IdHopDong.Text);
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
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvHopDongThue.Rows.Count)
                {
                    DataGridViewRow row = dgvHopDongThue.Rows[e.RowIndex];

                    // Update text boxes with appropriate column names
                    tb_IdHopDong.Text = row.Cells["IdHopDong"]?.Value?.ToString() ?? "";
                    tb_NguoiThue.Text = row.Cells["IdNguoiThue"]?.Value?.ToString() ?? "";
                    tb_Phong.Text = row.Cells["IdPhong"]?.Value?.ToString() ?? "";

                    // Safely parse dates
                    if (row.Cells["NgayKetThucThue"].Value != null)
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgayKetThucThue"].Value);
                    }

                    if (row.Cells["NgayThue"].Value != null)
                    {
                        datetimeNgayThue.Value = Convert.ToDateTime(row.Cells["NgayThue"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo SaveFileDialog để chọn nơi lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML files (*.html)|*.html",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = "DanhSachHopDong.html"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string xmlPath = Application.StartupPath + fileXML;
                    string xsltPath = Application.StartupPath + "\\XuatHopDong.xslt";

                    // Tạo XslCompiledTransform
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xsltPath);

                    // Thực hiện transform
                    using (XmlReader reader = XmlReader.Create(xmlPath))
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            xslt.Transform(reader, null, writer);
                        }
                    }

                    MessageBox.Show("Xuất danh sách thành công!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file sau khi xuất
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất danh sách: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
