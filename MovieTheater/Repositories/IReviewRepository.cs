using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetAllReviews(ClaimsPrincipal user);

        Review GetReview(ClaimsPrincipal user, int id);

        bool ReviewExists(int id);
    }
}
