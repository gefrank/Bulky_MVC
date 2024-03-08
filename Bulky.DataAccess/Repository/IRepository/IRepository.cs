using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        // General syntax for get expression using FirstOrDefault
        T Get(Expression<Func<T, bool>> filter,string? includeProperties = null, bool tracked = false); //Added tracked so EF doesn't automatically change a returned entity unless specifically call an update to it..
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
