namespace FarmInventoryREST.Models
{
    public class CartItemResponse
    {
        /* Set a structure of the response obtained from the remote server */
        public int statusCode { get; set; }
        public string message { get; set; }
        public CartItem cartItem { get; set; }
        public List<CartItem> cartItems { get; set; }
    }
}
