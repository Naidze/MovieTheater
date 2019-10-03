using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Cinema> Cinemas { get; set; }
    }
}
