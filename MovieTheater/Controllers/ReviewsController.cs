using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;
using MovieTheater.Repositories;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;

        public ReviewsController(IReviewRepository reviewRepository, IMovieRepository movieRepository)
        {
            _context = new MovieContext();
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }

        // GET: api/Reviews
        [HttpGet]
        public ActionResult<IEnumerable<Review>> GetReviews()
        {
            return Ok(_reviewRepository.GetAllReviews(User));
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound("Review with id: '" + id + "' was not found.");

            return _reviewRepository.GetReview(User, id);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Review dbReview = _reviewRepository.GetReview(User, id);
            if (dbReview == null)
                return NotFound("Review: " + id + " not found");

            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_reviewRepository.ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(CreateReviewViewModel model)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Movie movie = _movieRepository.GetMovie(User, model.MovieID);
            if (movie == null)
                return NotFound("Movie " + model.MovieID + " not found.");

            Review review = new Review
            {
                Stars = model.Stars,
                Comment = model.Comment,
                MovieID = movie.Id
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Review review = _reviewRepository.GetReview(User, id);
            if (review == null)
                return NotFound("Review: " + id + " not found");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }
    }
}
