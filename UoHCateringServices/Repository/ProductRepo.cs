using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.Interfaces;
using UoHCateringServices.Models;

namespace UoHCateringServices.Repository
{
    public class ProductRepo : IProductRepo
    {
        //in-Memmory Database
        private readonly List<Product> listOfProducts;

        public ProductRepo()
        {
             List<Product> products = new List<Product>{
                new Product() { ProductId = 1 , ProductName = "Pizza", Amount = 4, ImagePath = "/assets/Pizza.jpeg"},
                new Product() { ProductId = 2 , ProductName = "Cofee", Amount = 8, ImagePath = "/assets/Coffee.jpeg"},
                new Product() { ProductId = 3 , ProductName = "Cake", Amount = 1.50, ImagePath = "/assets/Cake.jpeg"}
             };

            listOfProducts = products;
        }

        public List<Product> getAllProducts()
        {
            return listOfProducts; 
        }
    }
}
