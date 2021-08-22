using API.Data.Repositories;
using API.Models;
using API.Services.ThirdParty;
using System;
using System.Threading.Tasks;

namespace API.Services
{
  public class CoffeeService : ICoffeeService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IOpenWeatherService _openWeatherService;

    private const int ColdCoffeeTempreatureThreshold = 30;
    private const string HotCoffeeMessage = "Your piping hot coffee is ready";
    private const string ColdCoffeeMessage = "Your refreshing iced coffee is ready";

    public CoffeeService(IOrderRepository orderRepository, IOpenWeatherService openWeatherService)
    {
      _orderRepository = orderRepository;
      _openWeatherService = openWeatherService;
    }
    public async Task<CoffeeResponse> GetCoffeeAsync()
    {
      var tempreature = await _openWeatherService.GetTempreatureAsync();
      string message = HotCoffeeMessage;
      if (tempreature.HasValue && tempreature.Value > ColdCoffeeTempreatureThreshold)
        message = ColdCoffeeMessage;

      var order = new Entities.Order()
      {
        OrderTimestamp = DateTimeOffset.Now
      };
      _orderRepository.Add(order);
      await _orderRepository.SaveChangesAsync();
      var orderCount = await _orderRepository.GetOrderCountAsync();

      if (orderCount % 5 == 0) return null;

      return new()
      {
        Message = message,
        Prepared = order.OrderTimestamp
      };
    }
  }
}
