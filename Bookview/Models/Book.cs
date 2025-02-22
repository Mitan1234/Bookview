using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Bookview.Models
{
    public class Book
    {
        public Book()
        {
            Title = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
            CoverImage = string.Empty;
            Reviews = new List<Review>();
        }

        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(255, ErrorMessage = "Tytuł może mieć maksymalnie 255 znaków")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany")]
        [StringLength(255, ErrorMessage = "Autor może mieć maksymalnie 255 znaków")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(2000, ErrorMessage = "Opis może mieć maksymalnie 2000 znaków")]
        public string Description { get; set; }

        [Display(Name = "Okładka książki")]
        public string CoverImage { get; set; } = string.Empty;

        [NotMapped] // To pole nie trafia do bazy danych - tylko do uploadu zdjęć
        [Display(Name = "Dodaj okładkę")]
        public IFormFile? CoverImageFile { get; set; } // Dodano "?"

        public int? CategoryId { get; set; } // `CategoryId` może być NULL

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
