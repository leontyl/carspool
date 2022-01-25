using CarsPool.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Dal.Abstractions
{
    public interface ICarRepository
    {
        public Task<Car> Get(int id);
        public Task<IEnumerable<Car>> GetAll();

        public Task<Car> Create(Car car);
        public Task Delete(int id);

        public Task Update(Car car);
    }
}
