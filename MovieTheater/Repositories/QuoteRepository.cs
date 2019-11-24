using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MovieTheater.Helpers;
using MovieTheater.Models;

namespace MovieTheater.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly MovieContext _context;

        public QuoteRepository()
        {
            _context = new MovieContext();
        }

        public IEnumerable<Quote> GetAllQuotes(ClaimsPrincipal user)
        {
            var quotes = _context.Quotes.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                quotes = quotes.Where(q => q.Movie.Category.User.UserName.Equals(user.Identity.Name));

            return quotes
                .Select(q => new Quote
                {
                    Id = q.Id,
                    Title = q.Title,
                    Text = q.Text,
                    MovieID = q.MovieID
                });
        }

        public Quote GetQuote(ClaimsPrincipal user, int id)
        {
            var quotes = _context.Quotes.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                quotes = quotes.Where(q => q.Movie.Category.User.UserName.Equals(user.Identity.Name));

            if (quotes.Count() == 0)
                return null;

            return quotes
                .Where(q => q.Id == id)
                .Select(q => new Quote
                {
                    Id = q.Id,
                    Title = q.Title,
                    Text = q.Text,
                    MovieID = q.MovieID
                })
                .FirstOrDefault();
        }

        public bool QuoteExists(int id)
        {
            return _context.Quotes
                .Any(q => q.Id == id);
        }
    }
}
