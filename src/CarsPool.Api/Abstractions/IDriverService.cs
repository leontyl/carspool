using CarsPool.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Api.Abstractions
{
    public interface IDriverService
    {
        public Task<DriverResponseModel> Create(DriverRequestModel car);

        public Task<DriverResponseModel> GetById(int carId);

        public Task<IEnumerable<DriverResponseModel>> GetAll();

        public Task Delete(int id);

        public Task Update(int id, DriverRequestModel driver);
    }
}
