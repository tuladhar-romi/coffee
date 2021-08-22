using API.Data.Repositories;
using API.Entities;
using API.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests
{
  public class CoffeeServiceTests
  {
    private readonly CoffeeService _coffeeService;
    private readonly Mock<IOrderRepository> _orderRepository = new();

    public CoffeeServiceTests()
    {
      _orderRepository.Setup(x => x.Add(It.IsAny<Order>())).Verifiable();
      _orderRepository.Setup(x => x.SaveChangesAsync()).Verifiable();
      _coffeeService = new CoffeeService(_orderRepository.Object);
    }

    [Fact]
    public async void GetCoffee_ShouldReturnCoffeeResponse_WhenHappyDays()
    {
      //arrange
      _orderRepository.Setup(x => x.GetOrderCountAsync()).ReturnsAsync(2);

      //act
      var response = await _coffeeService.GetCoffeeAsync();

      //assert
      response.Message.Should().Be("Your piping hot coffee is ready");
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
