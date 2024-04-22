using Xunit;
using Microsoft.EntityFrameworkCore;
using Flightmanager.Login.Data;
using Flightmanager.Login.Controllers;
using Flightmanager.Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Assert = Xunit.Assert;


namespace TestApis
{
    public class UnitTestUser
    {
        [Fact]
        public void RegisterTest()
        {
            //araange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            //act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context);
                var result = controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });
                Assert.IsType<OkObjectResult>(result.Result);
            }
        }

        [Fact]
        public void Register_ReturnsOkResult_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "RegisterTestDatabase")
                .Options;

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            // Act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object,
                    },
                };

                var result = controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });

                // Assert
                Assert.IsType<OkObjectResult>(result.Result);
            }
        }

        [Fact]
        public void Register_ReturnsConflict_WhenUserExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "RegisterConflictTestDatabase")
                .Options;

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            // Act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object,
                    },
                };

                controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });
                var result = controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });

                // Assert
                Assert.IsType<ConflictResult>(result.Result);
            }
        }

        [Fact]
        public void Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "LoginTestDatabase")
                .Options;

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();
            var mockCookies = new Mock<IResponseCookies>();

            mockResponse.SetupGet(r => r.Cookies).Returns(mockCookies.Object);
            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            // Act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object,
                    },
                };

                controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });
                var result = controller.Login(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void Login_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "LoginNotFoundTestDatabase")
                .Options;

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            // Act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object,
                    },
                };

                var result = controller.Login(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenPasswordIsIncorrect()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserApiContext>()
                .UseInMemoryDatabase(databaseName: "LoginBadRequestTestDatabase")
                .Options;

            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            // Act
            using (var context = new UserApiContext(options))
            {
                var controller = new Usercontroler(context)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = mockHttpContext.Object,
                    },
                };

                controller.Register(new UserDto { Username = "TestUser", PasswordHash = "TestPassword" });
                var result = controller.Login(new UserDto { Username = "TestUser", PasswordHash = "WrongPassword" });

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }
    }
}
        

    