using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
