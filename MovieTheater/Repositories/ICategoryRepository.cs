using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();

        Category GetCategory(int id);

        bool CategoryExists(int id);
    }
}
