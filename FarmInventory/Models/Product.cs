using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmInventory.Models
{
    public class Product
    {
        // Instance variables, getters and setters
        public string name { get; set; }
        public int id { get; set; }
        public double amount { get; set; }
        public decimal price { get; set; }

        // Default constructor
        public Product() { }

        // constructor with parameters
        public Product(string name, int id, double amountKg, decimal price)
        {
            this.name = name;
            this.id = id;
            this.amount = amountKg;
            this.price = price;
        }
    } 
}
