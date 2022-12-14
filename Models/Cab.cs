namespace CabManagementSystems.Models
{
    public enum Model { 
        SUV,
        Economic,
        Sedan
    }

    public class Cab
    {
        public int Id { get; set; }

        public Model model { get; set; }

        public string CarNumber { get; set; }


    }
}
