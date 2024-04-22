using Flightmanager.Server.Controllers;
using Flightmanager.Server.Data;
using Flightmanager.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;

namespace TestApis;

public class UnitTestServer
{
    [Fact]
    public void AddFlight_ReturnsOkResult_WhenFlightDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "AddFlightTestDatabase")
            .Options;

        var flight = new Flight
        {
            Id = 1,
            FlightNumber = "FN123",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            var result = controller.AddFlight(flight);

            // Assert
            Assert.IsType<OkObjectResult>(result.Value);
        }
    }

    [Fact]
    public void AddFlight_ReturnsConflict_WhenFlightExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "AddFlightTestDatabase")
            .Options;

        var flight = new Flight
        {
            Id = 1,
            FlightNumber = "FN123",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            controller.AddFlight(flight);
            var result = controller.AddFlight(flight);

            // Assert
            Assert.IsType<ConflictResult>(result.Value);
        }
    }

    [Fact]
    public void UpdateFlight_ReturnsOkResult_WhenFlightExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "UpdateFlightTestDatabase")
            .Options;

        var flight = new Flight
        {
            Id = 1,
            FlightNumber = "FN123",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        var updatedFlight = new Flight
        {
            Id = 1,
            FlightNumber = "FN124",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            controller.AddFlight(flight);
            var result = controller.UpdateFlight(updatedFlight);

            // Assert
            Assert.IsType<OkResult>(result.Value);
        }
    }

    [Fact]
    public void UpdateFlight_ReturnsNotFound_WhenFlightDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "UpdateFlightNotFoundTestDatabase")
            .Options;

        var updatedFlight = new Flight {
            Id = 1,
            FlightNumber = "FN124",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };;

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            var result = controller.UpdateFlight(updatedFlight);

            // Assert
            Assert.IsType<NotFoundResult>(result.Value);
        }
    }

    [Fact]
    public void UpdateFlight_ReturnsNotFound_WhenFlightIsBad()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "UpdateFlightNotFoundTestDatabase")
            .Options;

        var updatedFlight = new Flight
        {
            Id = 1,
            FlightNumber = "FN124",
            AirplaneType = "Boeing 747",
            Departure = "London",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            var result = controller.UpdateFlight(updatedFlight);

            // Assert
            Assert.IsType<BadRequestResult>(result.Value);
        }
    }

    [Fact]
    public void DeleteFlight_ReturnsOkResult_WhenFlightExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "DeleteFlightTestDatabase")
            .Options;

        var flight = new Flight
        {
            Id = 1,
            FlightNumber = "FN123",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            controller.AddFlight(flight);
            var result = controller.DeleteFlight(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Value);
        }
    }

    [Fact]
    public void DeleteFlight_ReturnsNotFound_WhenFlightDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "DeleteFlightNotFoundTestDatabase")
            .Options;

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            var result = controller.DeleteFlight(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Value);
        }
    }

    [Fact]
    public void GetFlights_ReturnsOkResult_WhenFlightExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "GetFlightsTestDatabase")
            .Options;

        var flight = new Flight
        {
            Id = 1,
            FlightNumber = "FN123",
            AirplaneType = "Boeing 747",
            DepartureTime = DateTime.Now.AddDays(1),
            Departure = "New York",
            Destination = "London"
        };

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            controller.AddFlight(flight);
            var result = controller.GetFlights(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Value);
        }
    }

    [Fact]
    public void GetFlights_ReturnsNotFound_WhenFlightDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "GetFlightsNotFoundTestDatabase")
            .Options;

        // Act
        using (var context = new ApiContext(options))
        {
            var controller = new FlightController(context);
            var result = controller.GetFlights(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Value);
        }
    }

    
}
