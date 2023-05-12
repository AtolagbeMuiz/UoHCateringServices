using System;
using System.ComponentModel.DataAnnotations;

namespace UoHCateringServices.Models
{
    public class User
    {
        [Key]
        public Guid User_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
