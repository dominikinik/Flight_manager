# Flightmanager

Flightmanager is an application for managing flights. The application consists of two main parts: `Flightmanager.Login` and `Flightmanager.Server`.

## Flightmanager.Login

`Flightmanager.Login` is an authorization service that allows for user registration and login. Users can also refresh their tokens using a special endpoint.

### Endpoints

- `POST /api/usercontroler/register`: Register a new user.
- `POST /api/usercontroler/login`: User login.
- `GET /api/usercontroler/ref_refresh_token`: Refresh user token.

## Flightmanager.Server

`Flightmanager.Server` is a flight management service. It allows for adding, updating, deleting, and retrieving flights.

### Endpoints

- `POST /flight/addFlight`: Add a new flight.
- `PUT /flight/updateFlight`: Update a flight.
- `DELETE /flight/delateFlight`: Delete a flight.
- `GET /flight/getFlight`: Retrieve flight information.
- `GET /flight/getAllFlight`: Retrieve information about all flights.

### Flight Validation in Flightmanager

The `CheckDate` class in the `Flightmanager.Server.Controllers` namespace is responsible for validating the details of a flight. It checks the following conditions:

1. **Departure and Destination are Different**: The `IsDepartureAndDestinationDifferent` method checks if the departure and destination locations of the flight are different. A flight is considered invalid if both locations are the same.

2. **Departure Time is in the Future**: The `IsDepartureTimeInFuture` method checks if the departure time of the flight is in the future. A flight is considered invalid if the departure time is in the past.

3. **Locations are Valid**: The `AreLocationsValid` method checks if the departure and destination locations do not contain any digits. A flight is considered invalid if either location contains a digit.

4. **Flight Number is Valid**: The `IsFlightNumberValid` method checks if the flight number matches a specific pattern. The pattern is defined as two to three uppercase letters, followed by one to four digits, optionally followed by an uppercase letter. A flight is considered invalid if the flight number does not match this pattern.



## Flightmanager.UI

Simple UI in Angular that shows API endpoints as a web application. You can run it by typing npm install then  ng serve --open in the terminal, located in Flightmanager.UI directory.

## Testing

The application have Flightmanager.Tests with unit tests.

