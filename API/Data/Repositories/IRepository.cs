using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
  public interface IRepository<TEntity> where TEntity : class
  {
    void Add(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
  }
}
