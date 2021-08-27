using CoreLayer.ViewModels.Auth;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users
{
    public interface IUserService
    {
        Task<bool> IsUserExist(string userName);
        Task<bool> IsUserExist(long userId);
        Task<bool> RegisterUser(RegisterViewModel registerModel);
        Task<User> LoginUser(LoginViewModel loginModel);
    }
}

