using FluentAssertions;
using Inside.Controllers;
using InsideUnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Moq;
using Xunit;
using Inside.Services;
using System.Configuration;
using Inside.Models;
using Inside.Data;
using Microsoft.Extensions.Configuration;

namespace InsideUnitTests
{
    
    /// <summary>
    /// Arrange
    /// Act
    /// Assert
    /// </summary>

    public class UnitTests
    {
        IConfiguration _configuration;
        [Fact]
        public void LoginModel()
        {
            var loginModel = new Inside.Models.UserLoginModel();
            Assert.Contains(loginModel.Username, loginModel.Password);
        }

        [Fact]
        public void NotEmptyName()
        {
            // Arrange
            var loginModel_1 = new UserLoginModel
            {
                Password = "SomePassword",
                Username = String.Empty
            };
            var loginModel_2 = new UserLoginModel
            {
                Password = String.Empty,
                Username = "AnyUsername"
            };
            var mockContext = new Mock<InsideDbContext>();
            var Jwt = new JwtWorker(_configuration);
            var controller = new InsideAuthController(mockContext.Object, Jwt);

            // Act
            //var result = controller.Login(loginModel);
            // Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => controller.Login(loginModel_1)
                );
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => controller.Login(loginModel_2)
                );

        }

        public void NotEmptyPassword()
        {

        }
    }

    public class IntegrationTesting
    {
        [Fact]
        public async Task Get_OnSuccess_returnStatusCode200()
        {
            // Arrange
            var sut = new InsidePublicController(new Inside.Data.InsideDbContext());

            //Act
            var result = await sut.GetMessages();

            //Assert
            //FluentAssertion provide an elegant way to assertcontroller response code
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
                        
        }


    }

    public class SystemTesting
    {

    }
    public class AcceptanceTesting
    {

    }
}