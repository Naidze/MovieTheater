using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface IQuoteRepository
    {
        IEnumerable<Quote> GetAllQuotes(ClaimsPrincipal user);

        Quote GetQuote(ClaimsPrincipal user, int id);

        bool QuoteExists(int id);
    }
}
