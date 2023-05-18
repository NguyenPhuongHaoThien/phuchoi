using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FahasaApp
{
    public partial class Edit_Book : Form
    {
        private AdminForm adminForm;  // Add this
        private int bookId;
        public Edit_Book(AdminForm adminForm, int bookId)
        {
            InitializeComponent();
            this.bookId = bookId;
            this.adminForm = adminForm;
            TextBoxBookID.TextChanged += TextBoxBookID_TextChanged;
            LoadBookData(bookId);
        }

        private void TextBoxBookID_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(TextBoxBookID.Text, out bookId))
            {
                LoadBookData(bookId);
            }
        }

        private void LoadBookData(int bookId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getBookByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", bookId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TextBoxBookName.Text = reader["BookTitle"].ToString();
                    TextBoxPriceMoney.Text = ((decimal)reader["Price"]).ToString("F2", CultureInfo.InvariantCulture) + " vnđ"; // Append " vnđ" after price
                    TextBoxNumberBook.Text = reader["BookQuantity"].ToString();
                }
                else
                {
                    TextBoxBookName.Clear();
                    TextBoxPriceMoney.Clear();
                    TextBoxNumberBook.Clear();
                }
            }
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string bookName = TextBoxBookName.Text;
            string priceStr = TextBoxPriceMoney.Text.Replace(" vnđ", ""); // Remove " vnđ" before parsing
            string quantityStr = TextBoxNumberBook.Text;

            if (bookName.Length < 9)
            {
                MessageBox.Show("Tên sách phải có ít nhất 9 ký tự!");
                return;
            }

            if (!decimal.TryParse(priceStr, out decimal price))
            {
                MessageBox.Show("Giá tiền phải là một số!");
                return;
            }

            if (!int.TryParse(quantityStr, out int quantity))
            {
                MessageBox.Show("Số lượng phải là một số!");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("updateBookByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookID", bookId);
                command.Parameters.AddWithValue("@BookTitle", bookName);
                command.Parameters.AddWithValue("@Price", price); // Pass price as number
                command.Parameters.AddWithValue("@BookQuantity", quantity);
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
