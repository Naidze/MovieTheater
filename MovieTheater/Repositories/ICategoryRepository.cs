using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories(ClaimsPrincipal user);

        Category GetCategory(ClaimsPrincipal user, int id);

        bool CategoryExists(int id);
    }
}
