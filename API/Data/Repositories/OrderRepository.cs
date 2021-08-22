using API.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
  public class OrderRepository : Repository<Order>, IOrderRepository
  {
    public OrderRepository(DataContext context) : base(context) { }
    public async Task<int> GetOrderCountAsync()
    {
      return (await GetAllAsync()).Count();
    }
  }
}
