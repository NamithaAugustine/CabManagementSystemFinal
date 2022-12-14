using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystems.Models
{
    public class Place
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public Location From { get; set; }

        [Required]
        public Location To { get; set; }

        [Required]
        public int Distance { get; set; }
    }
}
