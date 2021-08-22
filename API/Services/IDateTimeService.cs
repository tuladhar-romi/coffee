using System;

namespace API.Services
{
  public interface IDateTimeService
  {
    DateTimeOffset Now();
  }

  public class DateTimeService : IDateTimeService
  {
    public DateTimeOffset Now()
    {
      return DateTimeOffset.Now;
    }
  }

}
