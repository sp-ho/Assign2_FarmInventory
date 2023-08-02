using FarmInventory.Controllers;
using FarmInventory.Models;
using Npgsql;
using System;
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

namespace FarmInventory.Views
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        private SalesController salesController; 

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

        // Action when the Add To Cart button is pressed by the customer
        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {

            // we are going to send the cart item information/ structure we are getting from 
            // the front-end. We need to create same CartItem.cs classes in this application 
            // as well

            // Get the selected product from list box 
            Product selectedProduct = listBoxProducts.SelectedItem as Product;
            

            if (selectedProduct != null)
            {
                // Retrieve the amount entered by the customer
                double amountPurchased = double.Parse(tbAmount.Text);
                // Calculate the subtotal based on the amount and unit price
                decimal subtotal = salesController.calcSubtotal(amountPurchased, selectedProduct.price);

                CartItem cartItem = new CartItem
                {
                    // access the selected product's info and use them to create a CartItem object
                    productName = selectedProduct.name,
                    productId = selectedProduct.id,
                    amountPurchased = amountPurchased,
                    pricePerKg = selectedProduct.price,
                    subtotal = subtotal
                };

                // Create a new thread to add item to cart
                Thread addToCartThread = new Thread(() =>
                {
                    // Add the cart item
                    salesController.AddCartItem(cartItem);

                    // Dispatcher.Invoke method is used to update the UI by setting the ItemsSource of dataGridCart to updated cart items
                    Dispatcher.Invoke(() =>
                    {
                        // Display the cart items in the dataGridCart
                        dataGridCart.ItemsSource = salesController.GetCartItems();

                        decimal totalPrice = salesController.calcFinalTotal(); // calculate the total price of all cart items
                        lblTotalPrice.Content = totalPrice.ToString("C"); // "C" format specifier for currency

                    });
                });
                addToCartThread.Start();
            }
        }

        // Action when the Confirm button is pressed by customer
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // create a new thread to update the amount of selected product in the inventory
            Thread confirmThread = new Thread(() =>
            {
                MessageBox.Show("Purchase confirmed.");

                salesController.UpdateInventory();

                Dispatcher.Invoke(() =>
                {
                    // clear the data grid
                    dataGridCart.ItemsSource = null;

                    // reset total price
                    lblTotalPrice.Content = "$0.00";
                });
            });
            confirmThread.Start();
        }
    }
}
