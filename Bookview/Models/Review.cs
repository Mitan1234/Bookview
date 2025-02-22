using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookview.Models
{
    public class Review
    {
        public Review()
        {
            Content = string.Empty;
            AddedDate = DateTime.Now;
        }

        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Treść recenzji jest wymagana")]
        [StringLength(1000, ErrorMessage = "Recenzja może mieć maksymalnie 1000 znaków")]
        public string Content { get; set; }

        [Range(1, 5, ErrorMessage = "Ocena musi być w zakresie 1-5")]
        public int Rating { get; set; }

        public DateTime AddedDate { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}
