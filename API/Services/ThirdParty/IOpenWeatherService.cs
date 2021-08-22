using System.Threading.Tasks;

namespace API.Services.ThirdParty
{
  public interface IOpenWeatherService
  {
    Task<double?> GetTempreatureAsync();
  }
}
