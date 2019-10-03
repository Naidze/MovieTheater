using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.ViewModels
{
    public class CreateMovieViewModel
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Year { get; set; }
        public double? Rating { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
    }
}
