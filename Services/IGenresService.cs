using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetById(byte id);
        Task<Genre> CreateAsync(Genre genre);
        Genre UpdateAsync(Genre genre);
        Genre DeleteAsync(Genre genre);
        Task<bool> IsValidGenre(byte id);
    }
}
