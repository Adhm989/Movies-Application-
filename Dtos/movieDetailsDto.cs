using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class movieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public byte[] Poster { get; set; }
        public byte GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
