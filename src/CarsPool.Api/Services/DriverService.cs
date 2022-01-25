using AutoMapper;
using CarsPool.Api.Abstractions;
using CarsPool.Api.Models;
using CarsPool.Dal.Abstractions;
using CarsPool.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Api.Services
{
    public class DriverService : IDriverService
    {
        private readonly IMapper _mapper;
        private readonly IDriverRepository _driverRepository;

        public DriverService(IMapper mapper, IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        public async Task<DriverResponseModel> Create(DriverRequestModel driver)
        {
            var driverEntity = _mapper.Map<Driver>(driver);
            var resultEntity = await _driverRepository.Create(driverEntity);
            return _mapper.Map<DriverResponseModel>(resultEntity);
        }

        public async Task<DriverResponseModel> GetById(int carId)
        {
            var resultEntity = await _driverRepository.Get(carId);
            return _mapper.Map<DriverResponseModel>(resultEntity);
        }

        public async Task<IEnumerable<DriverResponseModel>> GetAll()
        {
            var resultEntities = await _driverRepository.GetAll();
            return _mapper.Map<IEnumerable<DriverResponseModel>>(resultEntities);
        }

        public async Task Delete(int id)
        {
            await _driverRepository.Delete(id);
        }

        public async Task Update(int id, DriverRequestModel driver)
        {
            var driverEntity = _mapper.Map<DriverRequestModel, Driver>(driver, opt =>
            {
                opt.AfterMap((src, dst) => dst.Id = id);
            });
            await _driverRepository.Update(driverEntity);
        }
    }
}
