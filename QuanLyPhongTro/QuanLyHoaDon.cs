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
using System.Xml.Xsl;
using static System.Windows.Forms.LinkLabel;

namespace QuanLyPhongTro
{
    public partial class QuanLyHoaDon : QuanLyPhongTro.Form1
    {
        TaoXML taoXML = new TaoXML();
        string fileXML = "\\HoaDon.xml";


        void LoadData()
        {
            string sql = "select * from HoaDon";
            taoXML.taoXML(sql, "HoaDon", fileXML);
            dataGridView1.DataSource = taoXML.loadDataGridView(fileXML);

            // Đặt cột Id thành ReadOnly
            if (dataGridView1.Columns.Contains("IdHoaDon"))
            {
                dataGridView1.Columns["IdHoaDon"].ReadOnly = true; // Đảm bảo tên cột trùng khớp
            }
            textBoxId.Enabled = false;

            // Điền dữ liệu vào ComboBox từ bảng Phong
            DataTable dsPhong = GetDanhSachPhong();
            comboBoxPhong.DataSource = dsPhong; // ComboBox bạn đặt trên giao diện
            comboBoxPhong.DisplayMember = "TenPhong"; // Tên phòng hiển thị
            comboBoxPhong.ValueMember = "IdPhong";    // Giá trị là Id phòng
            dataGridView1.CellClick += dataGridView1_CellContentClick;
        }




        string strCon = "Server=DESKTOP-26K2NEP;Initial Catalog=QuanLyPhongTro;Integrated Security=True";

        public QuanLyHoaDon()
        {
            InitializeComponent();
            LoadData();
        }

        private DataTable GetDanhSachPhong()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sql = "SELECT IdPhong, TenPhong FROM Phong"; // Lấy Id và Tên phòng
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, con))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Đưa dữ liệu từ các cột lên TextBox
                textBoxId.Text = row.Cells["IdHoaDon"].Value?.ToString();
                comboBoxPhong.SelectedValue = row.Cells["IdPhong"].Value?.ToString(); // Thiết lập giá trị của ComboBox
                textBoxSoDien.Text = row.Cells["ChuSoDien"].Value?.ToString();
                textBoxMetKhoi.Text = row.Cells["MetKhoi"].Value?.ToString();
                dateTimePicker.Value = Convert.ToDateTime(row.Cells["TuNgay"].Value);
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["DenNgay"].Value);

            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            textBoxId.Text = string.Empty;
            comboBoxPhong.SelectedIndex = -1;
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
            //string phong = textBoxPhong.Text.Trim();
            string phong = comboBoxPhong.SelectedValue.ToString();
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


            taoXML.Them(Application.StartupPath + fileXML, newXml);

            //Lưu vào cơ sở dữ liệu
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

            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox và DateTimePicker
            string idHoaDon = textBoxId.Text.Trim();
            //string phong = textBoxPhong.Text.Trim();
            string phong = comboBoxPhong.SelectedValue.ToString();
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
                taoXML.sua(Application.StartupPath + fileXML, xpathCondition, newXmlContent, "HoaDon");
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

            LoadData();
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

            string xpathCondition = $"//HoaDon[IdHoaDon='{idHoaDon}']"; // Tìm hóa đơn theo IdHoaDon

            taoXML.xoa(fileXML, xpathCondition);
            taoXML.Xoa_Database("HoaDon", "IdHoaDon", textBoxId.Text);
         
            LoadData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHTML_Click(object sender, EventArgs e)
        {
            try
            {
                // Hộp thoại lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML files (*.html)|*.html",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = "DanhSachHoaDon.html" // Tên file mặc định
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Đường dẫn file XML và XSL
                    string xmlPath = Application.StartupPath + "\\HoaDon.xml";
                    string xsltPath = Application.StartupPath + "\\HoaDon.xsl";

                    // Kiểm tra tệp XML và XSL có tồn tại không
                    if (!File.Exists(xmlPath))
                    {
                        MessageBox.Show("File XML không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!File.Exists(xsltPath))
                    {
                        MessageBox.Show("File XSL không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo XslCompiledTransform để thực hiện chuyển đổi
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xsltPath); // Nạp file XSL

                    // Chuyển đổi từ XML sang HTML và lưu tại đường dẫn được chọn
                    using (XmlReader reader = XmlReader.Create(xmlPath))
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            xslt.Transform(reader, null, writer);
                        }
                    }

                    MessageBox.Show("Xuất danh sách hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file HTML sau khi xuất
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất danh sách hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
