using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.ViewModels
{
    public class CreateCategoryViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Movie[] Movies { get; set; }
    }
}
