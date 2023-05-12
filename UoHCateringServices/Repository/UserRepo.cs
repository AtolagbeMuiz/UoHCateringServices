using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UoHCateringServices.Data;
using UoHCateringServices.Interfaces;
using UoHCateringServices.Models;

namespace UoHCateringServices.Repository
{
    public class UserRepo : IUserRepo
    {
        private DBContext _context;
        public UserRepo(DBContext context)
        {
            _context = context;
        }
        public object AddUser(User user)
        {
            if (user != null)
            {
                var isUserExist = _context.User.Where(x => x.Email == user.Email).FirstOrDefault();

                if (isUserExist == null)
                {
                    _context.User.Add(user);
                    _context.SaveChanges();

                }
                else
                {
                    return "user already exist";
                }

                return user;
            }
            return null;
        }
    }
}
