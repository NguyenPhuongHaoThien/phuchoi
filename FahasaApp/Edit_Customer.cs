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
    public partial class Edit_Customer : Form
    {
        private AdminForm_Customer adminForm;  // Add this
        private int customerID;
        public Edit_Customer(AdminForm_Customer adminForm, int accountID)  // Modify this
        {
            InitializeComponent();
            this.adminForm = adminForm;

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.TextBox_ID.TextChanged += TextBox_ID_TextChanged;
            TextBox_ID.Text = accountID.ToString();
            LoadCustomerData(accountID);
        }

        private void TextBox_ID_TextChanged(object sender, EventArgs e)
        {
            int accountId;
            if (int.TryParse(TextBox_ID.Text, out accountId))
            {
                LoadCustomerData(customerID);
            }
        }
        private void LoadCustomerData(int customerID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getAccountsByPrivilige", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Privilige", 0); // Assuming the privilege for staff is 1
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int currentCustomerID = Convert.ToInt32(reader["USERID"]);
                    if (currentCustomerID == customerID)
                    {
                        TextBox_ID.Text = currentCustomerID.ToString(); // Trả lại ID vào ô mã nhân viên
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string customerID = TextBox_ID.Text;
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
                SqlCommand command = new SqlCommand("updateCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", customerID);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
