using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.ViewModels
{
    public class CreateCinemaViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Capacity { get; set; }
    }
}
