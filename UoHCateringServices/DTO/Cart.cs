using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.Models;

namespace UoHCateringServices.DTO
{
    public class Cart
    {
        public List<Product> cartItems = new List<Product>();

        public List<Product> AddProductToCart(Product product)
        {
            cartItems.Add(product);
            return cartItems;

        }

        public List<Product> GetCartItems()
        {
            return cartItems;
        }
    }
}
