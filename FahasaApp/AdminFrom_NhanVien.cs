using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FahasaApp
{
    public partial class AdminFrom_NhanVien : Form
    {
        public AdminFrom_NhanVien()
        {
            InitializeComponent();
            LoadData();
            dataGridView_nhanvien.RowPrePaint += dataGridView_nhanvien_RowPrePaint;
            dataGridView_nhanvien.CellPainting += dataGridView_nhanvien_CellPainting;
            dataGridView_nhanvien.EnableHeadersVisualStyles = false;
        }


        public void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("getAccountsByPrivilige", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Privilige", 1));  // Assuming the privilege for staff is 1
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView_nhanvien.DataSource = dataTable;

                if (dataGridView_nhanvien.DataSource != null)
                {
                    // Remove the first column (RowHeader column)
                    dataGridView_nhanvien.RowHeadersVisible = false;

                    // Set the fill mode
                    dataGridView_nhanvien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    foreach (DataGridViewColumn col in dataGridView_nhanvien.Columns)
                    {
                        if (col.Name != "STAFFID" && col.Name != "Họ tên" && col.Name != "Email" && col.Name != "Phone" && col.Name != "Address")
                        {
                            col.Visible = false;
                        }
                    }
                    foreach (DataGridViewColumn col in dataGridView_nhanvien.Columns)
                    {
                        if (col.Name != "STAFFID" && col.Name != "Họ tên" && col.Name != "Email" && col.Name != "Phone" && col.Name != "Address")
                        {
                            col.Visible = false;
                        }
                    }


                    dataGridView_nhanvien.Columns["STAFFID"].HeaderText = "Mã NV";
                    dataGridView_nhanvien.Columns["Họ tên"].HeaderText = "Họ Tên";
                    dataGridView_nhanvien.Columns["Email"].HeaderText = "Email";
                    dataGridView_nhanvien.Columns["Phone"].HeaderText = "Số Điện Thoại";
                    dataGridView_nhanvien.Columns["Address"].HeaderText = "Địa Chỉ";

                    // Define the fill weights for columns
                    dataGridView_nhanvien.Columns["STAFFID"].FillWeight = 7;
                    dataGridView_nhanvien.Columns["Họ tên"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Email"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Phone"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Address"].FillWeight = 23;
                    // Center align for header cells and some specific columns
                    foreach (DataGridViewColumn col in dataGridView_nhanvien.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (col.Name == "STAFFID" || col.Name == "Phone")
                        {
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }



            private void dataGridView_nhanvien_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            if (e.RowIndex % 2 == 0)
                dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            else
                dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(239, 212, 212);

            dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

        }

        private void dataGridView_nhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
        }
        private void dataGridView_nhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                e.CellStyle.BackColor = Color.FromArgb(201, 33, 39);
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            }
            if (e.ColumnIndex == -1 && e.RowIndex != -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            }
            dataGridView_nhanvien.GridColor = Color.White;
        }

        private async void XoaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView_nhanvien.CurrentRow != null)
            {
                int staffID = Convert.ToInt32(dataGridView_nhanvien.CurrentRow.Cells["StaffID"].Value);

                DataTable dataTable = (DataTable)dataGridView_nhanvien.DataSource;
                DataRow rowToDelete = dataTable.AsEnumerable().FirstOrDefault(row => Convert.ToInt32(row["StaffID"]) == staffID);
                if (rowToDelete != null)
                    dataTable.Rows.Remove(rowToDelete);

                string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Assuming you have a stored procedure named deleteStaff
                    SqlCommand command = new SqlCommand("deleteStaff", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StaffID", staffID);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa dữ liệu thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu để xóa!");
                    }
                }

                await Task.Delay(2000);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!");
            }
        }


        private void ThemBtn_Click(object sender, EventArgs e)
        {
            Add_NhanVien a = new Add_NhanVien(this);
            a.ShowDialog();
        }

        private void SuaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView_nhanvien.CurrentRow != null)
            {
                int accountID = Convert.ToInt32(dataGridView_nhanvien.CurrentRow.Cells["StaffID"].Value);

                // Tạo một instance của Edit_NhanVien và truyền ID vào
                Edit_NhanVien editForm = new Edit_NhanVien(this, accountID);
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!");
            }
        }

        private void AdminFrom_NhanVien_Load(object sender, EventArgs e)
        {

        }
    }
}
