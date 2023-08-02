using FarmInventory.Controllers;
using FarmInventory.Models;
using Npgsql;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
/*using System.Net.Http.Json;*/

namespace FarmInventory.Views
{
    public partial class Admin : Window
    {
        /* Before using the REST APIs created in FarmInventoryREST, 
         * we need to configure our REST API connection with the present WPF application.
         * REST APIs are connected to the remote server (database) with http communication
         * protocol. So, to communicate with REST APIs, we configure out http communication too
         */
        private string baseUri = "https://localhost:7079/api/Product/";
/*        private AdminController adminController;
        private List<Product> products;*/
        public Admin()
        {
            InitializeComponent();
            Loaded += AdminLoaded; // calling AdminLoaded method
/*            adminController = new AdminController();*/
        }

        // Our Desktop application is going to communicate with the remote server 
        // through rest api. Once you use the rest API, each rest API will generate a
        // response for you which you need to catch in your program. Our rest API
        // generates the response message as we have designed it in the Response.cs 
        // class. So, we need the same structure/class in the WPF application as well
        // to capture the response properly.

        // Show data once the Admin window is opened
        private void AdminLoaded(object sender, RoutedEventArgs e)
        {
            ShowAllData();
        }

        // create Product class instance to take the information from front-end
        // Action when the Insert button is pressed
        private async void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            // testing////////////
            // initialize the variables
            string productName = tbProductName.Text;
            int productId = int.Parse(tbProductID.Text);
            double amountKg = double.Parse(tbAmountKg.Text);
            decimal pricePerKg = decimal.Parse(tbPricePerKg.Text);

            // create a Product instance to get info from front end
            Product product = new Product
            {
                name = productName,
                id = productId,
                amount = amountKg,
                price = pricePerKg
            };

            try
            {
                // Serialize the Product object to JSON
                string jsonProduct = JsonConvert.SerializeObject(product);

                // Create a HttpClient instance
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseUri);

                    // Set the "Content-Type" header to "application/json"
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Post the JSON data to the API
                    HttpResponseMessage response = await httpClient.PostAsync("InsertData", new StringContent(jsonProduct, Encoding.UTF8, "application/json"));

                    // Read the response content as a string
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the response JSON to a Response object
                    ProductResponse apiResponse = JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);

                    // Display the response message
                    MessageBox.Show(apiResponse.message);

                    // Refresh the product list on data grid
                    ShowAllData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        // Action when the Show All button is pressed 
        private async void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Call the GetAllData API
                ProductResponse apiResponse = await CallGetAllDataApi();

                // Display the product list on dataView
                UpdateUIWithProductList(apiResponse.products);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }
        
        // Action when Search button is pressed
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearchId.Text))
            {
                MessageBox.Show("Please enter a product ID.");
                return;
            }

            if (!int.TryParse(tbSearchId.Text, out int searchId))
            {
                MessageBox.Show("Invalid product ID. Please enter a valid integer value.");
                return;
            }

            try
            {
                // Call the SearchData API
                Product product = await CallSearchDataApi(searchId);

                if (product != null)
                {
                    // Product found, show it on dataView
                    dataView.ItemsSource = new List<Product> { product };
                }
                else
                {
                    // Product not found, clear dataView and show message
                    dataView.ItemsSource = null;
                    MessageBox.Show("Product not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        // Action when Delete button is pressed
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product selectedRow = dataView.SelectedItem as Product;

            if (selectedRow == null)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            int selectedId = selectedRow.id;

            try
            {
                // Call the DeleteData API
                ProductResponse response = await CallDeleteDataApi(selectedId);

                if (response.statusCode == 200)
                {
                    // Deletion successful, show success message and refresh the product list on data grid
                    MessageBox.Show("Product deleted successfully.");
                    ShowAllData();
                }
                else
                {
                    // Deletion failed, show error message
                    MessageBox.Show("Failed to delete product.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }


        // Action when the + button is pressed
        private async void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbIdUpdate.Text, out int productId))
            {
                MessageBox.Show("Invalid product ID.");
                return;
            }

            if (!double.TryParse(tbAmountKgUpdate.Text, out double amountToAdd))
            {
                MessageBox.Show("Invalid amount.");
                return;
            }

            try
            {
                // Call the AddAmount API
                ProductResponse response = await CallAddAmountApi(productId, amountToAdd);

                if (response.statusCode == 200)
                {
                    // Addition successful, show success message and refresh the product list on data grid
                    MessageBox.Show("Amount added successfully.");
                    ShowAllData();
                }
                else
                {
                    // Addition failed, show error message
                    MessageBox.Show("Failed to add amount.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }


        // Action when - button is pressed
        private async void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbIdUpdate.Text, out int productId))
            {
                MessageBox.Show("Invalid product ID.");
                return;
            }

            if (!double.TryParse(tbAmountKgUpdate.Text, out double amountToMinus))
            {
                MessageBox.Show("Invalid amount.");
                return;
            }

            try
            {
                // Call the MinusAmount API
                ProductResponse response = await CallMinusAmountApi(productId, amountToMinus);

                if (response.statusCode == 200)
                {
                    // Subtraction successful, show success message and refresh the product list on data grid
                    MessageBox.Show("Amount subtracted successfully.");
                    ShowAllData();
                }
                else
                {
                    // Subtraction failed, show error message
                    MessageBox.Show("Failed to subtract amount.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }


        // Action when Direct Update button is pressed
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbIdUpdate.Text, out int productId))
            {
                MessageBox.Show("Invalid product ID.");
                return;
            }

            if (!double.TryParse(tbAmountKgUpdate.Text, out double amountToUpdate))
            {
                MessageBox.Show("Invalid amount.");
                return;
            }

            try
            {
                // Call the UpdateAmount API
                ProductResponse response = await CallUpdateAmountApi(productId, amountToUpdate);

                if (response.statusCode == 200)
                {
                    // Update successful, show success message and refresh the product list on data grid
                    MessageBox.Show("Amount updated successfully.");
                    ShowAllData();
                }
                else
                {
                    // Update failed, show error message
                    MessageBox.Show("Failed to update amount.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        // Method to call the GetAllData API
        private async Task<ProductResponse> CallGetAllDataApi()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://localhost:7079/api/Product/GetAllData"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to get product data from API.");
                }
            }
        }



        // Method to call the SearchData API
        private async Task<Product> CallSearchDataApi(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7079/api/Product/SearchData/{id}"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Product>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to search product from API.");
                }
            }
        }

        // Method to call the DeleteData API
        private async Task<ProductResponse> CallDeleteDataApi(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7079/api/Product/DeleteData/{id}"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.DeleteAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to delete product from API.");
                }
            }
        }

        // Method to call the AddAmount API
        private async Task<ProductResponse> CallAddAmountApi(int id, double amountToAdd)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7079/api/Product/AddAmount/{id}?amount={amountToAdd}"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.PutAsync(url, null);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to add amount to product from API.");
                }
            }
        }

        // Method to call the MinusAmount API
        private async Task<ProductResponse> CallMinusAmountApi(int id, double amountToMinus)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7079/api/Product/MinusAmount/{id}?amount={amountToMinus}"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.PutAsync(url, null);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to subtract amount from product from API.");
                }
            }
        }

        // Method to call the UpdateAmount API
        private async Task<ProductResponse> CallUpdateAmountApi(int id, double amountToUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7079/api/Product/UpdateAmount/{id}?amount={amountToUpdate}"; // Replace with the actual URL of your API
                HttpResponseMessage response = await client.PutAsync(url, null);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ProductResponse>(jsonResponse);
                }
                else
                {
                    throw new Exception("Failed to update amount of product from API.");
                }
            }
        }


        // Method to show all data on DataGrid
        private async void ShowAllData()
        {
            try
            {
                // Call the GetAllData API
                ProductResponse apiResponse = await CallGetAllDataApi();

                // Check if the response is successful
                if (apiResponse.statusCode == 200 && apiResponse.products != null)
                {
                    // Update the product list on dataView
                    UpdateUIWithProductList(apiResponse.products);
                }
                else
                {
                    // Show an error message
                    MessageBox.Show("Failed to retrieve product data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }




        // Method to update the product list
        private void UpdateUIWithProductList(List<Product> products)
        {
            // Set the ItemsSource of dataView as the list of products
            dataView.ItemsSource = products;
        }


    }
}
