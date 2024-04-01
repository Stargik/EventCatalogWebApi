using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Services
{
    public interface IAccountService
    {
        Task RegisterUser(UserModel userModel);
        Task<string> LoginUser(UserLoginModel userLoginModel);
    }
}
