using System;

namespace API.Models
{
  public class CoffeeResponse
  {
    public string Message { get; set; }
    public DateTimeOffset Prepared { get; set; }
  }
}
