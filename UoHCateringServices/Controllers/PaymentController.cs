using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UoHCateringServices.DTO;

namespace UoHCateringServices.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApiRoute _apiRoute;
        Cart cart;


        public PaymentController(IConfiguration config, IOptionsSnapshot<ApiRoute> options, Cart _cart)
        {
            _config = config;
            _apiRoute = options.Value;
            cart = _cart;

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string amount, string invoice_id)
        {

            //Get Config values
            var baseUrl = _config.GetValue<string>("ApiRoute:PaypalBaseUrl");
            string PayPalClientId = _config.GetValue<string>("PayPalCredentials:SandboxClientId");
            string PayPalSecretKey = _config.GetValue<string>("PayPalCredentials:SandboxClientSecret");

            string AccessTokenEndpoint = _config.GetValue<string>("ApiRoute:GetAccessToken");
            var url = baseUrl + AccessTokenEndpoint;

            TempData["baseurl"] = baseUrl;


            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                           $"{PayPalClientId}:{PayPalSecretKey}")));

            var dict = new Dictionary<string, string>();
            dict.Add("Content-Type", "application/x-www-form-urlencoded");
            
            var req = new HttpRequestMessage(HttpMethod.Post, url + "?grant_type=client_credentials") 
            {
                Content = new FormUrlEncodedContent(dict) 
            };
            HttpResponseMessage resp = await client.SendAsync(req);

            client.Dispose();

            if(resp.StatusCode == System.Net.HttpStatusCode.OK && resp.IsSuccessStatusCode)
            {
                ////fetch cart items
                var items = cart.GetCartItems();
                
                var list = new List<Item>();
                foreach (var it in items)
                {
                    list.Add(new Item { 
                        name = it.ProductName,
                        description = it.ProductName,
                        quantity = it.Quantity,
                        unit_amount = new UnitAmount { currency_code = "GBP", value = it.Amount.ToString()}
                    });
                }

                var contentResponse = resp.Content.ReadAsStringAsync();
                var responseResult = contentResponse.Result;

                //serialize result into object model
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseResult);


                PayPalOrder order = new PayPalOrder
                {
                    intent = "CAPTURE",
                    invoice_id = invoice_id,
                    reference_id = invoice_id,
                    purchase_units = new[] {
                        new PurchaseUnit
                        {
                            amount = new Amount
                            {
                                currency_code = "GBP",
                                value = amount,

                               breakdown = new Breakdown()
                                {
                                    item_total = new ItemTotal()
                                    {
                                        currency_code = "GBP",
                                        value = amount
                                    }
                                }
                            },
                            items = list
 
                        }
                    }
                };


                
                //get order checkout endpoint route from appsettings
                string checkoutOrderEndpoint = _config.GetValue<string>("ApiRoute:CheckoutOrder");


                using(var _client = new HttpClient())
                {
                    TempData["access_token"] = authResponse.access_token;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.access_token);
                    var requestHeader = new Dictionary<string, string>();
                    requestHeader.Add("Content-Type", "application/json");

                    StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //call checkout order endpoint
                    var oderEndpoint = baseUrl + checkoutOrderEndpoint;
                    HttpResponseMessage response = await _client.PostAsync(oderEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var orderResponse = response.Content.ReadAsStringAsync();
                        var orderresponseResult = orderResponse.Result;

                        return Json(orderresponseResult);
                    }


                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(string orderID)
        {
            //get needed data
            var baseUrl = TempData["baseurl"].ToString();
            var token = TempData["access_token"].ToString();

            string PayPalClientId = _config.GetValue<string>("PayPalCredentials:SandboxClientId");
            string PayPalSecretKey = _config.GetValue<string>("PayPalCredentials:SandboxClientSecret");

            using (var _client = new HttpClient())
            {
                string captureOrder = _config.GetValue<string>("ApiRoute:CaptureOrder");

                //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //_client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                           $"{PayPalClientId}:{PayPalSecretKey}")));

                StringContent content = new StringContent("", Encoding.UTF8, "application/json"); 
                HttpResponseMessage response = await _client.PostAsync($"{baseUrl}{captureOrder}{orderID}/capture", content);
                if (response.IsSuccessStatusCode)
                {
                    var captureResponse = response.Content.ReadAsStringAsync();
                    var captureResponseResult = captureResponse.Result;

                    return Json(captureResponseResult);
                }
                else
                {
                    var orderResponse = response.Content.ReadAsStringAsync();
                    var orderresponseResult = orderResponse.Result;

                    return Json(orderresponseResult);
                }
                

            }


            return View();
        }
    }
}
