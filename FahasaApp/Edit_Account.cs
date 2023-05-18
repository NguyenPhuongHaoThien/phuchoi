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
    public partial class Edit_Account : Form
    {
        private AdminForm_Account adminForm;
        private int accountId1;

        public Edit_Account(AdminForm_Account adminForm, int accountId)
        {
            InitializeComponent();
            this.adminForm = adminForm;

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.TextBox_AccountID.TextChanged += TextBox_AccountID_TextChanged;
            TextBox_AccountID.Text = accountId.ToString();
            LoadAccountData(accountId);
        }


        private void TextBox_AccountID_TextChanged(object sender, EventArgs e)
        {
            int accountId;
            if (int.TryParse(TextBox_AccountID.Text, out accountId))
            {
                LoadAccountData(accountId1);
            }
        }

        private void LoadAccountData(int accountId1)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[getAllAccount]", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int currentAccountId = Convert.ToInt32(reader["AccountID"]);
                    if (currentAccountId == accountId1)
                    {
                        TextBox_AccountID.Text = currentAccountId.ToString(); // Trả lại ID vào ô mã nhân viên
                        TextBoxEmail.Text = reader["Email"].ToString();
                        TextBoxPhone.Text = reader["Phone"].ToString();
                        TextBoxPassword.Text = reader["Password"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string lastName = reader["LastName"].ToString();
                        TextBoxHoTen.Text = $"{firstName} {lastName}"; // Hiển thị dữ liệu bằng FirstName và LastName
                        break;
                    }
                }
                reader.Close();
            }
        }


        private void TextBoxDacQuyen_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get the input values
            int accountId = int.Parse(TextBox_AccountID.Text);
            int privilege = int.Parse(TextBoxDacQuyen.Text);
            string email = TextBoxEmail.Text;
            string phone = TextBoxPhone.Text;
            string password = TextBoxPassword.Text;
            string hoTen = TextBoxHoTen.Text;

            // Validate the hoTen
            if (hoTen.Length < 7 || !hoTen.Contains(" "))
            {
                MessageBox.Show("Họ tên phải có ít nhất 7 ký tự và ít nhất 1 dấu cách!");
                return;
            }

            // Split the hoTen into FirstName and LastName
            string[] hoTenSplit = hoTen.Split(new char[] { ' ' }, 2);
            string firstName = hoTenSplit[0];
            string lastName = hoTenSplit.Length > 1 ? hoTenSplit[1] : "";

            // Validate the email
            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Invalid email format!");
                return;
            }

            // Validate the phone number
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("Invalid phone number format! It should start with a '0' and be 10 digits long.");
                return;
            }

            // Update the account in the database
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("updateAccount", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountID", accountId);
                command.Parameters.AddWithValue("@Privilige", privilege);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BoxTextHoTen_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
