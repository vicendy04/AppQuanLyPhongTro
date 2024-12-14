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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo instance của class TaoXML
                TaoXML taoXMLInstance = new TaoXML();

                // Câu lệnh SQL truy vấn (bạn có thể thay bằng bảng thật)
                string sql = "SELECT * FROM Phong"; // Đổi 'Products' thành tên bảng trong database của bạn
                string bang = "Phong"; // Tên bảng
                string _FileXML = "\\data_output.xml"; // Tên file XML

                // Gọi hàm taoXML
                taoXMLInstance.taoXML(sql, bang, _FileXML);

                // Hiển thị thông báo thành công
                MessageBox.Show("Tạo file XML thành công tại: " + Application.StartupPath + _FileXML,
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Có lỗi xảy ra khi tạo XML: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
