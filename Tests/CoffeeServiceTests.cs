using API.Data.Repositories;
using API.Entities;
using API.Services;
using API.Services.ThirdParty;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests
{
  public class CoffeeServiceTests
  {
    private readonly CoffeeService _coffeeService;
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IOpenWeatherService> _openWeatherService = new();

    public CoffeeServiceTests()
    {
      _orderRepository.Setup(x => x.Add(It.IsAny<Order>())).Verifiable();
      _orderRepository.Setup(x => x.SaveChangesAsync()).Verifiable();
      _openWeatherService.Setup(x => x.GetTempreatureAsync()).ReturnsAsync(22);
      _coffeeService = new CoffeeService(_orderRepository.Object, _openWeatherService.Object);
    }

    [Theory]
    [InlineData(25, "Your piping hot coffee is ready")]
    [InlineData(32, "Your refreshing iced coffee is ready")]
    public async void GetCoffee_ShouldReturnCoffeeResponse_WithMessageDependingOnWeather(double temperature, string message)
    {
      //arrange
      _orderRepository.Setup(x => x.GetOrderCountAsync()).ReturnsAsync(2);
      _openWeatherService.Setup(x => x.GetTempreatureAsync()).ReturnsAsync(temperature);

      //act
      var response = await _coffeeService.GetCoffeeAsync();

      //assert
      response.Message.Should().Be(message);
      _orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
      _orderRepository.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async void GetCoffee_ShouldReturnNull_ForEveryFifthOrder()
    {
      //arrange
      _orderRepository.Setup(x => x.GetOrderCountAsync()).ReturnsAsync(25);

      //act
      var response = await _coffeeService.GetCoffeeAsync();

      //assert
      response.Should().BeNull();
      _orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
      _orderRepository.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
    }

  }
}
