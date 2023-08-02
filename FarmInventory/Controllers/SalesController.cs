using FarmInventory.Models;
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
    internal class SalesController
    {
        // Instance variables
        private List<CartItem> cartItems;   // items in customer's cart
        private List<Product> products;     // products in inventory 
        private Dispatcher dispatcher;      // manage multi-threading and updating user interface

        // Constructor
        public SalesController(Dispatcher dispatcher) 
        {
            cartItems = new List<CartItem>();
            products = new List<Product>();
            this.dispatcher = dispatcher;
        }

        // Method to calculate the subtotal of each selected product
        public decimal calcSubtotal(double amount, decimal pricePerKg)
        {
            return (decimal)amount * pricePerKg;
        }

        // Method to calculate the total price of cart items
        public decimal calcFinalTotal()
        {
            decimal total = 0;

            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.Invoke(() =>
            {
                foreach (CartItem item in cartItems)
                {
                    total += item.subtotal; // accumulate the subtotal of each item
                }
            });
            return total;
        }

        // Populate the list box with the products from database
        public List<Product> GetProductsFromDatabase()
        {
            List<Product> products = new List<Product>(); // initialize a list for the product objects

            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                {
                    string query = "SELECT * FROM products ORDER BY name"; // select all rows in products table in database

                    cmd.CommandText = query; // assign query string to command property

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd); // adapter passes cmd as a parameter
                    DataTable dataTable = new DataTable(); // store the results retrieved from database
                    dataAdapter.Fill(dataTable); // fill the dataTable with retreieve data

                    // dataTable is converted to enumerable sequence of rows
                    products = dataTable.AsEnumerable()
                        .Select(row => new Product( // project each row of dataTable into new Product object
                            row.Field<string>("name"), // access field value from each row using Field method
                            row.Field<int>("id"),
                            row.Field<double>("amount"),
                            row.Field<decimal>("price")
                            )) // return IEnumerable<Product>
                        .ToList(); // convert IEnumerable<Product> to List<Product> and store in products variable
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return products;
        }

        // Method to add cart item to the List<CartItem>
        public void AddCartItem (CartItem cartItem)
        {
            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.Invoke(() =>
            {
                cartItems.Add(cartItem); // add cart item to cartItems list
            });
        }

        // Method to get information of item in customer's cart 
        public List<CartItem> GetCartItems()
        {
            List<CartItem > cartItemsCopy = new List<CartItem>();

            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.Invoke(() =>
            {
                // project CartItem object in cartItems list into a CartItem object with same property values
                // to make sure the returned list contains seperate instances of CartItem objects so that the original/previous object is not affected
                cartItemsCopy = cartItems.Select(item => new CartItem // project each item of cartItems list into new CartItem object
                {
                    // assign values of CartItem from corresponding properties 
                    productName = item.productName,
                    productId = item.productId,
                    amountPurchased = item.amountPurchased,
                    pricePerKg = item.pricePerKg,
                    subtotal = item.subtotal
                }).ToList(); // convert IEnumerable<Product> to List<Product> and store in products variable

            });
            return cartItemsCopy;
        }

        // Method to update the product amount in inventory after customer made the purchase
        public void UpdateInventory()
        {
            foreach (CartItem item in cartItems) // for each CartItem object in the cartItems list
            {
                // Use try-catch block to handle exception(s) that may occur during database operations
                try
                {
                    using (var cmd = DbConnection.CreateCommand()) // establish singleton database connection and create command object (cmd)
                    {
                        string query = "UPDATE products SET amount = amount - @amountPurchased WHERE id = @id"; // deduct the amount of product with ceratin id
                        cmd.CommandText = query; // assign query string to command property
                        cmd.Parameters.AddWithValue("@amountPurchased", item.amountPurchased);  // 1st parameter placeholder @amountPurchased of query: amountPurchased value of CartItem 
                        cmd.Parameters.AddWithValue("@id", item.productId);                     // 2nd parameter placeholder @id of query: productId value of CartItem 

                        cmd.ExecuteNonQuery(); // execute the update query
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            // Fetch the updated inventory amounts from database
            List<Product> updatedProducts = GetProductsFromDatabase();

            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.Invoke(() =>
            {
                products = updatedProducts; // Update the products list with the updated inventory amounts
                cartItems.Clear(); // clear the cart after confirming order
            });
        }
    }
}
