using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FahasaApp
{
    public partial class Edit_NhanVien : Form
    {
        public AdminFrom_NhanVien adminForm;  // Add this
        private int staffId;

        public Edit_NhanVien(AdminFrom_NhanVien adminForm, int accountId)
        {
            InitializeComponent();
            this.adminForm = adminForm;

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.TextBox_ID.TextChanged += TextBox_ID_TextChanged;
            TextBox_ID.Text = accountId.ToString();
            LoadStaffData(accountId);
        }

        public void TextBox_ID_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(TextBox_ID.Text, out staffId))
            {
                LoadStaffData(staffId);
            }
        }

        public void LoadStaffData(int staffId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getAccountsByPrivilige", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Privilige", 1); // Assuming the privilege for staff is 1
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int currentStaffId = Convert.ToInt32(reader["STAFFID"]);
                    if (currentStaffId == staffId)
                    {
                        TextBox_ID.Text = currentStaffId.ToString(); // Trả lại ID vào ô mã nhân viên
                        string fullName = reader["Họ tên"].ToString();
                        string[] nameParts = fullName.Split(' ');
                        TextBoxFirstName.Text = nameParts[0]; // Lấy phần họ là FirstName
                        TextBoxLastName.Text = string.Join(" ", nameParts.Skip(1)); // Lấy phần tên đệm và tên là LastName
                        TextBoxEmail.Text = reader["Email"].ToString();
                        TextBoxPhone.Text = reader["Phone"].ToString();
                        TextBoxAddress.Text = reader["Address"].ToString();
                        break;
                    }
                }
                reader.Close();
            }
        }


        private void TextBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdapt_Click_1(object sender, EventArgs e)
        {
            string staffID = TextBox_ID.Text;
            string firstName = TextBoxFirstName.Text;
            string lastName = TextBoxLastName.Text;
            string email = TextBoxEmail.Text;
            string phone = TextBoxPhone.Text;
            string address = TextBoxAddress.Text;

            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Invalid email format!");
                return;
            }

            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("Invalid phone number format! It should start with a '0' and be 10 digits long.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("updateStaff", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StaffID", staffID);
                command.Parameters.AddWithValue("@Firstname", firstName);
                command.Parameters.AddWithValue("@Lastname", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Address", address);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    MessageBox.Show("Error updating data into Database!");
                else
                {
                    MessageBox.Show("Data has been updated successfully!");
                    adminForm.LoadData();
                    this.Close();
                }
            }
        }
    }
}
