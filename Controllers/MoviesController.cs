using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MoviesApi.Dtos;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required");
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png are Allowed");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max Allowed Size for poster is 1 MB!");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre ID!");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _moviesService.AddMovie(movie);

            return Ok(movie);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAllMovies();
            var data = _mapper.Map<IEnumerable<movieDetailsDto>>(movies);
            
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyIdAsync(int id)
        {
            var movie = await _moviesService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            var data = _mapper.Map<movieDetailsDto>(movie);
            
            return Ok(data);
        }
        [HttpGet("GetbyGenreId")]
        public async Task<IActionResult> GetbyGenreIdAsync(byte genreId)
        {
            var movie = await _moviesService.GetAllMovies(genreId);
            if (movie == null) return NotFound();
            var data = _mapper.Map<IEnumerable<movieDetailsDto>>(movie);
            return Ok(data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Not valid Genre ");
            if(dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Not Allowed Extension!");
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("File is too big!");
                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();
            }
            movie.Title=dto.Title;
            movie.Description=dto.Description;
            movie.Rate=dto.Rate;
            movie.Year=dto.Year;
            movie.GenreId= dto.GenreId;

            _moviesService.UpdateMovie(movie);
            return Ok(movie);
            

        }
        [HttpDelete("{id}")]
        public async Task <IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetMovieById(id);
            if (movie == null) return NotFound();
            _moviesService.DeleteMovie(movie);

            return Ok(movie);
        }
    }
}
