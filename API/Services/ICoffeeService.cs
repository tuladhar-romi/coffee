using API.Models;
using System.Threading.Tasks;

namespace API.Services
{
  public interface ICoffeeService
  {
    Task<CoffeeResponse> GetCoffeeAsync();
  }
}
