using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystems.Models
{
    public enum CarModel
    {
        Suv,
        Sedan,
        Economic,
        AutoRickshaw,
        Pink
    }
    //public enum Gender
    //{
    //    Male,
    //    Female
    //}
    public class DriverDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime Dob { get; set; }

        public CarModel Car { get; set; }

        public Gender Gender { get; set; }

        public string CarNumber { get; set; }

        public string CarName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public IEnumerable<Booking> Bookings { get; set; }
    }
}
