using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bookview.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Reviews = new List<Review>();
        }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię może mieć maksymalnie 50 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100, ErrorMessage = "Nazwisko może mieć maksymalnie 100 znaków")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
