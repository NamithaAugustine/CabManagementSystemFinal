using System.ComponentModel.DataAnnotations;

namespace CabManagementSystems.Models.ViewModel
{
    public class PlaceViewModel
    {
        [Required]
        public Location From { get; set; }

        [Required]
        public Location To { get; set; }

        [Required]
        public int Distance { get; set; }
    }
}
