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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        /// <summary>
        /// We want to pass this implentation to all the base classes
        /// </summary>
        /// <param name="db"></param>
        public OrderHeaderRepository(ApplicationDbContext db) : base(db) //whatever db context we get here, is passed to the repository
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
           _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
           var orderFromDb = _db.OrderHeaders.FirstOrDefault(x=>x.Id == id);    
            if (orderFromDb  != null) 
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            if (id != 0)
            {
                var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    orderFromDb.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderFromDb.PaymentIntentId = paymentIntentId;
                    orderFromDb.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
