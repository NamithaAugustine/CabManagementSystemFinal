using System.ComponentModel.DataAnnotations;

namespace CabManagementSystems.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50,MinimumLength =3)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Gender Gender { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(25)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(25)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
