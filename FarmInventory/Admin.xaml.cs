using FarmInventory.Controllers;
using FarmInventory.Models;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FarmInventory
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private ProductController productController;
        public Admin()
        {
            InitializeComponent();
            Loaded += AdminLoaded;
            productController = new ProductController(); 
        }

        // Show data once the Admin window is opened
        private void AdminLoaded(object sender, RoutedEventArgs e)
        {
            ShowAllData();
        }

        // Thread for insert operation
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            Thread insertThread = new Thread(() =>
            {
                string productName = string.Empty;
                int productId = 0;
                double amountKg = 0;
                decimal pricePerKg = 0;

                Dispatcher.Invoke(() =>
                {
                    productName = tbProductName.Text;
                    productId = int.Parse(tbProductID.Text);
                    amountKg = double.Parse(tbAmountKg.Text);
                    pricePerKg = decimal.Parse(tbPricePerKg.Text);
                });
                productController.InsertData(productName, productId, amountKg, pricePerKg);
                ShowAllData() ;
            });
            insertThread.Start();
        }

        // Thread for show all operation
        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            Thread showAllThread = new Thread(ShowAllData);
            showAllThread.Start();
        }
        
        // Thread for search operation
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearchId.Text))
            {
                MessageBox.Show("Please enter a product ID.");
                return;
            }
            int searchId = int.Parse(tbSearchId.Text);
            Thread searchThread = new Thread(() => 
            {
                /*int id = searchId; // create a local copy of the search ID*/
                List<Product> searchResults = productController.SearchData(searchId);

                Dispatcher.Invoke(() => 
                {
                    if (searchResults != null && searchResults.Any())
                    {
                        dataView.ItemsSource = searchResults;
                    }
                    else
                    {
                        dataView.ItemsSource = null;
                        MessageBox.Show("No result found.");
                    }
                });
            });
            searchThread.Start();
        }

        // Thread for delete operation
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected row from the DataGrid
            Product selectedRow = dataView.SelectedItem as Product;
            if (selectedRow == null)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            // Get the ID of the selected row
            int selectedId = selectedRow.id;

            // Delete a new thread to perform the deletion
            /*int selectedId = int.Parse(tbProductID.Text);*/
            Thread deleteThread = new Thread(() => 
            {
                productController.DeleteData(selectedId);
                ShowAllData();
            });
            deleteThread.Start();
        }

        private void ShowAllData()
        {
            List<Product> products = productController.GetAllData();
            Dispatcher.Invoke(() => UpdateUIWithProductList(products));
        }

        private void UpdateUIWithProductList(List<Product> products)
        {
            Dispatcher.Invoke(() =>
            {
                dataView.ItemsSource = products;
            });
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            // Get the product ID from the text box
            if (!int.TryParse(tbIdUpdate.Text, out int productId))
            {
                MessageBox.Show("Invalid product ID.");
                return;
            }

            // Get the amount to add from the text box
            if (!double.TryParse(tbAmountKgUpdate.Text, out double amountToAdd))
            {
                MessageBox.Show("Invalid amount.");
                return;
            }

            // Create a new thread to perform the addition
            Thread addAmountThread = new Thread(() =>
            {
                productController.AddAmount(productId, amountToAdd);
                ShowAllData();
            });
            addAmountThread.Start();
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            // Get the product ID from the text box
            if (!int.TryParse(tbIdUpdate.Text, out int productId))
            {
                MessageBox.Show("Invalid product ID.");
                return;
            }

            // Get the amount to add from the text box
            if (!double.TryParse(tbAmountKgUpdate.Text, out double amountToMinus))
            {
                MessageBox.Show("Invalid amount.");
                return;
            }

            // Create a new thread to perform the addition
            Thread minusAmountThread = new Thread(() =>
            {
                productController.MinusAmount(productId, amountToMinus);
                ShowAllData();
            });
            minusAmountThread.Start();
        }

        /* // Method to insert new product
         private void InsertData()
         {
             try
             {
                 using (var cmd = DbConnection.CreateCommand()) // singleton database connection
                 {
                     // Create sql query
                     string query = "INSERT INTO products values(@name, default, @amountKg, @pricePerKg)";

                     // Initialize the value of parameters in the sql query
                     string productName = "";
                     double amountKg = 0.0;
                     double pricePerKg = 0.0;

                     // Wrap UI-related code inside Dispatcher.Invoke to guarantee it will be executed on the UI thread
                     Dispatcher.Invoke(() =>
                     {
                         productName = tbProductName.Text;
                         amountKg = double.Parse(tbAmountKg.Text);
                         pricePerKg = double.Parse(tbPricePerKg.Text);
                     });

                     // Set the command text
                     cmd.CommandText = query;

                     // Add the values for the parameters in the sql query
                     cmd.Parameters.AddWithValue("@name", productName);
                     //cmd.Parameters.AddWithValue("@id", int.Parse("DEFAULT"));
                     cmd.Parameters.AddWithValue("@amountKg", amountKg);
                     cmd.Parameters.AddWithValue("@pricePerKg", pricePerKg);

                     // Execute the Query
                     cmd.ExecuteNonQuery();

                     // Execute on the UI thread
                     Dispatcher.Invoke(() =>
                     {
                         MessageBox.Show("Data Insertion Successful.");
                     });
                 }
             }
             catch (NpgsqlException ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }

         // Method to show all products
         private void ShowAllData()
         {
             try
             {
                 using (var cmd = DbConnection.CreateCommand())
                 {
                     // Create sql query
                     string query = "SELECT * FROM products";

                     // Set the command text
                     cmd.CommandText = query;

                     // SqlDataRead adapter and create sql DataTable
                     NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                     DataTable dataTable = new DataTable();
                     dataAdapter.Fill(dataTable);

                     Dispatcher.Invoke(() =>
                     {
                         // pass the values from database to dataView DataGrid
                         dataView.ItemsSource = dataTable.AsDataView();
                         DataContext = dataAdapter;
                     });
                 }
             }
             catch (NpgsqlException ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }

         // Action for when the Search button is clicked
         private void SearchData()
         {
             try
             {
                 string searchId = string.Empty;

                 // Execute on the UI thread
                 Dispatcher.Invoke(() =>
                 {
                     searchId = tbSearchId.Text;
                 });

                 //DataTable dataTable = new DataTable();
                 using (var cmd = DbConnection.CreateCommand())
                 {
                     // Create sql query
                     string query = "SELECT * FROM products WHERE id=@ID";

                     // Create sql command
                     cmd.CommandText = query;
                     cmd.Parameters.AddWithValue("@ID", int.Parse(searchId));

                     // SqlDataRead adapter and create sql DataTable
                     NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                     DataTable dataTable = new DataTable();
                     dataAdapter.Fill(dataTable);

                     // Wrap UI-related code inside Dispatcher.Invoke to guarantee it will be executed on the UI thread
                     Dispatcher.Invoke(() =>
                     {
                         // pass the values from database to dataView DataGrid
                         dataView.ItemsSource = dataTable.AsDataView();
                     });
                 }
             }
             catch (NpgsqlException ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }

         // Method to delete a row from the database
         private void DeleteData(int id)
         {
             try
             {
                 using (var cmd = DbConnection.CreateCommand())
                 {
                     // Create sql query
                     string query = "DELETE FROM products WHERE id = @ID";

                     // Set the command text
                     cmd.CommandText = query;

                     // Add the parameter for the ID
                     cmd.Parameters.AddWithValue("@ID", id);

                     // Execute the query
                     cmd.ExecuteNonQuery();

                     Dispatcher.Invoke(() =>
                     {
                         MessageBox.Show("Data deletion successful.");
                         // Remove the selected row from the DataGrid
                         //DataRowView selectedRow = dataView.SelectedItem as DataRowView;
                         ShowAllData();
                     });
                 }
             }
             catch (NpgsqlException ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }*/

    }
}
