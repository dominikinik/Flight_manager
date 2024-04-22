using Microsoft.AspNetCore.Mvc;
using Flightmanager.Server.Models;
using Flightmanager.Server.Data;
using Flightmanager.Server.identity;
using Microsoft.AspNetCore.Authorization;

namespace Flightmanager.Server.Controllers
{
    
    [Authorize]
    [Route("flight")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ApiContext _flightService;

        public FlightController(ApiContext flight)
        {
            _flightService = flight;
        }

        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpPost("addFlight")]
        public JsonResult AddFlight(Flight flight)
        {
            CheckDate checkDate = new CheckDate();
            if (!checkDate.IsValid(flight))
            {
                return new JsonResult(BadRequest());
            }

            var flightInDb = _flightService.Flights.Find(flight.Id);
            if (flightInDb != null)
            {
                return new JsonResult(Conflict());
            }

            _flightService.Flights.Add(flight);
            _flightService.SaveChanges();
            return new JsonResult(Ok(flight));
        }

        [HttpPut("updateFlight")]
        public JsonResult UpdateFlight(Flight flight)
        {
            CheckDate checkDate = new CheckDate();
            if (!checkDate.IsValid(flight))
            {
                return new JsonResult(BadRequest());
            }

            var flightInDb2 = _flightService.Flights.Find(flight.Id);
            if (flightInDb2 == null)
            {
                return new JsonResult(NotFound());
            }

            flightInDb2.FlightNumber = flight.FlightNumber;
            flightInDb2.DepartureTime = flight.DepartureTime;
            flightInDb2.Departure = flight.Departure;
            flightInDb2.Destination = flight.Destination;
            flightInDb2.AirplaneType = flight.AirplaneType;

            _flightService.SaveChanges();
            return new JsonResult(Ok());
        }

        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpDelete("delateFlight")]
        public JsonResult DeleteFlight(int id)
        {
            var flightInDb = _flightService.Flights.Find(id);
            if (flightInDb == null)
            {
                return new JsonResult(NotFound());
            }

            _flightService.Flights.Remove(flightInDb);
            _flightService.SaveChanges();
            return new JsonResult(Ok(flightInDb));
        }

        [HttpGet("getFlight")]
        public JsonResult GetFlights(int id)
        {
            var flightInDb = _flightService.Flights.Find(id);
            if (flightInDb == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(flightInDb));
        }

        [HttpGet("getAllFlight")]
        public JsonResult GetAll()
        {
            var result = _flightService.Flights.ToList();
            return new JsonResult(Ok(result));
        }
    }
}