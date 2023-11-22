using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;
        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateAsync(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre DeleteAsync(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> IsValidGenre(byte id)
        {
            return _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre UpdateAsync(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
