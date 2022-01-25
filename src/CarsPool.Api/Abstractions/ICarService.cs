using CarsPool.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Api.Abstractions
{
    public interface ICarService
    {
        public Task<CarResponseModel> Create(CarRequestModel car);

        public Task<CarResponseModel> GetById(int carId);

        public Task<IEnumerable<CarResponseModel>> GetAll();

        public Task Delete(int id);

        public Task Update(int id, CarRequestModel car);
    }
}
