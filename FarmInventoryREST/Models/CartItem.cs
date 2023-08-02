namespace FarmInventoryREST.Models
{
    public class CartItem
    {
        // instance variables, getters and setters
        public string productName { get; set; }
        public int productId { get; set; }
        public double amountPurchased { get; set; }
        public decimal pricePerKg { get; set; }
        public decimal subtotal { get; set; }

        // Default constructor
        public CartItem() { }

        // Constructor with parameters
        public CartItem(string productName, int productId, double amountPurchased, decimal pricePerKg, decimal subtotal)
        {
            this.productName = productName;
            this.productId = productId;
            this.amountPurchased = amountPurchased;
            this.pricePerKg = pricePerKg;
            this.subtotal = subtotal;
        }
    }
}
