namespace Flightmanager.Server.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public string AirplaneType { get; set; }
    }
}