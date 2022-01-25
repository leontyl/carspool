using CarsPool.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Dal.Abstractions
{
    public interface IDriverRepository
    {
        public Task<Driver> Get(int id);
        public Task<IEnumerable<Driver>> GetAll();

        public Task<Driver> Create(Driver driver);
        public Task Delete(int id);

        public Task Update(Driver driver);
    }
}
