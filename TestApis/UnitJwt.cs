using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Flightmanager.Login.Controllers;
using Flightmanager.Login.Data;
using Flightmanager.Login.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Flightmanager.Server.identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Assert = Xunit.Assert;

namespace TestApis;

public class UnitJwt
{
    [Fact]
    public void TestJwtTokenAndRolePolicy()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<UserApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        var user = new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password", "$2a$10$vwjDYzovdAjC1NOElQmSHO")
        };

        var mockHttpContext = new Mock<HttpContext>();
        var mockResponse = new Mock<HttpResponse>();
        var mockCookies = new Mock<IResponseCookies>();

        mockResponse.SetupGet(r => r.Cookies).Returns(mockCookies.Object);
        mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

        using (var context = new UserApiContext(options))
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        // Act
        UserDto userDto;
        using (var context = new UserApiContext(options))
        {
            var controller = new Usercontroler(context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            userDto = new UserDto { Username = "admin", PasswordHash = "password" };
            var result = controller.Login(userDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            dynamic tokenObject = result.Value;
            string json = JsonConvert.SerializeObject(tokenObject);

            var data = JsonConvert.DeserializeAnonymousType(json, new { token = "" });
            
            string token = data.token;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var usernameClaim = jwtToken.Claims.First(c => c.Type == ClaimTypes.Name);
            Assert.Equal(userDto.Username, usernameClaim.Value);

            var roleClaim = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role);
            Assert.Equal(IdentityData.AdminUserClaimName, roleClaim.Value);
        }
    }
}

   


   
