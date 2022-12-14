namespace CabManagementSystems.Models.ViewModel
{
    public class DriverEditViewModel
    {
        public DateTime Dob { get; set; }

        public CarModel Car { get; set; }

        public Gender Gender { get; set; }

        public string CarNumber { get; set; }

        public string CarName { get; set; }
    }
}
