using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UoHCateringServices.Interfaces
{
    public interface IEncode
    {
        string Encrypt(string clearText);
    }
}
