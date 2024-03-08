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
            _db.Products.Include(x => x.Category);
        }

        public void Add(T entity)
        {
           dbSet.Add(entity);
            // The above replaces generically
            // _db.Categories.Add(entity);
        }

        /// <summary>
        /// Added tracked so EF doesn't automatically change a returned entity unless specifically call an update to it.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <param name="tracked"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;

            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                //Adds the navigation to the FK Tables
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }
    

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }            
            if (!string.IsNullOrEmpty(includeProperties))
            {
                //Adds the navigation to the FK Tables
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

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
