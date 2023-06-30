using ScooterRent.API.Enums;

namespace ScooterRent.API.Entities
{
    public class Scooter : BaseEntity
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public ScooterState State { get; set; }
        public DateTime LastSenkronDate { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int ChargeState { get; set; }
        public decimal RentPrice { get; set; }
        public int Range { get; set; }
    }
}
