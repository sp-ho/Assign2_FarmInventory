using System;
using System.Collections.Generic;

namespace FarmInventory.Models
{
    public class ProductResponse
    {
        /* Set a structure of the response obtained from the remote server */
        public int statusCode { get; set; }
        public string message { get; set; }
        public Product product { get; set; }
        public List<Product> products { get; set; }
        public CartItem item { get; set; }
        public List<CartItem> items { get; set; }

/*        public static implicit operator Response(List<Product> v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator List<object>(Response v)
        {
            throw new NotImplementedException();
        }*/
    }
}
