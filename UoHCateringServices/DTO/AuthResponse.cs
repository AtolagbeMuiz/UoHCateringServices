using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoHCateringServices.DTO
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public double expires_in { get; set; }
        public string nonce { get; set; }
    }

    public class PayPalOrder
    {
        public string intent { get; set; }
        public Payer payer { get; set; }
        public PurchaseUnit[] purchase_units { get; set; }
    }
    public class Payer
    {
        public string name { get; set; }
        public string email_address { get; set; }
    }

    public class PurchaseUnit
    {
        public Amount amount { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
}
