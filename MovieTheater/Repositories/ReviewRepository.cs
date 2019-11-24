using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MovieTheater.Helpers;
using MovieTheater.Models;

namespace MovieTheater.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieContext _context;

        public ReviewRepository()
        {
            _context = new MovieContext();
        }
        public IEnumerable<Review> GetAllReviews(ClaimsPrincipal user)
        {
            var reviews = _context.Reviews.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                reviews = reviews.Where(review => review.Movie.Category.User.UserName.Equals(user.Identity.Name));

            return reviews
                .Select(review => new Review
                {
                    
                    Id = review.Id,
                    Stars = review.Stars,
                    Comment = review.Comment,
                    MovieID = review.MovieID
                });
        }

        public Review GetReview(ClaimsPrincipal user, int id)
        {
            var reviews = _context.Reviews.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                reviews = reviews.Where(review => review.Movie.Category.User.UserName.Equals(user.Identity.Name));

            if (reviews.Count() == 0)
                return null;

            return reviews
                .Where(review => review.Id == id)
                .Select(review => new Review
                {

                    Id = review.Id,
                    Stars = review.Stars,
                    Comment = review.Comment,
                    MovieID = review.MovieID
                })
                .FirstOrDefault();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews
                .Any(review => review.Id == id);
        }
    }
}
