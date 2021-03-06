﻿using Microsoft.AspNetCore.Identity;
using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> EmailExists(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> UserExists(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }
    }
}
