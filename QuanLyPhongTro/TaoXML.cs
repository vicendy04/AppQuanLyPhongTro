using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;

namespace QuanLyPhongTro
{
    class TaoXML
    {
        string strCon = @"Server=DESKTOP-26K2NEP;Initial Catalog=QuanLyPhongTro;Integrated Security=True";
        public void taoXML(string sql, string bang, string _FileXML)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable(bang);
            ad.Fill(dt);
            dt.WriteXml(Application.StartupPath + _FileXML, XmlWriteMode.WriteSchema);
        }
        public DataTable loadDataGridView(string _FileXML)
        {
            DataTable dt = new DataTable();
            string FilePath = Application.StartupPath + _FileXML;
            if (File.Exists(FilePath))
            {
                //tao luong xu ly file xml
                FileStream fsReadXML = new FileStream(FilePath, FileMode.Open);
                //doc file xml vao datatable
                dt.ReadXml(fsReadXML);
                fsReadXML.Close();
            }
            else
            {
                MessageBox.Show("File không tồn tại");
            }
            return dt;
        }
        public void Them(string FileXML, string xml)
        {
            try
            {
                XmlTextReader textread = new XmlTextReader(FileXML);
                XmlDocument doc = new XmlDocument();
                doc.Load(textread);
                textread.Close();
                XmlNode currNode;
                XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                docFrag.InnerXml = xml;
                currNode = doc.DocumentElement;
                currNode.InsertAfter(docFrag, currNode.LastChild);
                doc.Save(FileXML);
            }
            catch
            {
                MessageBox.Show("lỗi");
            }
        }

        public void xoa(string _FileXML, string xpath)
        {
            try
            {
                string fileName = Application.StartupPath + _FileXML;
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                // Tìm node cần xóa
                XmlNode nodeToDelete = doc.SelectSingleNode(xpath);
                if (nodeToDelete == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cần xóa!");
                    return;
                }

                // Xóa node
                nodeToDelete.ParentNode.RemoveChild(nodeToDelete);
                doc.Save(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa XML: " + ex.Message);
            }
        }
        public void sua(string FileXML, string sql, string xml, string bang)
        {
            XmlTextReader reader = new XmlTextReader(FileXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            XmlNode oldValue;
            XmlElement root = doc.DocumentElement;
            oldValue = root.SelectSingleNode(sql);
            XmlElement newValue = doc.CreateElement(bang);
            newValue.InnerXml = xml;
            root.ReplaceChild(newValue, oldValue);
            doc.Save(FileXML);
        }
        public void TimKiem(string _FileXML, string xml, DataGridView dgv)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.StartupPath + _FileXML);
            string xPath = xml;
            XmlNode node = xDoc.SelectSingleNode(xPath);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            XmlNodeReader nr = new XmlNodeReader(node);
            ds.ReadXml(nr);
            dgv.DataSource = ds.Tables[0];
            nr.Close();
        }
        public string LayGiaTri(string duongDan, string truongA, string giaTriA, string truongB)
        {
            string giatriB = "";
            DataTable dt = new DataTable();
            dt = loadDataGridView(duongDan);
            int soDongNhanVien = dt.Rows.Count;
            for (int i = 0; i < soDongNhanVien; i++)
            {
                if (dt.Rows[i][truongA].ToString().Trim().Equals(giaTriA))
                {
                    giatriB = dt.Rows[i][truongB].ToString();
                    return giatriB;
                }
            }
            return giatriB;
        }
        public bool KiemTra(string _FileXML, string truongKiemTra, string giaTriKiemTra)
        {
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            dt.DefaultView.RowFilter = truongKiemTra + " ='" + giaTriKiemTra + "'";
            if (dt.DefaultView.Count > 0)
                return true;
            return false;
        }
        public string txtMa(string tienTo, string _FileXML, string tenCot)
        {
            string txtMa = "";
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            int dem = dt.Rows.Count;
            if (dem == 0)
            {
                txtMa = tienTo + "001";//HD001
            }
            else
            {
                int duoi = int.Parse(dt.Rows[dem - 1][tenCot].ToString().Substring(2, 3)) + 1;
                string cuoi = "00" + duoi;
                txtMa = tienTo + "" + cuoi.Substring(cuoi.Length - 3, 3);
            }
            return txtMa;
        }
        public string GetNextId(string _FileXML, string tenCot)
        {
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            int dem = dt.Rows.Count;
            if (dem == 0)
            {
                return "1";
            }
            else
            {
                int maxId = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int currentId = int.Parse(dt.Rows[i][tenCot].ToString());
                    if (currentId > maxId)
                        maxId = currentId;
                }
                return (maxId + 1).ToString();
            }
        }

        public bool KTMa(string _FileXML, string cotMa, string ma)
        {
            bool kt = true;
            DataTable dt = new DataTable();
            dt = loadDataGridView(_FileXML);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][cotMa].ToString().Trim().Equals(ma))
                {
                    kt = false;
                }
                else
                {
                    kt = true;
                }
            }
            return kt;
        }
        public void exCuteNonQuery(string sql)
        {
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            com.ExecuteNonQuery();
        }
        public void Them_Database(string tenBang, string _FileXML)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = table.Rows.Count - 1;

            // Tạo danh sách tên cột và giá trị
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            // Lấy tất cả các cột trừ cột Identity (thường là cột đầu tiên)
            for (int j = 1; j < table.Columns.Count; j++)
            {
                columns.Add(table.Columns[j].ColumnName);
                string value = table.Rows[dong][j].ToString().Trim();

                // Xử lý đặc biệt cho kiểu DateTime
                if (table.Columns[j].DataType == typeof(DateTime))
                {
                    value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                    values.Add("'" + value + "'");
                }
                else
                {
                    values.Add("N'" + value + "'");
                }
            }

            // Tạo câu lệnh SQL
            string sql = $"INSERT INTO {tenBang} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
            exCuteNonQuery(sql);
        }

        public void Sua_Database(string tenBang, string _FileXML, string tenCot, string giaTri)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            int dong = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tenCot].ToString().Trim() == giaTri)
                { dong = i; }
            }
            if (dong > -1)
            {
                string sql = "update " + tenBang + " set ";
                List<string> updates = new List<string>();

                // Bắt đầu từ cột thứ 1 (bỏ qua cột Identity ở index 0)
                for (int j = 1; j < table.Columns.Count; j++)
                {
                    string columnName = table.Columns[j].ToString();
                    string value = table.Rows[dong][j].ToString().Trim();

                    // Xử lý đặc biệt cho ngày tháng
                    if (table.Columns[j].DataType == typeof(DateTime))
                    {
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                        updates.Add($"{columnName} = '{value}'");
                    }
                    else
                    {
                        updates.Add($"{columnName} = N'{value}'");
                    }
                }

                sql += string.Join(", ", updates);
                sql += " where " + tenCot + "= '" + giaTri + "'";
                exCuteNonQuery(sql);
            }
        }
        public void Xoa_Database(string tenBang, string tenCot, string giaTri)
        {
            try
            {
                string sql = $"DELETE FROM {tenBang} WHERE {tenCot} = {giaTri}";
                exCuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa từ database: " + ex.Message);
            }
        }

        public void CapNhapTungBang(string tenBang, string _FileXML)
        {
            string duongDan = _FileXML;
            DataTable table = loadDataGridView(duongDan);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string sql = "insert into " + tenBang + " values(";
                for (int j = 0; j < table.Columns.Count - 1; j++)
                {
                    sql += "N'" + table.Rows[i][j].ToString().Trim() + "',";
                }
                sql += "N'" + table.Rows[i][table.Columns.Count - 1].ToString().Trim() + "'";
                sql += ")";
                exCuteNonQuery(sql);
            }

        }
        public void TimKiemXSLT(string data, string tenFileXML, string tenfileXSLT)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load("" + tenfileXSLT + ".xslt");
            XsltArgumentList argList = new XsltArgumentList();
            argList.AddParam("Data", "", data);
            XmlWriter writer = XmlWriter.Create("" + tenFileXML + ".html");
            xslt.Transform(new XPathDocument("" + tenFileXML + ".xml"), argList, writer);
            writer.Close();
            System.Diagnostics.Process.Start("" + tenFileXML + ".html");
        }
    }

}
