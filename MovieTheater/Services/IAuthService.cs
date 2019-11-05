using MovieTheater.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Services
{
    public interface IAuthService
    {
        Task<string> RequestToken(LoginViewModel model);
    }
}
