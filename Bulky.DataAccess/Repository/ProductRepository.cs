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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        /// <summary>
        /// We want to pass this implentation to all the base classes
        /// </summary>
        /// <param name="db"></param>
        public ProductRepository(ApplicationDbContext db) : base(db) //whatever db context we get here, is passed to the repository
        {
            _db = db;            
        }

        /// <summary>
        /// This was moved to UnitOfWork
        /// </summary>
        /// <param name="obj"></param>
        //public void Save()
        //{
        //   _db.SaveChanges();
        //}

        // Update is part of the IProductRepository contract and must be implemented
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(x=>x.Id == obj.Id);
            if (objFromDb != null)             
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;    
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.Category = obj.Category;
                objFromDb.Author = obj.Author;
                objFromDb.ProductImages = obj.ProductImages; //EF COre will handle all the updates to this table through this one call.
                //if (objFromDb.ImageUrl != null)
                //{
                //    objFromDb.ImageUrl = obj.ImageUrl;
                //}
                _db.Products.Update(objFromDb);
            }           
        }
    }
}
