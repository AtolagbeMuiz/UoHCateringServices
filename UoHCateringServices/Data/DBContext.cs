using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.Models;

namespace UoHCateringServices.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
        : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        
    }
}
