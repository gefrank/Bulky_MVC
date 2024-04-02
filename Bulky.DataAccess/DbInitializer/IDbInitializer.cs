using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        /// <summary>
        /// This will be responsible for creating the Admin User and Roles of the website
        /// </summary>
        void Initialize();
    }
}
