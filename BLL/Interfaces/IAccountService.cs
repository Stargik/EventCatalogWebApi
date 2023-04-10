using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task RegisterUser(UserModel userModel);
        Task<string> LoginUser(UserLoginModel userLoginModel);
    }
}
