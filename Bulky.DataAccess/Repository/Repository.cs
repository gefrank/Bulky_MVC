using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;


namespace Bulky.DataAccess.Repository
{
    // This class is generic as is the interface it implements.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        // internal the type or member can be accessed by the same class or a derived class.
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            // DbSet Class: A non-generic version of DbSet<TEntity> which can be used when the type of entity is not known at build time.
            // sets the generic type to the explict type to be used in the implementation
            // DbSet represents the db context and the entity to which the CRUD operation is to be performed.
            this.dbSet = _db.Set<T>();
            // code above is equivalent to this:
            // _db.Categories = dbSet
        }

        public void Add(T entity)
        {
           dbSet.Add(entity);
            // The above replaces generically
            // _db.Categories.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
