using Npgsql;
using System.Data.Common;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;

namespace FarmInventoryREST.Models
{
    public class Applications
    {

        /* Hold all CRUD related operations we are going to apply in our database/remote server.
                 * It generates the requests, get the Response and then send that response back to the 
                 * controller whenever called
                 * 
                 */
        private List<CartItem> cartItems;   // items in customer's cart
        private List<Product> products;     // products in inventory 
        private Dispatcher dispatcher;

        // Constructor
     /*   public Applications(Dispatcher dispatcher)
        {
            cartItems = new List<CartItem>();
            products = new List<Product>();
            this.dispatcher = dispatcher;
        }*/

        public ProductResponse InsertData(NpgsqlConnection con, Product product)
        {
            /* in each method under Application class,
             * we will send the data value from the controller class 
             * and then will be added those data information in the server/database.
             * we need the connection string of the remote database as well as the 
             * data information 
             */
            ProductResponse response = new ProductResponse();
            try
            {
                string query = "INSERT INTO products VALUES (@name, @id, @amount, @price)"; // insert new product to product table
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign query string to command property
                cmd.Parameters.AddWithValue("@name", product.name);     // 1st parameter placeholder of query: name input passed into this method 
                cmd.Parameters.AddWithValue("@id", product.id);         // 2nd parameter placeholder of query: id input passed into this method 
                cmd.Parameters.AddWithValue("@amount", product.amount); // 3rd parameter placeholder of query: amount input passed into this method 
                cmd.Parameters.AddWithValue("@price", product.price);   // 4th parameter placeholder of query: price input passed into this method 
                con.Open();

                int i = cmd.ExecuteNonQuery(); // execute the insert query

                if (i > 0)
                {
                    response.statusCode = 200; // it is successful
                    response.message = "Product is added successfully";
                    response.product = product;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100; // it is fail
                    response.message = "Nothing is added.";
                    response.product = null;
                    response.products = null;
                }
                
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public ProductResponse GetAllData(NpgsqlConnection con)
        {
            /* in each method under Application class,
             * we will send the data value from the controller class 
             * and then will be added those data information in the server/database.
             * we need the connection string of the remote database as well as the 
             * data information 
             */
            ProductResponse response = new ProductResponse();

            try
            {                   
                string query = "SELECT * FROM products ORDER BY id ASC"; // select all products, in ascending order based on id
                /*NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign query string to command property*/

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                List<Product> _products = new List<Product>();

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Product product = new Product(); // individual product
                        product.name = (string)dataTable.Rows[i]["name"];
                        product.id = (int)dataTable.Rows[i]["id"];
                        product.amount = (double)dataTable.Rows[i]["amount"];
                        product.price = (decimal)dataTable.Rows[i]["price"];

                        _products.Add(product); // add student to the list
                    }
                }

                if (_products.Count > 0) // this means we have retrieved info from the table
                {
                    response.statusCode = 200;
                    response.message = "Products is retrieved successfully";
                    response.product = null;
                    response.products = _products;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "No products is retrieved";
                    response.product = null;
                    response.products = null;
                }                
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public ProductResponse SearchData(NpgsqlConnection con, int id)
        {
            ProductResponse response = new ProductResponse();

            try
            {
                // perform query
                /*string query = "SELECT * FROM products WHERE id = '" + id + "'";*/
                string query = "SELECT * FROM products WHERE id = @ID"; // another way of query
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ID", id);

                // create data adapter
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                Product product = new Product();

                if(dataTable.Rows.Count > 0) // this means we get at least 1 match, which is required
                {
                    // here, we use 0 because we need only the 1st entry to be retrieved
                    product.name = (string)dataTable.Rows[0]["name"];
                    product.id = (int)dataTable.Rows[0]["id"];
                    product.amount = (double)dataTable.Rows[0]["amount"];
                    product.price = (decimal)dataTable.Rows[0]["price"];

                    // create the response for our server as well
                    response.statusCode = 200;
                    response.message = "Product found and retrieved";
                    response.product = product;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Product not found";
                    response.product = null;
                    response.products = null;
                }
            } 
            catch (NpgsqlException ex)
            {
                // Handle the exception
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public ProductResponse AddAmount(NpgsqlConnection con, int id, double amountToAdd)
        {
            ProductResponse response = new ProductResponse();

            try
            {
                string query = "UPDATE products SET amount = amount + @amountToAdd WHERE id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign query string to command property
                cmd.Parameters.AddWithValue("@amountToAdd", amountToAdd); // 1st parameter placeholder of query: amountToAdd input passed into this method
                cmd.Parameters.AddWithValue("@id", id); // 2nd parameter placeholder of query: id input passed into this method

                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.statusCode = 200;
                    response.message = "Product amount is added";
                    response.product = null;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Product amount is not added";
                    response.product = null;
                    response.products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.statusCode = 500;
                response.message = "Error: " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return response;
        }

        public ProductResponse MinusAmount(NpgsqlConnection con, int id, double amountToMinus)
        {
            ProductResponse response = new ProductResponse();

            try
            {
                string query = "UPDATE products SET amount = amount - @amountToMinus WHERE id = @id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign query string to command property
                cmd.Parameters.AddWithValue("@amountToMinus", amountToMinus); // 1st parameter placeholder of query: amountToAdd input passed into this method
                cmd.Parameters.AddWithValue("@id", id); // 2nd parameter placeholder of query: id input passed into this method

                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.statusCode = 200;
                    response.message = "Product amount is subtracted";
                    response.product = null;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Product amount is not subtracted";
                    response.product = null;
                    response.products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.statusCode = 500;
                response.message = "Error: " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return response;
        }

        public ProductResponse UpdateAmount(NpgsqlConnection con, int id, double amount)
        {
            ProductResponse response = new ProductResponse();

            try
            {
                string query = "UPDATE products SET amount = @amountToUpdate WHERE id = @id";

                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@amountToUpdate", amount);
                
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.statusCode = 200;
                    response.message = "Product information is updated";
                    response.product = null;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Product information is not updated";
                    response.product = null;
                    response.products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.statusCode = 500;
                response.message = "Error: " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return response;
        }

        public ProductResponse DeleteData(NpgsqlConnection con, int id)
        {
            ProductResponse response = new ProductResponse();

            try
            {
                con.Open();
                string query = "DELETE FROM products WHERE id = @ID";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign query string to command property
                cmd.Parameters.AddWithValue("@ID", id); // parameter placeholder of query: id input passed into this method

                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.statusCode = 200;
                    response.message = "Product is deleted";
                    response.product = null;
                    response.products = null;
                }
                else
                {
                    response.statusCode = 100;
                    response.message = "Product does not exist";
                    response.product = null;
                    response.products = null;
                }
            }
            catch (NpgsqlException ex)
            {
                response.statusCode = 500;
                response.message = "Error: " + ex.Message;
                Console.WriteLine(ex.Message);
            }
            return response;

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
            dispatcher.InvokeAsync(() =>
            {
                foreach (CartItem item in cartItems)
                {
                    total += item.subtotal; // accumulate the subtotal of each item
                }
            });
            return total;
        }

        // Populate the list box with the products from the database
        public List<Product> GetProductsFromDatabase(NpgsqlConnection con)
        {
            List<Product> products = new List<Product>(); // initialize a list for the product objects

            // Use try-catch block to handle exception(s) that may occur during database operations
            try
            {
                con.Open(); // Replace "your_connection_string_here" with your actual database connection string
                
                
                string query = "SELECT * FROM products ORDER BY name"; // select all rows in products table in the database
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandText = query; // assign the query string to the command property

                con.Open();
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd); // adapter passes cmd as a parameter
                DataTable dataTable = new DataTable(); // store the results retrieved from the database
                dataAdapter.Fill(dataTable); // fill the dataTable with retrieved data

                // dataTable is converted to an enumerable sequence of rows
                products = dataTable.AsEnumerable()
                    .Select(row => new Product( // project each row of dataTable into a new Product object
                        row.Field<string>("name"), // access field value from each row using the Field method
                        row.Field<int>("id"),
                        row.Field<double>("amount"),
                        row.Field<decimal>("price")
                        )) // return IEnumerable<Product>
                    .ToList(); // convert IEnumerable<Product> to List<Product> and store in the products variable
                
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products;
        }

        // Method to add cart item to the List<CartItem>
        public CartItemResponse AddCartItem(NpgsqlConnection con, CartItem cartItem)
        {
            CartItemResponse response = new CartItemResponse();

            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.InvokeAsync(() =>
            {
                cartItems.Add(cartItem); // add the cart item to the cartItems list
                response.statusCode = 200;
                response.message = "CartItem added successfully.";
                response.cartItem = cartItem;
                response.cartItems = cartItems.ToList(); // Return a copy of the cartItems list to avoid affecting the original list in the response
            });

            return response;
        }

        // Method to get information about items in the customer's cart 
        public CartItemResponse GetCartItems(NpgsqlConnection con)
        {
            CartItemResponse response = new CartItemResponse();

            // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
            // this avoids the potential conflicts when accessing shared data from different thread
            dispatcher.InvokeAsync(() =>
            {
                List<CartItem> cartItemsCopy = cartItems.Select(item => new CartItem
                {
                    // assign values of CartItem from corresponding properties 
                    productName = item.productName,
                    productId = item.productId,
                    amountPurchased = item.amountPurchased,
                    pricePerKg = item.pricePerKg,
                    subtotal = item.subtotal
                }).ToList(); // convert IEnumerable<CartItem> to List<CartItem> and store in cartItemsCopy variable

                response.statusCode = 200;
                response.message = "Cart items retrieved successfully.";
                response.cartItems = cartItemsCopy;
            });

            return response;
        }

        // Method to update the product amount in inventory after the customer made the purchase
        public CartItemResponse UpdateInventory(NpgsqlConnection con, int id)
        {
            CartItemResponse response = new CartItemResponse();

            // Replace "your_connection_string_here" with your actual database connection string
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        foreach (CartItem item in cartItems) // for each CartItem object in the cartItems list
                        {
                            string query = "UPDATE products SET amount = amount - @amountPurchased WHERE id = @id"; // deduct the amount of the product with a certain id
                            using (var cmd = new NpgsqlCommand(query, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@amountPurchased", item.amountPurchased);  // 1st parameter placeholder @amountPurchased of query: amountPurchased value of CartItem 
                                cmd.Parameters.AddWithValue("@id", item.productId);                     // 2nd parameter placeholder @id of query: productId value of CartItem 

                                cmd.ExecuteNonQuery(); // execute the update query
                            }
                        }

                        transaction.Commit();

                        // Fetch the updated inventory amounts from the database
                        products = GetProductsFromDatabase(con);

                        // Ensure the code is executed on the UI thread to provide thread-safe access to the cartItems collection,
                        // this avoids the potential conflicts when accessing shared data from different thread
                        dispatcher.InvokeAsync(() =>
                        {
                            cartItems.Clear(); // clear the cart after confirming the order
                            response.statusCode = 200;
                            response.message = "Inventory updated successfully.";
                        });
                    }
                    catch (NpgsqlException e)
                    {
                        transaction.Rollback();
                        Console.WriteLine(e.Message);
                        response.statusCode = 500;
                        response.message = "An error occurred during inventory update.";
                    }
                }
            }

            return response;
        }


    }
}
