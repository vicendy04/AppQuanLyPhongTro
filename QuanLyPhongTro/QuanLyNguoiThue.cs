using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyPhongTro
{
    public partial class QuanLyNguoiThue : Form
    {
        TaoXML taoXML = new TaoXML();
        string fileXML = "\\NguoiThueTro.xml";

        public QuanLyNguoiThue()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            string sql = "select * from NguoiThueTro";
            taoXML.taoXML(sql, "NguoiThueTro", fileXML);
            dgvNguoiThue.DataSource = taoXML.loadDataGridView(fileXML);
        }

        void ResetFields()
        {
            txbIdNguoiThue.Text = "";
            txbHoTen.Text = "";
            txbSDT.Text = "";
            txbCCCD.Text = "";
            dtpkNgaySinh.Value = DateTime.Now;
            txbQueQuan.Text = "";
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbHoTen.Text) ||
                    string.IsNullOrWhiteSpace(txbSDT.Text) ||
                    string.IsNullOrWhiteSpace(txbCCCD.Text) ||
                    string.IsNullOrWhiteSpace(txbQueQuan.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<NguoiThueTro>";
                xml += "<IdNguoiThue>" + taoXML.GetNextId(fileXML, "IdNguoiThue") + "</IdNguoiThue>";
                xml += "<HoTen>" + txbHoTen.Text.Trim() + "</HoTen>";
                xml += "<SDT>" + txbSDT.Text.Trim() + "</SDT>";
                xml += "<CCCD>" + txbCCCD.Text.Trim() + "</CCCD>";
                xml += "<NgaySinh>" + dtpkNgaySinh.Value.ToString("yyyy-MM-dd") + "</NgaySinh>";
                xml += "<QueQuan>" + txbQueQuan.Text.Trim() + "</QueQuan>";
                xml += "</NguoiThueTro>";

                taoXML.Them(Application.StartupPath + fileXML, xml);
                taoXML.Them_Database("NguoiThueTro", fileXML);

                LoadData();
                ResetFields();
                MessageBox.Show("Thêm người thuê thành công!");
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
                if (string.IsNullOrWhiteSpace(txbHoTen.Text) ||
                    string.IsNullOrWhiteSpace(txbSDT.Text) ||
                    string.IsNullOrWhiteSpace(txbCCCD.Text) ||
                    string.IsNullOrWhiteSpace(txbQueQuan.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string xml = "<IdNguoiThue>" + txbIdNguoiThue.Text + "</IdNguoiThue>";
                xml += "<HoTen>" + txbHoTen.Text.Trim() + "</HoTen>";
                xml += "<SDT>" + txbSDT.Text.Trim() + "</SDT>";
                xml += "<CCCD>" + txbCCCD.Text.Trim() + "</CCCD>";
                xml += "<NgaySinh>" + dtpkNgaySinh.Value.ToString("yyyy-MM-dd") + "</NgaySinh>";
                xml += "<QueQuan>" + txbQueQuan.Text.Trim() + "</QueQuan>";

                string sql = "//NguoiThueTro[IdNguoiThue='" + txbIdNguoiThue.Text + "']";
                taoXML.sua(Application.StartupPath + fileXML, sql, xml, "NguoiThueTro");
                taoXML.Sua_Database("NguoiThueTro", fileXML, "IdNguoiThue", txbIdNguoiThue.Text);
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
                if (string.IsNullOrEmpty(txbIdNguoiThue.Text))
                {
                    MessageBox.Show("Vui lòng chọn người thuê cần xóa!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn xóa người thuê này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string xml = "//NguoiThueTro[IdNguoiThue=" + txbIdNguoiThue.Text + "]";
                    taoXML.xoa(fileXML, xml);
                    taoXML.Xoa_Database("NguoiThueTro", "IdNguoiThue", txbIdNguoiThue.Text);
                    LoadData();
                    ResetFields();
                    MessageBox.Show("Xóa người thuê thành công!");
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

        private void dgvNguoiThue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNguoiThue.Rows[e.RowIndex];
                txbIdNguoiThue.Text = row.Cells["IdNguoiThue"].Value.ToString();
                txbHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txbSDT.Text = row.Cells["SDT"].Value.ToString();
                txbCCCD.Text = row.Cells["CCCD"].Value.ToString();
                dtpkNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txbQueQuan.Text = row.Cells["QueQuan"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txbIdNguoiThue.Text = string.Empty;
            txbHoTen.Text = string.Empty;
            txbSDT.Text = string.Empty;
            txbCCCD.Text = string.Empty;
            txbQueQuan.Text = string.Empty;

            // Đặt giá trị DateTimePicker về ngày hiện tại
            dtpkNgaySinh.Value = DateTime.Now;

            // Hiển thị thông báo (nếu cần)
            MessageBox.Show("Đã đặt lại dữ liệu nhập!");
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo SaveFileDialog để chọn nơi lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML files (*.html)|*.html",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = "DanhSachNguoiThue.html"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string xmlPath = Application.StartupPath + fileXML;
                    string xsltPath = Application.StartupPath + "\\XuatNguoiThue.xslt";

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