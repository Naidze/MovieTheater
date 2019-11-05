using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string userName);
        Task<bool> EmailExists(string email);
    }
}
