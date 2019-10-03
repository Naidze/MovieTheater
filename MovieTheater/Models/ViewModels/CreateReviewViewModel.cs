using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.ViewModels
{
    public class CreateReviewViewModel
    {
        [Range(1, 5)]
        public int Stars { get; set; }
        public string Comment { get; set; }
        public int MovieID { get; set; }
    }
}
