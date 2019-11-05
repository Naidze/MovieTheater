using Microsoft.AspNetCore.Identity;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Services
{
    public interface IUserService
    {
        Task<bool> Login(LoginViewModel model);
        Task<IdentityResult> Register(RegisterViewModel model);

        IEnumerable<object> GetAllUsers();
    }
}
