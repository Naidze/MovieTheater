﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.ViewModels
{
    public class CreateQuoteViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int MovieID { get; set; }
    }
}
