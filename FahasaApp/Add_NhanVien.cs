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
    public partial class Add_NhanVien : Form
    {
        private AdminFrom_NhanVien adminForm;  // Add this

        public Add_NhanVien(AdminFrom_NhanVien adminForm)  // Modify this
        {
            InitializeComponent();
            this.adminForm = adminForm;

            TextBox_AccountID.ReadOnly = true;

            string connectionString = Program.getConnectString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getMaxCustomerID", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int maxCustomerID = reader["MaxCustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxCustomerID"]);
                    TextBox_AccountID.Text = (maxCustomerID + 1).ToString();
                }
            }

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        }

        private void TextBox_ID_TextChanged(object sender, EventArgs e)
        {

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
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

            string password = "default_password";
            int privilege = 1;

            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("addStaff", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Firstname", firstName);
                command.Parameters.AddWithValue("@Lastname", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Privilige", privilege);
                command.Parameters.AddWithValue("@Address", address);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                    MessageBox.Show("Error inserting data into Database!");
                else
                {
                    MessageBox.Show("Data has been inserted successfully!");

                    adminForm.LoadData();
                    this.Close();
                }
            }
        }
    }
}
