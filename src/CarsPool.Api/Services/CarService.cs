using AutoMapper;
using CarsPool.Api.Abstractions;
using CarsPool.Api.Models;
using CarsPool.Dal.Abstractions;
using CarsPool.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsPool.Api.Services
{
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        public CarService(IMapper mapper, ICarRepository carRepository)
        {
            _mapper = mapper;
            _carRepository = carRepository;
        }

        public async Task<CarResponseModel> Create(CarRequestModel car)
        {
            var carEntity = _mapper.Map<Car>(car);
            var resultEntity = await _carRepository.Create(carEntity);
            return _mapper.Map<CarResponseModel>(resultEntity);
        }

        public async Task<CarResponseModel> GetById(int carId)
        {
            var resultEntity = await _carRepository.Get(carId);
            return _mapper.Map<CarResponseModel>(resultEntity);
        }

        public async Task<IEnumerable<CarResponseModel>> GetAll()
        {
            var resultEntities = await _carRepository.GetAll();
            return _mapper.Map<IEnumerable<CarResponseModel>>(resultEntities);
        }

        public async Task Delete(int id)
        {
            await _carRepository.Delete(id);
        }

        public async Task Update(int id, CarRequestModel car)
        {
            var carEntity = _mapper.Map<CarRequestModel, Car>(car, opt =>
            {
                opt.AfterMap((src, dst) => dst.Id = id);
            });

            await _carRepository.Update(carEntity);
        }
    }
}
