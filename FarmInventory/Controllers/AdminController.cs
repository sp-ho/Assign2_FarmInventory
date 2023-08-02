using FarmInventory.Models;
using GalaSoft.MvvmLight.Messaging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FarmInventory.Controllers
{
    internal class AdminController
    {
        private List<Product> products;

        // create synchronization object that can be used to enforce exclusive access to a section of code.
        // to ensure only one thread can enter the locked section at a time, avoiding competition conditions.
        private readonly object lockObject = new object(); 

        public AdminController()
        {
            products = new List<Product>();
        }

        // done
        // Method to insert a new product into the products table in database
        public void InsertData(string name, int id, double amount, decimal price)
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "INSERT INTO products VALUES (@name, @id, @amount, @price)"; // insert new product to product table

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@name", name);     // 1st parameter placeholder of query: name input passed into this method 
                    cmd.Parameters.AddWithValue("@id", id);         // 2nd parameter placeholder of query: id input passed into this method 
                    cmd.Parameters.AddWithValue("@amount", amount); // 3rd parameter placeholder of query: amount input passed into this method 
                    cmd.Parameters.AddWithValue("@price", price);   // 4th parameter placeholder of query: price input passed into this method 

                    cmd.ExecuteNonQuery(); // execute the insert query

                    MessageBox.Show("Data Insertion Successful.");
                }
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                MessageBox.Show(ex.Message);
            }
        }

        // done
        public List<Product> GetAllData()
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "SELECT * FROM products ORDER BY id ASC"; // select all products, in ascending order based on id

                    cmd.CommandText = query; // assign query string to command property

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    lock (lockObject)
                    {
                        products = dataTable.AsEnumerable()
                            .Select(row => new Product(
                                row.Field<string>("name"),
                                row.Field<int>("id"),
                                row.Field<double>("amount"),
                                row.Field<decimal>("price")
                            ))
                            .ToList();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                MessageBox.Show(ex.Message);
            }

            return products;
        }

        // done
        public List<Product> SearchData(int id)
        {
            List<Product> searchResults = new List<Product>();

            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "SELECT * FROM products WHERE id = @ID";

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@ID", id); // parameter placeholder of query: id input passed into this method

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        searchResults.Add(new Product(
                            row.Field<string>("name"),
                            row.Field<int>("id"),
                            row.Field<double>("amount"),
                            row.Field<decimal>("price")
                        ));
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                MessageBox.Show(ex.Message);
            }

            return searchResults;
        }

        // done
        // Delete a data
        public void DeleteData(int id)
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "DELETE FROM products WHERE id = @ID";

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@ID", id); // parameter placeholder of query: id input passed into this method

                    cmd.ExecuteNonQuery(); // execute the delete query
                }
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                MessageBox.Show(ex.Message);
            }
        }

        // done
        public void AddAmount(int id, double amountToAdd)
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "UPDATE products SET amount = amount + @amountToAdd WHERE id = @id";

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@amountToAdd", amountToAdd); // 1st parameter placeholder of query: amountToAdd input passed into this method
                    cmd.Parameters.AddWithValue("@id", id); // 2nd parameter placeholder of query: id input passed into this method

                    cmd.ExecuteNonQuery (); // execute the insert query
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // done
        public void MinusAmount(int id, double amountToMinus)
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "UPDATE products SET amount = amount - @amountToMinus WHERE id = @id";

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@amountToMinus", amountToMinus); // 1st parameter placeholder of query: amountToMinus input passed into this method
                    cmd.Parameters.AddWithValue("@id", id); // 2nd parameter placeholder of query: id input passed into this method

                    cmd.ExecuteNonQuery(); // execute the insert query
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // done
        public void UpdateAmount(int id, double amountToUpdate)
        {
            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "UPDATE products SET amount = @amountToUpdate WHERE id = @id";

                    cmd.CommandText = query; // assign query string to command property
                    cmd.Parameters.AddWithValue("@amountToUpdate", amountToUpdate); // 1st parameter placeholder of query: amountToUpdate input passed into this method
                    cmd.Parameters.AddWithValue("@id", id); // 2nd parameter placeholder of query: id input passed into this method

                    cmd.ExecuteNonQuery(); // execute the insert query
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
