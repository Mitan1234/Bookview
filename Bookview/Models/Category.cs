using System.ComponentModel.DataAnnotations;

namespace Bookview.Models
{
    public class Category
    {
        public Category()
        {
            Books = new List<Book>(); // Inicjalizowanie listy
        }

        [Key]
        [Display(Name = "Identyfikator kategorii")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kategorii")]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać opis kategorii")]
        [Display(Name = "Opis kategorii")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę podać nazwę ikony")]
        [Display(Name = "Ikona kategorii")]
        public string Icon { get; set; } = string.Empty;

        public virtual ICollection<Book> Books { get; set; }
    }
}
