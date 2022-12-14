
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CabManagementSystems.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class ApplicationUser : IdentityUser
    {
        [StringLength(20,MinimumLength =2)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(20,MinimumLength=2)]
        [Required]
        public string LastName { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }

        public Gender Gender { get; set; }

    }
}

