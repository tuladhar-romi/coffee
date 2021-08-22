using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
  public interface IOrderRepository: IRepository<Order>
  {
    Task<int> GetOrderCountAsync();
  }
}
