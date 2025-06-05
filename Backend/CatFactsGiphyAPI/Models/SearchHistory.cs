using System.ComponentModel.DataAnnotations;

namespace CatFactsGiphyAPI.Models
{
    public class SearchHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime SearchDate { get; set; }

        [Required]
        public string FactText { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string QueryWords { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string GifUrl { get; set; } = string.Empty;
    }
}