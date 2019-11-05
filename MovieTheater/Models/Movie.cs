using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Year { get; set; }
        public double? Rating { get; set; }
        public string ImageURL { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int ReviewID { get; set; }
        public virtual Review Review { get; set; }

        public virtual IEnumerable<Quote> Quotes { get; set; }
    }
}
