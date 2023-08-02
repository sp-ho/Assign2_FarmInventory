using FarmInventory.Controllers;
using FarmInventory.Models;
using Newtonsoft.Json;
using Npgsql;
using Octokit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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

namespace FarmInventory.Views
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        private SalesController salesController;
        private string baseUri = "https://localhost:7079/api/Product/";

        public Sales()
        {
            InitializeComponent();
            Loaded += SalesLoaded; // calling SalesLoaded method
            salesController = new SalesController(Dispatcher); // passing Dispatcher for multi-threading purpose
        }

        // Loaded event of window: what the window does when it is loaded
        private void SalesLoaded(object sender, RoutedEventArgs e)
        {
            List<Product> products = salesController.GetProductsFromDatabase(); // retrieve product list from database
            listBoxProducts.ItemsSource = products; // populate the product list from database to list box
        }

        private async void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = listBoxProducts.SelectedItem as Product;

            if (selectedProduct != null)
            {
                if (double.TryParse(tbAmount.Text, out double amountPurchased))
                {
                    decimal subtotal = salesController.calcSubtotal(amountPurchased, selectedProduct.price);

                    CartItem cartItem = new CartItem
                    {
                        productName = selectedProduct.name,
                        productId = selectedProduct.id,
                        amountPurchased = amountPurchased,
                        pricePerKg = selectedProduct.price,
                        subtotal = subtotal
                    };

                    try
                    {
                        // Serialize the cartItem to JSON
                        string json = JsonConvert.SerializeObject(cartItem);

                        // Create HttpClient instance
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.BaseAddress = new Uri(baseUri);
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            // Create the HttpContent from the JSON data
                            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                            // Send POST request to AddCartItem endpoint
                            HttpResponseMessage response = await httpClient.PostAsync("AddCartItem", content);
                            response.EnsureSuccessStatusCode();

                            // Read the response content
                            string responseContent = await response.Content.ReadAsStringAsync();

                            // Deserialize the response JSON to CartItemResponse
                            CartItemResponse addToCartResponse = JsonConvert.DeserializeObject<CartItemResponse>(responseContent);

                            // Update the UI using the Dispatcher to ensure thread safety
                            Dispatcher.Invoke(() =>
                            {
                                dataGridCart.ItemsSource = addToCartResponse.cartItems;
                                decimal totalPrice = salesController.calcFinalTotal(); // calculate the total price of all cart items
                                lblTotalPrice.Content = totalPrice.ToString("C"); // "C" format specifier for currency
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding item to cart: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid amount entered. Please enter a valid number.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product from the list.");
            }
        }



        // Action when the Confirm button is pressed by customer
        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Thread confirmThread = new Thread(async () =>
            {
                MessageBox.Show("Purchase confirmed.");

                try
                {
                    // Call the UpdateInventory REST API using the HttpClient
                    HttpResponseMessage response = await UpdateInventoryAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            // clear the data grid
                            dataGridCart.ItemsSource = null;

                            // reset total price
                            lblTotalPrice.Content = "$0.00";
                        });
                    }
                    else
                    {
                        // Handle unsuccessful response (if needed)
                        MessageBox.Show("Failed to update inventory. Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating inventory: " + ex.Message);
                }
            });

            confirmThread.Start();
        }

        private async Task<HttpResponseMessage> UpdateInventoryAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUri);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Here, you might need to pass the correct product ID for the UpdateInventory endpoint.
                // For now, let's assume productId is obtained from somewhere in the Sales window.
                int productId = 1; // Replace with the actual product ID.

                HttpResponseMessage response = await httpClient.PutAsync($"UpdateInventory/{productId}", null);
                return response;
            }
        }
    }
}
