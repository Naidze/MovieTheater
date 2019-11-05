using MovieTheater.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoriesRepository;

        public CategoryService(ICategoryRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public IEnumerable<object> GetAllCategories()
        {
            return _categoriesRepository.GetAllCategories();
        }

        public object GetCategory(int id)
        {
            return _categoriesRepository.GetCategory(id);
        }

        public bool CategoryExists(int id)
        {
            return _categoriesRepository.CategoryExists(id);
        }
    }
}
