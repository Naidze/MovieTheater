using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Services
{
    public interface ICategoryService
    {
        IEnumerable<object> GetAllCategories();

        object GetCategory(int id);

        bool CategoryExists(int id);
    }
}
