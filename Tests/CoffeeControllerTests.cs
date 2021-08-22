using API.Controllers;
using API.Models;
using API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
  public class CoffeeControllerTests
  {
    private readonly Mock<ICoffeeService> _coffeeServiceMock = new();
    private readonly Mock<IDateTimeService> _dateTimeServiceMock = new();
    private readonly CoffeeController _coffeeController;

    public CoffeeControllerTests()
    {
      _dateTimeServiceMock.Setup(x => x.Now()).Returns(DateTimeOffset.Now);
      _coffeeController = new(_coffeeServiceMock.Object, _dateTimeServiceMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturn_ServiceNotAvailable_WhenServiceReturnsNull()
    {
      //Arrange
      _coffeeServiceMock.Setup(svc => svc.GetCoffeeAsync()).ReturnsAsync((CoffeeResponse)null);

      //act
      var result = (await _coffeeController.Get()).Result as StatusCodeResult;

      //assert
      Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnImATeapot_WhenFirstApril()
    {
      //Arrange
      _dateTimeServiceMock.Setup(x => x.Now()).Returns(new System.DateTimeOffset(2021, 4, 1, 10, 10, 0, new System.TimeSpan(10, 0, 0)));

      //act
      var result = (await _coffeeController.Get()).Result as StatusCodeResult;

      //assert
      Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnCoffeeResponse_WhenHappyDays()
    {
      //Arrange
      var response = new CoffeeResponse { Message = "Your piping hot coffee is ready", Prepared = DateTimeOffset.Now };
      _coffeeServiceMock.Setup(svc => svc.GetCoffeeAsync()).ReturnsAsync(response);

      //act
      var actionResult = await _coffeeController.Get();

      //assert
      var result = actionResult.Result as OkObjectResult;
      var coffeeResponse = result.Value as CoffeeResponse;
      coffeeResponse.Message.Should().Be(response.Message);
    }
  }
}
