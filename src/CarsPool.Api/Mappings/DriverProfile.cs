using AutoMapper;
using CarsPool.Api.Models;
using CarsPool.Dal.Entities;

namespace CarsPool.Api.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<DriverRequestModel, Driver>();
            CreateMap<Driver, DriverResponseModel>();
        }
    }
}
