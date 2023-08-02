using FarmInventoryREST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FarmInventoryREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CartItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AddCartItem")]
        public CartItemResponse AddCartItem(CartItem cartItem)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            {
                CartItemResponse response = new CartItemResponse();
                Applications app = new Applications();
                response = app.AddCartItem(con, cartItem);
                return response;
            }
        }

        [HttpGet]
        [Route("GetCartItems")]
        public CartItemResponse GetCartItems()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            {
                CartItemResponse response = new CartItemResponse();
                Applications app = new Applications();
                response = app.GetCartItems(con);
                return response;
            }
        }

        [HttpPut]
        [Route("UpdateInventory/{id}")]
        public CartItemResponse UpdateInventory(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("dbconnection").ToString()))
            {
                CartItemResponse response = new CartItemResponse();
                Applications app = new Applications();
                response = app.UpdateInventory(con, id);
                return response;
            }
        }
    }
}
