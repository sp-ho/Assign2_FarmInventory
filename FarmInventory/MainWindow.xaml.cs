using FarmInventory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace FarmInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
/*        HttpClient httpClient = new HttpClient(); // configure the http connection for the remote REST APIs*/

        public MainWindow()
        {
            /*
            * We need to configure our wpf application to ensure proper connection
            * 
            * step 01: Setup the Base address for the REST APIs
            *//*

            httpClient.BaseAddress = new Uri("https://localhost:7079/api/Product/"); // check port number on swagger

            // step 02: Configure the packets header
            // we first need to clear the present packet header
            httpClient.DefaultRequestHeaders.Accept.Clear();

            // setp 03: Add our customized header information
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

            // we are sending request to the server in json format as well as the response
            // we have configured for our application is in JSON format as well. Thus, we need to
            // configure them at the beginning.*/
            InitializeComponent();
        }

        // Action when Product button is pressed
        private void btnProduct_Click(object sender, object e)
        {
            Admin admin = new Admin(); // create a Admin instance
            admin.Show(); // open Admin window
        }

        // Action when Sales button is pressed
        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            Sales sales = new Sales(); // create a Sales instance
            sales.Show(); // open Sales window
        }

    }
}
