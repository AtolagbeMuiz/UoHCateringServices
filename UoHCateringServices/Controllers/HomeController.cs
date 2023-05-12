using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.DTO;
using UoHCateringServices.Interfaces;
using UoHCateringServices.Models;

namespace UoHCateringServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserRepo _userRepo;
        private IProductRepo _productRepo;
        private IEncode _encode;
        Cart cart; //= new Cart();
        public HomeController(ILogger<HomeController> logger, IUserRepo userRepo, IEncode encode, IProductRepo productRepo, Cart _cart)
        {
            _logger = logger;
            _userRepo = userRepo;
            _encode = encode;
            _productRepo = productRepo;
            cart = _cart;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                //Generate Guid ID for User
                userDTO.User_Id = Guid.NewGuid();

                //Encrypt Password
                userDTO.Password = _encode.Encrypt(userDTO.Password);

                var user = new User
                {
                    User_Id = userDTO.User_Id,
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                };
                var response = _userRepo.AddUser(user);
                
                if(response.ToString() == "user already exist")
                {
                    ViewData["StatusMsg"] = "User already exist";

                    return View();
                }
                else if (response != null)
                {
                    ViewData["StatusMsg"] = "Registered Successfully";

                    return View();
                }
            }
            ViewData["StatusMsg"] = "Registration Failed";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CartItems()
        {
            var cartItems = cart.GetCartItems();
            if(cartItems.Count > 0)
            {
                return View(cartItems);

            }
            return View(null);

        }

        [HttpGet]
        public IActionResult PaymentStatus(string status)
        {
            if(!string.IsNullOrEmpty(status))
            {
                var paymentStatus = bool.Parse(status);
                if(paymentStatus == true)
                {
                    ViewBag.paymentStatus = true;

                }
                else
                {
                    ViewBag.paymentStatus = false;

                }

            }
            else
            {
                ViewBag.paymentStatus = false;

            }

            return View();
        }


        public IActionResult Items()
        {
            if (TempData.ContainsKey("cartItemsCount"))
            {
                ViewBag.cartItemsCount = TempData["cartItemsCount"];

            }
            else
            {
                ViewBag.cartItemsCount = "0";
            }

            var products = _productRepo.getAllProducts();
            if(products.Count > 0)
            {
                return View(products);
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddToCart(string productId, string productName, string amount)
        {
            var product_id = int.Parse(productId);
            var productAmount = double.Parse(amount);

            Product prod = new Product
            {
                ProductId = product_id,
                ProductName = productName,
                Amount = productAmount
               
            };

            //Add to Cart
            //var items = cart.GetCartItems();
            var cartItems = cart.AddProductToCart(prod);


            TempData["cartItemsCount"] = cartItems.Count().ToString();

            //Redirect to items page
            return RedirectToAction("Items");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
