using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FahasaApp
{
    public partial class Add_Book : Form
    {
        private AdminForm adminForm;
        public Add_Book(AdminForm adminForm)
        {
            InitializeComponent();
            this.adminForm = adminForm;

            TextBoxBookID.ReadOnly = true;  // Prevent user input

            // Fetch the max ID from the database and increment by 1
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getMaxBookID", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int MaxBookID = reader["MaxBookID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxBookID"]);
                    TextBoxBookID.Text = (MaxBookID + 1).ToString();
                }
            }
        }

        private void TextBoxBookID_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxBookName_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPriceMoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxNumberBook_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Get the input values
            string bookName = TextBoxBookName.Text;
            string priceStr = TextBoxPriceMoney.Text;
            string quantityStr = TextBoxNumberBook.Text;

            // Validate the book name length
            if (bookName.Length < 9)
            {
                MessageBox.Show("Tên sách phải có ít nhất 9 ký tự!");
                return;
            }

            // Validate the price is a number and add the currency
            if (!double.TryParse(priceStr, out double price))
            {
                MessageBox.Show("Giá tiền phải là một số!");
                return;
            }

            // Validate the quantity is a number
            if (!int.TryParse(quantityStr, out int quantity))
            {
                MessageBox.Show("Số lượng phải là một số!");
                return;
            }

            // Insert the new book into the database
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("addBook", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookTitle", bookName);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@BookQuantity", quantity);
                connection.Open();
                int result = command.ExecuteNonQuery();

                // Check Error
                if (result < 0)
                    MessageBox.Show("Error inserting data into Database!");
                else
                {
                    MessageBox.Show("Data has been inserted successfully!");
                    adminForm.LoadData();  // Call the method to refresh the gridview data
                    this.Close();  // Close the current form
                }
            }
        }
    }
}
