using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace API.Controllers
{
  [ApiController]
  [Route("api/brew-coffee")]
  public class CoffeeController : ControllerBase
  {
    private readonly ICoffeeService _coffeeService;
    private readonly IDateTimeService _dateTimeService;

    private const int AprilFirstMonth = 4;
    private const int AprilFirstDay = 1;

    public CoffeeController(ICoffeeService coffeeService, IDateTimeService dateTimeService)
    {
      _coffeeService = coffeeService;
      _dateTimeService = dateTimeService;
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CoffeeResponse))]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    [SwaggerResponse(StatusCodes.Status418ImATeapot)]
    public async Task<ActionResult<CoffeeResponse>> Get()
    {
      var now = _dateTimeService.Now();
      if (now.Month == AprilFirstMonth && now.Day == AprilFirstDay)
        return StatusCode(StatusCodes.Status418ImATeapot);

      var response = await _coffeeService.GetCoffeeAsync();
      if (response == null)
        return StatusCode(StatusCodes.Status503ServiceUnavailable);

      return Ok(response);
    }
  }
}
