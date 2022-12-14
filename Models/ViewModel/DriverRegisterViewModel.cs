using System.ComponentModel.DataAnnotations;

namespace CabManagementSystems.Models.ViewModel
{
    public class DriverRegisterViewModel
    {
        [Required]
        [StringLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public CarModel Car { get; set; }

        public Gender Gender { get; set; }

        public string CarNumber { get; set; }

        public string CarName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }



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

