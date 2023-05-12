﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.Models;

namespace UoHCateringServices.Interfaces
{
    public interface IProductRepo
    {
        List<Product> getAllProducts();
    }
}
