using System.Text.RegularExpressions;
using Flightmanager.Server.Models;

namespace Flightmanager.Server.Controllers;

public class CheckDate
{
    public bool IsValid(Flight flight)
    {
        return IsDepartureAndDestinationDifferent(flight)
               && IsDepartureTimeInFuture(flight)
               && AreLocationsValid(flight)
               && IsFlightNumberValid(flight);
    }
    
    private bool IsDepartureAndDestinationDifferent(Flight flight)
    {
        return !flight.Departure.Equals(flight.Destination);
    }

    private bool IsDepartureTimeInFuture(Flight flight)
    {
        return flight.DepartureTime > DateTime.Now ;
    }

    private bool AreLocationsValid(Flight flight)
    {
        return !flight.Departure.Any(char.IsDigit) && !flight.Destination.Any(char.IsDigit);
    }

    private bool IsFlightNumberValid(Flight flight)
    {
        string pattern = @"^[A-Z]{2,3}\d{1,4}[A-Z]?$";
        return Regex.IsMatch(flight.FlightNumber, pattern);
    }
}