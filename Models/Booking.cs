using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public enum Location
{
    Phase1,
    Phase2,
    Kakanad,
    Edapally,
    Vytilla,
    Palarivatom,
    Thripunithura,
    Aroor
}

namespace CabManagementSystems.Models
{
    
    public class Booking
    {
        public int Id { get; set; }
        public global::Location From { get; set; }
        public global::Location To { get; set; }
        public DateTime Date { get; set; }

        public CarModel CarModelCar { get; set; }

        public bool DriverConfirmed { get; set; } = false;

        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public DriverDetails Driver { get; set; }
        [ForeignKey(nameof(Driver))]
        public string DriverId { get; set; }

        public bool? Payed { get; set; } = false;

        public int Distance { get; set; }

    }
}
