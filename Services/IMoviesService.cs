using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAllMovies(byte genreId=0);
        Task<Movie> GetMovieById(int id);
        Task<Movie> AddMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
        Movie DeleteMovie(Movie movie);
    }
}
