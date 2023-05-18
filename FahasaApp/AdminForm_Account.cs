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
    public partial class AdminForm_Account : Form
    {
        public AdminForm_Account()
        {
            InitializeComponent();
            LoadData();
            dataGridView_account.RowPrePaint += dataGridView_account_RowPrePaint;
            dataGridView_account.CellPainting += dataGridView_account_CellPainting;
            dataGridView_account.EnableHeadersVisualStyles = false;
        }

        public void LoadData()
        {
            SqlConnection conn = new SqlConnection(Program.getConnectString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("[getAllAccount]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            dataGridView_account.DataSource = dt;

            if (dataGridView_account.DataSource != null)
            {
                dataGridView_account.RowHeadersVisible = false;
                dataGridView_account.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Ẩn các cột không cần thiết
                foreach (DataGridViewColumn col in dataGridView_account.Columns)
                {
                    if (col.Name != "AccountID" && col.Name != "Email" && col.Name != "Privilige" && col.Name != "Phone" && col.Name != "Password")
                    {
                        col.Visible = false;
                    }
                }

                dataGridView_account.Columns["AccountID"].HeaderText = "Account ID";
                dataGridView_account.Columns["Email"].HeaderText = "Email";
                dataGridView_account.Columns["Privilige"].HeaderText = "Privilege";
                dataGridView_account.Columns["Phone"].HeaderText = "Phone";
                dataGridView_account.Columns["Password"].HeaderText = "Password";

                dataGridView_account.Columns["AccountID"].FillWeight = 7;
                dataGridView_account.Columns["Email"].FillWeight = 20;
                dataGridView_account.Columns["Privilige"].FillWeight = 7;
                dataGridView_account.Columns["Phone"].FillWeight = 20;
                dataGridView_account.Columns["Password"].FillWeight = 19;

                foreach (DataGridViewColumn col in dataGridView_account.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (col.Name == "AccountID" || col.Name == "Phone" || col.Name == "Privilige")
                    {
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
        }


        private void AdminForm_Account_Load(object sender, EventArgs e)
        {

        }

        private void ThemBtn_Click(object sender, EventArgs e)
        {
            Add_Account a = new Add_Account(this);
            a.ShowDialog();
        }

        private bool isDeleting = false;

        private async void XoaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView_account.CurrentRow != null)
            {
                int accountID = Convert.ToInt32(dataGridView_account.CurrentRow.Cells["AccountID"].Value);

                DataTable dataTable = (DataTable)dataGridView_account.DataSource;
                DataRow rowToDelete = dataTable.AsEnumerable().FirstOrDefault(row => Convert.ToInt32(row["AccountID"]) == accountID);
                if (rowToDelete != null)
                    dataTable.Rows.Remove(rowToDelete);

                string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Assuming you have a stored procedure named deleteStaff
                    SqlCommand command = new SqlCommand("deleteAccount", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AccountID", accountID);
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
            if (dataGridView_account.CurrentRow != null)
            {
                int accountID = Convert.ToInt32(dataGridView_account.CurrentRow.Cells["AccountID"].Value);
                Edit_Account a = new Edit_Account(this, accountID);
                a.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!");
            }
        }

        private void dataGridView_account_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
        }

        private void dataGridView_account_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView_account.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            if (e.RowIndex % 2 == 0)
                dataGridView_account.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            else
                dataGridView_account.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(239, 212, 212);

            dataGridView_account.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }

        private void dataGridView_account_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
            dataGridView_account.GridColor = Color.White;
        }
    }
}
