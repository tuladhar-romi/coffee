using API.Data.Repositories;
using API.Models;
using System;
using System.Threading.Tasks;

namespace API.Services
{
  public class CoffeeService : ICoffeeService
  {
    private readonly IOrderRepository _orderRepository;


    public CoffeeService(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }
    public async Task<CoffeeResponse> GetCoffeeAsync()
    {
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
        Message = "Your piping hot coffee is ready",
        Prepared = order.OrderTimestamp
      };
    }
  }
}
