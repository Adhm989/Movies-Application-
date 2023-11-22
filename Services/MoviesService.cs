using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;
        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Movie> AddMovie(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return (movie);
        }

        public Movie DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();

            return (movie);
        }

        public async Task<IEnumerable<Movie>> GetAllMovies(byte genreId = 0)
        {
            return await _context.Movies.Include(m => m.Genre)
                .Where(m => m.GenreId == genreId || genreId == 0)
                .OrderByDescending(m => m.Rate)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieByGenreId(byte id)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            
        }

        public Movie UpdateMovie(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();

            return movie;
        }
    }
}
