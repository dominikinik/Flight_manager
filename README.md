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

## Requirements

- .NET 6.0
- An IDE that supports .NET, e.g., JetBrains Rider, Visual Studio.

## Running

1. Clone the repository to your local environment.
2. Open the project in your IDE.
3. Run `Flightmanager.Login` and `Flightmanager.Server` (you can do this simultaneously using the `Multiple Startup Projects` feature in Visual Studio or its equivalent in other IDEs).
4. The application is now ready to use. You can test it using tools like Postman or Flightmanager.UI

## Flightmanager.UI

Simple UI in Angular that shows API endpoints as a web application. You can run it by typing npm install then  ng serve --open in the terminal, located in Flightmanager.UI directory.

## Testing

The application have Flightmanager.Tests with unit tests.

