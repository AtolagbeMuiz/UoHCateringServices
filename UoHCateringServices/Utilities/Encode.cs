using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UoHCateringServices.Interfaces;

namespace UoHCateringServices.Utilities
{
    public class Encode : IEncode
    {
        //private static string EncryptionKey = "qwewe!@#$%^&&*redf";

        private readonly IConfiguration _config;
        public Encode(IConfiguration config)
        {
            _config = config;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = _config.GetValue<string>("Config:EncryptionKey");

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                                                            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    var stringBytes = ms.ToArray();
                    clearText = Convert.ToBase64String(stringBytes, 0, stringBytes.Length);
                }
            }
            return clearText.Replace("+", "_");
        }
    }
}
