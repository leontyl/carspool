using AutoMapper;
using CarsPool.Api.Models;
using CarsPool.Dal.Entities;

namespace CarsPool.Api.Mappings
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CarRequestModel, Car>();
            CreateMap<Car, CarResponseModel>();
        }
    }
}
