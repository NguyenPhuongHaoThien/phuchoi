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
    public partial class Add_Bill : Form
    {
        private AdminForm_Bill adminForm;
        public Add_Bill(AdminForm_Bill adminForm)
        {
            InitializeComponent();
            this.adminForm = adminForm;

            TextBox_ID_HD.ReadOnly = true;  // Prevent user input

            // Fetch the max ID from the database and increment by 1
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("getMaxOrderID", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                int maxOrderID = Convert.ToInt32(command.ExecuteScalar());
                TextBox_ID_HD.Text = (maxOrderID + 1).ToString();
            }
        }


        private void TextBox_ID_HD_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxID_DatHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxTotalMoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPaymentID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Get the input values
            DateTime orderDate = DateTime.ParseExact(TextBoxDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int customerID = Convert.ToInt32(TextBoxID_DatHang.Text);
            float totalPrice = float.Parse(TextBoxTotalMoney.Text);
            int paymentID = Convert.ToInt32(TextBoxPaymentID.Text);

            // Insert the new order into the database using a stored procedure
            string connectionString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("addOrder", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderDate", orderDate);
                command.Parameters.AddWithValue("@CustomerID", customerID);
                command.Parameters.AddWithValue("@TotalPrice", totalPrice);
                command.Parameters.AddWithValue("@PaymentID", paymentID);
                SqlParameter newOrderIDParam = new SqlParameter("@NewOrderID", SqlDbType.Int);
                newOrderIDParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(newOrderIDParam);

                command.ExecuteNonQuery();

                connection.Close();

                MessageBox.Show("Data has been inserted successfully!");
                adminForm.LoadData();  // Refresh the data in the parent form
                this.Close();  // Close the current form
            }
        }
    }
}
