using FarmInventoryREST.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace FarmInventoryREST.Controllers
{
    /* this is the scope for our API controller.
     * we need to create the base api name for the REST APIs we will create for this program. 
     * first, give the route of APIs and then we need to pass the controller we will create in this program.
     */

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            /* The IConfiguration in this constructor will be invoked when the
             * class instance is created. it will grab the connection from the 
             * appsettings and will send it back to the configuration object or
             * instance of this scope
             */
            _configuration = configuration;
        }

        // Method to insert a new product into the products table in database
        [HttpPost]
        [Route("InsertData")]
        public ProductResponse InsertData(Product product)
        {
            // create database connection
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString());
            ProductResponse response = new ProductResponse();

            // call the application interface
            Applications app = new Applications();
            response = app.InsertData(con, product);

            return response;
        }

        // Method to read the data of product
        [HttpGet]
        [Route("GetAllData")]

        public ProductResponse GetAllData()
        {
            // create database connection
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString());
            ProductResponse response = new ProductResponse();

            // call the application interface
            Applications app = new Applications();
            response = app.GetAllData(con);

            return response;
        }

        // reading/retrieving a product using id
        [HttpGet]
        [Route("SearchData/{id}")] // sending a id parameter to the remote server
        public ProductResponse SearchData(int id)
        {
            ProductResponse response = new ProductResponse();
            // create database connection
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString());

            // call the application interface
            Applications app = new Applications();
            response = app.SearchData(con, id);

            return response;
        }

        // add amount of product
        [HttpPut]
        [Route("AddAmount/{id}/{amountToAdd}")]
        public ProductResponse AddAmount(int id, double amountToAdd) 
        {
            ProductResponse response = new ProductResponse();
            // create database connection
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            {
                Applications app = new Applications();
                response = app.AddAmount(con, id, amountToAdd);
            }
            return response;
        }

        [HttpPut]
        [Route("MinusAmount/{id}/{amountToMinus}")]
        public ProductResponse MinusAmount(int id, double amountToMinus)
        {
            ProductResponse response = new ProductResponse();
            // create database connection
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            {
                Applications app = new Applications();
                response = app.MinusAmount(con, id, amountToMinus);
            }
            return response;
        }

        // update amount of product
        [HttpPut] // generate put request to update the data information
        [Route("UpdateAmount/{id}/{amount}")]
        public ProductResponse UpdateAmount(int id, double amount)
        {
            ProductResponse response = new ProductResponse();
            // create database connection
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            { 
                Applications app = new Applications();
                response = app.UpdateAmount(con, id, amount);
            }
            return response;
        }

        // delete a product
        [HttpDelete]
        [Route("DeleteData/{id}")]
        public ProductResponse DeleteData(int id)
        {
            ProductResponse response = new ProductResponse();
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString());
            Applications app = new Applications();
            response = app.DeleteData(con, id);
            return response;
        }

    }
}
