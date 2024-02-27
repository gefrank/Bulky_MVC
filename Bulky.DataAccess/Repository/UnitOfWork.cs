﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    /// <summary>
    /// UnitOfWork implements the repositories and the IUnitOfWork contract forces the implentation of Product and Category
    /// as well as Save
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            // UnitOfWork has the implementation for CategoryRepository and ProductRepository
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }
        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
