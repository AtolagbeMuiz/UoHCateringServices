﻿using System;

namespace UoHCateringServices.DTO
{
    public class UserDTO
    {
       
            public Guid User_Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public string ConfirmPassword { get; set; }
        
    }
}
