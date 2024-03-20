using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        /// <summary>
        /// We want to pass this implentation to all the base classes
        /// </summary>
        /// <param name="db"></param>
        public OrderDetailRepository(ApplicationDbContext db) : base(db) //whatever db context we get here, is passed to the repository
        {
            _db = db;
        }
        public void Update(OrderDetail obj)
        {
           _db.OrderDetails.Update(obj);
        }
    }
}
