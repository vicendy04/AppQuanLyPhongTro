using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace QuanLyPhongTro
{
    public partial class QuanLyHopDongThue : Form
    {
        TaoXML taoXML1 = new TaoXML();
        string strCon = "Server=DESKTOP-1O7UCNA;Initial Catalog=QuanLyPhongTro;Integrated Security=True";
        string fileXML = "\\HopDongThue.xml";
        public QuanLyHopDongThue()
        {
            InitializeComponent();
            this.Load += QuanLyHopDongThue_Load;
        }
        private void QuanLyHopDongThue_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        void LoadData()
        {
            string sql = "SELECT * FROM HopDongThue";
            taoXML(sql, "HopDongThue", fileXML);
            DataTable dt = loadDataGridView(fileXML);
            dgvHopDongThue.DataSource = dt;
        }
        public void taoXML(string sql, string bang, string _FileXML)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable(bang);
                ad.Fill(dt);
                dt.WriteXml(Application.StartupPath + _FileXML, XmlWriteMode.WriteSchema);
            }
        }
        public DataTable loadDataGridView(string _FileXML)
        {
            DataTable dt = new DataTable();
            string FilePath = Application.StartupPath + _FileXML;
            if (File.Exists(FilePath))
            {
                using (FileStream fsReadXML = new FileStream(FilePath, FileMode.Open))
                {
                    dt.ReadXml(fsReadXML);
                }
            }
            else
            {
                MessageBox.Show("File không tồn tại");
            }
            return dt;
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
            string hopdong = tb_IdHopDong.Text.Trim();
            string phong = tb_Phong.Text.Trim();
            string nguoiThue = tb_NguoiThue.Text.Trim();
            string thoiHanThue = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string ngayThue = datetimeNgayThue.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(phong) || string.IsNullOrEmpty(nguoiThue))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            string newXml = $"<HopDongThue>" +
                            $"<IdHopDong>{hopdong}</IdHopDong>" +
                            $"<IdPhong>{phong}</IdPhong>" +
                            $"<IdNguoiThue>{nguoiThue}</IdNguoiThue>" +
                            $"<NgayThue>{ngayThue}</NgayThue>" +
                            $"<NgayKetThucThue>{thoiHanThue}</NgayKetThucThue>" +
                            $"</HopDongThue>";

            string filePath = Application.StartupPath + fileXML;
            Them(filePath, newXml);

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = "INSERT INTO HopDongThue (IdPhong, IdNguoiThue, NgayThue, NgayKetThucThue) " +
                                 "VALUES (@IdPhong, @IdNguoiThue, @NgayThue, @NgayKetThucThue)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {                       
                        cmd.Parameters.AddWithValue("@IdPhong", phong);
                        cmd.Parameters.AddWithValue("@IdNguoiThue", nguoiThue);
                        cmd.Parameters.AddWithValue("@NgayThue", ngayThue);
                        cmd.Parameters.AddWithValue("@NgayKetThucThue", thoiHanThue);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được lưu vào cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu vào cơ sở dữ liệu: {ex.Message}");
            }

            LoadData();
        }
        public void Them(string FileXML, string xml)
        {
            try
            {
                if (!File.Exists(FileXML))
                {
                    MessageBox.Show("File XML không tồn tại.");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(FileXML);
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = xml;

                XmlNode rootNode = doc.DocumentElement;
                if (rootNode != null)
                {
                    rootNode.AppendChild(docFrag);
                }

                doc.Save(FileXML);
                MessageBox.Show("Thêm dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}");
            }
        }

        private void btt_sua_Click(object sender, EventArgs e)
        {
            string idHopDong = tb_IdHopDong.Text.Trim();
            string phong = tb_Phong.Text.Trim();
            string nguoiThue = tb_NguoiThue.Text.Trim();
            string thoiHanThue = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string ngayThue = datetimeNgayThue.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(idHopDong) || string.IsNullOrEmpty(phong) || string.IsNullOrEmpty(nguoiThue))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi sửa.");
                return;
            }

            string filePath = Application.StartupPath + fileXML;
            string xpathCondition = $"//HopDongThue[IdHopDong='{idHopDong}']";
            string newXmlContent = $@"
            <HopDongThue>
                <IdHopDong>{idHopDong}</IdHopDong>
                <IdPhong>{phong}</IdPhong>
                <IdNguoiThue>{nguoiThue}</IdNguoiThue>
                <NgayThue>{ngayThue}</NgayThue>
                <NgayKetThucThue>{thoiHanThue}</NgayKetThucThue>
            </HopDongThue>";           
            // Thông báo trước khi cập nhật file XML
            MessageBox.Show("Đang cập nhật file XML...");
            Sua(filePath, xpathCondition, newXmlContent);
            MessageBox.Show("Cập nhật file XML thành công!");

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = @"UPDATE HopDongThue 
                                   SET IdPhong = @IdPhong, IdNguoiThue = @IdNguoiThue, 
                                       NgayThue = @NgayThue, NgayKetThucThue = @NgayKetThucThue 
                                   WHERE IdHopDong = @IdHopDong";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@IdHopDong", idHopDong);
                        cmd.Parameters.AddWithValue("@IdPhong", phong);
                        cmd.Parameters.AddWithValue("@IdNguoiThue", nguoiThue);
                        cmd.Parameters.AddWithValue("@NgayThue", ngayThue);
                        cmd.Parameters.AddWithValue("@NgayKetThucThue", thoiHanThue);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được sửa thành công trong cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa dữ liệu trong cơ sở dữ liệu: {ex.Message}");
            }

            LoadData();
        }
        public void Sua(string FileXML, string xpathCondition, string xmlContent)
        {
            try
            {
                if (!File.Exists(FileXML))
                {
                    MessageBox.Show("File XML không tồn tại.");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(FileXML);
                XmlNode oldNode = doc.SelectSingleNode(xpathCondition);

                if (oldNode == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cần sửa trong file XML.");
                    return;
                }

                XmlDocumentFragment newNode = doc.CreateDocumentFragment();
                newNode.InnerXml = xmlContent;

                XmlNode parentNode = oldNode.ParentNode;
                parentNode.ReplaceChild(newNode, oldNode);
                doc.Save(FileXML);

                MessageBox.Show("Cập nhật dữ liệu trong file XML thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa file XML: {ex.Message}");
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
                    taoXML1.xoa(fileXML, xml);
                    taoXML1.Xoa_Database("HopDongThue", "IdHopDong", tb_IdHopDong.Text);
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
                    tb_IdHopDong.Text = row.Cells["IdHopDong"]?.Value?.ToString() ?? "";
                    tb_NguoiThue.Text = row.Cells["IdNguoiThue"]?.Value?.ToString() ?? "";
                    tb_Phong.Text = row.Cells["IdPhong"]?.Value?.ToString() ?? "";
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

        private void button2_Click(object sender, EventArgs e)
        {
            ResetFields();
            MessageBox.Show("Đã đặt lại dữ liệu nhập!");
        }
    }
}