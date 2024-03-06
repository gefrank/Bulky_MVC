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
    /// The ICompanyRepository inteface implements all the base functionaility for all the repository,
    /// and then added to this is an Update and Save for the Company repository.
    /// </summary>
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
