using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoHCateringServices.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public double Amount { get; set; }

        public string Quantity { get; set; } = "1";
    }
}
