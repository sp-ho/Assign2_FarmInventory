namespace FarmInventoryREST.Models
{
    public class ProductResponse
    {
        /* Set a structure of the response obtained from the remote server */
        public int statusCode { get; set; }
        public string message { get; set; }
        public Product product { get; set; }
        public List<Product> products { get; set; }
    }
}
