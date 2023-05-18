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
    public partial class AdminForm_Customer : Form
    {
        public AdminForm_Customer()
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
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Privilige", 0); // Assuming the privilege for staff is 1
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
                        if (col.Name != "USERID" && col.Name != "Họ tên" && col.Name != "Email" && col.Name != "Phone" && col.Name != "Address")
                        {
                            col.Visible = false;
                        }
                    }


                    dataGridView_nhanvien.Columns["USERID"].HeaderText = "Mã KH";
                    dataGridView_nhanvien.Columns["Họ tên"].HeaderText = "Họ Tên";
                    dataGridView_nhanvien.Columns["Email"].HeaderText = "Email";
                    dataGridView_nhanvien.Columns["Phone"].HeaderText = "Số Điện Thoại";
                    dataGridView_nhanvien.Columns["Address"].HeaderText = "Địa Chỉ";

                    // Define the fill weights for columns
                    dataGridView_nhanvien.Columns["USERID"].FillWeight = 7;
                    dataGridView_nhanvien.Columns["Họ tên"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Email"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Phone"].FillWeight = 23;
                    dataGridView_nhanvien.Columns["Address"].FillWeight = 23;
                    // Center align for header cells and some specific columns
                    foreach (DataGridViewColumn col in dataGridView_nhanvien.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (col.Name == "USERID" || col.Name == "Phone")
                        {
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }

        private void ThemBtn_Click(object sender, EventArgs e)
        {
            Add_Customer a = new Add_Customer(this);
            a.ShowDialog();
        }

        private async void XoaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView_nhanvien.CurrentRow != null)
            {
                int userID = Convert.ToInt32(dataGridView_nhanvien.CurrentRow.Cells["UserID"].Value);

                DataTable dataTable = (DataTable)dataGridView_nhanvien.DataSource;
                DataRow rowToDelete = dataTable.AsEnumerable().FirstOrDefault(row => Convert.ToInt32(row["UserID"]) == userID);
                if (rowToDelete != null)
                    dataTable.Rows.Remove(rowToDelete);

                string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Assuming you have a stored procedure named deleteStaff
                    SqlCommand command = new SqlCommand("deleteCustomer", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", userID);
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

        private void SuaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView_nhanvien.CurrentRow != null)
            {
                int accountID = Convert.ToInt32(dataGridView_nhanvien.CurrentRow.Cells["UserID"].Value);

                // Tạo một instance của Edit_NhanVien và truyền ID vào
                Edit_Customer editForm = new Edit_Customer(this, accountID);
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!");
            }
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

        private void dataGridView_nhanvien_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            if (e.RowIndex % 2 == 0)
                dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            else
                dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(239, 212, 212);

            dataGridView_nhanvien.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

        }

        private void AdminForm_Customer_Load(object sender, EventArgs e)
        {

        }
    }
}
