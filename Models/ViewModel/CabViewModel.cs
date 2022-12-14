using System.ComponentModel.DataAnnotations;

namespace CabManagementSystems.Models.ViewModel
{
    public class CabViewModel
    {
        public Model model { get; set; }

        [Required]
        [StringLength(50,MinimumLength =10)]
        public string CarNumber { get; set; }
    }
}
