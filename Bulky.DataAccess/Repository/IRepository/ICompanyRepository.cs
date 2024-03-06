using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    /// <summary>
    /// Here we are implenting the IRepository, and we know what class we are implementing.
    /// The ICategoryRepository inteface implements all the base functionaility for all the repository,
    /// and then added to this is an Update and Save for the Category repository.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
        // This was move to UnitOfWWork
        //void Save();
    }
}
