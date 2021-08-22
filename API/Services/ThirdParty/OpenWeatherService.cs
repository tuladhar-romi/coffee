using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Services.ThirdParty
{
  public class OpenWeatherService : IOpenWeatherService
  {
    private readonly OpenWeatherConfig _config;
    private readonly HttpClient _httpClient;
    public OpenWeatherService(OpenWeatherConfig config, IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient();
      _config = config;
    }

    public async Task<double?> GetTempreatureAsync()
    {
      var url = string.Format(_config.Url, _config.Location, _config.ApiKey);

      var request = new HttpRequestMessage(HttpMethod.Get, url);
      request.Headers.Add("Accept", "application/json");

      var response = await _httpClient.SendAsync(request);
      OpenWeatherResponse openWeatherResponse = null;
      if (response.IsSuccessStatusCode)
      {
        var responseStream = await response.Content.ReadAsStringAsync();
        openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(responseStream);
      }
      return openWeatherResponse?.Main?.Temp;
    }
  }
}
