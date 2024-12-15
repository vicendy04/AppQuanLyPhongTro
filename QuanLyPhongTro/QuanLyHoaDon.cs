using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.LinkLabel;

namespace QuanLyPhongTro
{
    public partial class QuanLyHoaDon : QuanLyPhongTro.Form1
    {
        string strCon = "Server=DESKTOP-B0P3AUF\\COMPUTER1;Initial Catalog=QuanLyPhongTro;Integrated Security=True";

        public QuanLyHoaDon()
        {
            InitializeComponent();
            this.Load += QuanLyHoaDon_Load;
        }

        // Tạo file XML từ SQL Server
        public void taoXML(string sql, string bang, string _FileXML)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable(bang);
            ad.Fill(dt);
            dt.WriteXml(Application.StartupPath + _FileXML, XmlWriteMode.WriteSchema);
            con.Close();
        }

        // Load dữ liệu từ file XML vào DataGridView
        public DataTable loadDataGridView(string _FileXML)
        {
            DataTable dt = new DataTable();
            string FilePath = Application.StartupPath + _FileXML;
            if (File.Exists(FilePath))
            {
                // Tạo luồng xử lý file XML
                FileStream fsReadXML = new FileStream(FilePath, FileMode.Open);
                // Đọc file XML vào DataTable
                dt.ReadXml(fsReadXML);
                fsReadXML.Close();
            }
            else
            {
                MessageBox.Show("File không tồn tại");
            }
            return dt;
        }

        // Khi form tải lên, thực hiện kết nối và hiển thị dữ liệu
        private void QuanLyHoaDon_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM HoaDon";
            string bang = "HoaDon";
            string fileXML = "\\HoaDon.xml";

            // Tạo file XML từ cơ sở dữ liệu
            taoXML(sql, bang, fileXML);

            // Load dữ liệu từ file XML vào DataGridView
            DataTable dt = loadDataGridView(fileXML);
            dataGridView1.DataSource = dt;

            // Đặt cột Id thành ReadOnly
            if (dataGridView1.Columns.Contains("IdHoaDon"))
            {
                dataGridView1.Columns["IdHoaDon"].ReadOnly = true; // Đảm bảo tên cột trùng khớp
            }
            textBoxId.Enabled = false;
            //textBoxId.ReadOnly = true;

            // Đăng ký sự kiện CellClick
            dataGridView1.CellClick += dataGridView1_CellContentClick;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Đưa dữ liệu từ các cột lên TextBox
                textBoxId.Text = row.Cells["IdHoaDon"].Value?.ToString();
                textBoxPhong.Text = row.Cells["IdPhong"].Value?.ToString();
                textBoxSoDien.Text = row.Cells["ChuSoDien"].Value?.ToString();
                textBoxMetKhoi.Text = row.Cells["MetKhoi"].Value?.ToString();
                dateTimePicker.Value = Convert.ToDateTime(row.Cells["TuNgay"].Value);
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["DenNgay"].Value);

            }
        }

        public void Them(string FileXML, string xml)
        {
            try
            {
                // Kiểm tra nếu file XML tồn tại
                if (!File.Exists(FileXML))
                {
                    MessageBox.Show("File XML không tồn tại.");
                    return;
                }

                // Đọc file XML
                XmlTextReader textReader = new XmlTextReader(FileXML);
                XmlDocument doc = new XmlDocument();
                doc.Load(textReader);
                textReader.Close();

                // Tạo một nút mới từ chuỗi XML truyền vào
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = xml;

                // Lấy nút gốc của tài liệu XML
                XmlNode rootNode = doc.DocumentElement;

                // Thêm nút mới vào cuối nút gốc
                if (rootNode != null)
                {
                    rootNode.AppendChild(docFrag);
                }

                // Lưu lại file XML
                doc.Save(FileXML);
                MessageBox.Show("Thêm dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            textBoxId.Text = string.Empty;
            textBoxPhong.Text = string.Empty;
            textBoxSoDien.Text = string.Empty;
            textBoxMetKhoi.Text = string.Empty;

            // Đặt giá trị DateTimePicker về ngày hiện tại
            dateTimePicker.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;

            // Hiển thị thông báo (nếu cần)
            MessageBox.Show("Đã đặt lại dữ liệu nhập!");
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox và DateTimePicker
            string phong = textBoxPhong.Text.Trim();
            string soDien = textBoxSoDien.Text.Trim();
            string metKhoi = textBoxMetKhoi.Text.Trim();
            string ngayBatDau = dateTimePicker.Value.ToString("yyyy-MM-dd");
            string ngayKetThuc = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(phong) || string.IsNullOrEmpty(soDien) || string.IsNullOrEmpty(metKhoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Lưu vào file XML
            string newXml = $"<HoaDon>" +
                            $"<Phong>{phong}</Phong>" +
                            $"<SoDien>{soDien}</SoDien>" +
                            $"<MetKhoi>{metKhoi}</MetKhoi>" +
                            $"<NgayBatDau>{ngayBatDau}</NgayBatDau>" +
                            $"<NgayKetThuc>{ngayKetThuc}</NgayKetThuc>" +
                            $"</HoaDon>";

            string filePath = Application.StartupPath + "\\HoaDon.xml";
            Them(filePath, newXml);

            // Lưu vào cơ sở dữ liệu
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = "INSERT INTO HoaDon (IdPhong, ChuSoDien, MetKhoi, TuNgay, DenNgay) " +
                                 "VALUES (@IdPhong, @ChuSoDien, @MetKhoi, @TuNgay, @DenNgay)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@IdPhong", phong);
                        cmd.Parameters.AddWithValue("@ChuSoDien", soDien);
                        cmd.Parameters.AddWithValue("@MetKhoi", metKhoi);
                        cmd.Parameters.AddWithValue("@TuNgay", ngayBatDau);
                        cmd.Parameters.AddWithValue("@DenNgay", ngayKetThuc);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được lưu vào cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu vào cơ sở dữ liệu: {ex.Message}");
            }

            try
            {
                string sql = "SELECT * FROM HoaDon";
                string bang = "HoaDon";
                string fileXML = "\\HoaDon.xml";

                // Tạo lại file XML từ cơ sở dữ liệu
                taoXML(sql, bang, fileXML);

                // Load dữ liệu từ file XML vào DataGridView
                DataTable dt = loadDataGridView(fileXML);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đồng bộ file XML: {ex.Message}");
            }
        }

        public void Sua(string FileXML, string xpathCondition, string xmlContent)
        {
            try
            {
                // Kiểm tra nếu file XML tồn tại
                if (!File.Exists(FileXML))
                {
                    MessageBox.Show("File XML không tồn tại.");
                    return;
                }

                // Tải nội dung file XML
                XmlDocument doc = new XmlDocument();
                doc.Load(FileXML);

                // Tìm nút cần sửa bằng điều kiện XPath
                XmlNode oldNode = doc.SelectSingleNode(xpathCondition);

                if (oldNode == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cần sửa trong file XML.");
                    return;
                }

                // Tạo nút mới từ nội dung XML được cung cấp
                XmlDocumentFragment newNode = doc.CreateDocumentFragment();
                newNode.InnerXml = xmlContent;

                // Thay thế nút cũ bằng nút mới
                XmlNode parentNode = oldNode.ParentNode;
                parentNode.ReplaceChild(newNode, oldNode);

                // Lưu thay đổi vào file XML
                doc.Save(FileXML);

                MessageBox.Show("Cập nhật dữ liệu trong file XML thành công!");
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show($"Lỗi khi sửa file XML: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox và DateTimePicker
            string idHoaDon = textBoxId.Text.Trim();
            string phong = textBoxPhong.Text.Trim();
            string soDien = textBoxSoDien.Text.Trim();
            string metKhoi = textBoxMetKhoi.Text.Trim();
            string ngayBatDau = dateTimePicker.Value.ToString("yyyy-MM-dd");
            string ngayKetThuc = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(idHoaDon) || string.IsNullOrEmpty(phong) || string.IsNullOrEmpty(soDien) || string.IsNullOrEmpty(metKhoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi sửa.");
                return;
            }

            // Cập nhật file XML
            try
            {
                string filePath = Application.StartupPath + "\\HoaDon.xml";
                string xpathCondition = $"//HoaDon[IdHoaDon='{idHoaDon}']"; // Tìm hóa đơn theo IdHoaDon
                string newXmlContent = $@"
            <HoaDon>
                <IdHoaDon>{idHoaDon}</IdHoaDon>
                <IdPhong>{phong}</IdPhong>
                <ChuSoDien>{soDien}</ChuSoDien>
                <MetKhoi>{metKhoi}</MetKhoi>
                <TuNgay>{ngayBatDau}</TuNgay>
                <DenNgay>{ngayKetThuc}</DenNgay>
            </HoaDon>";

                Sua(filePath, xpathCondition, newXmlContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa dữ liệu trong file XML: {ex.Message}");
                return;
            }

            // Cập nhật cơ sở dữ liệu
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = @"UPDATE HoaDon 
                           SET IdPhong = @IdPhong, ChuSoDien = @ChuSoDien, MetKhoi = @MetKhoi, 
                               TuNgay = @TuNgay, DenNgay = @DenNgay 
                           WHERE IdHoaDon = @IdHoaDon";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@IdHoaDon", idHoaDon);
                        cmd.Parameters.AddWithValue("@IdPhong", phong);
                        cmd.Parameters.AddWithValue("@ChuSoDien", soDien);
                        cmd.Parameters.AddWithValue("@MetKhoi", metKhoi);
                        cmd.Parameters.AddWithValue("@TuNgay", ngayBatDau);
                        cmd.Parameters.AddWithValue("@DenNgay", ngayKetThuc);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được sửa thành công trong cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa dữ liệu trong cơ sở dữ liệu: {ex.Message}");
            }

            // Cập nhật lại DataGridView
            string fileXML = "\\HoaDon.xml";
            DataTable dt = loadDataGridView(fileXML);
            dataGridView1.DataSource = dt;
        }


        public void xoa(string _FileXML, string xpathCondition)
        {
            try
            {
                string fileName = Application.StartupPath + _FileXML;
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                // Tìm nút cần xóa
                XmlNode nodeCu = doc.SelectSingleNode(xpathCondition);
                if (nodeCu != null)
                {
                    // Xóa nút
                    doc.DocumentElement.RemoveChild(nodeCu);
                    doc.Save(fileName);
                    MessageBox.Show("Xóa dữ liệu thành công từ file XML!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nút cần xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string idHoaDon = textBoxId.Text.Trim();

            // Kiểm tra xem có nhập IdHoaDon không
            if (string.IsNullOrEmpty(idHoaDon))
            {
                MessageBox.Show("Vui lòng nhập IdHoaDon cần xóa.");
                return;
            }

            // Xóa khỏi file XML
            string filePath = "\\HoaDon.xml";
            string xpathCondition = $"//HoaDon[IdHoaDon='{idHoaDon}']"; // Tìm hóa đơn theo IdHoaDon
            xoa(filePath, xpathCondition);

            // Xóa khỏi cơ sở dữ liệu
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    string sql = "DELETE FROM HoaDon WHERE IdHoaDon = @IdHoaDon";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@IdHoaDon", idHoaDon);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được xóa thành công từ cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu trong cơ sở dữ liệu: {ex.Message}");
            }

            // Cập nhật lại DataGridView
            DataTable dt = loadDataGridView(filePath);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
