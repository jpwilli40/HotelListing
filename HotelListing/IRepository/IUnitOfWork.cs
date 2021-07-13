using HotelListing.Controllers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.IRepository
{
    public interface IUnitOfWork : IDisposable //register for every generic repository relative to task
    {
            IGenericRepository<Country> Countries { get; }
            IGenericRepository<Hotel> Hotels { get; }

            Task Save();  //actually saves the changes.  Before, the changes are just staged
    }
}
