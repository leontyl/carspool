using CarsPool.Api.Exceptions;
using CarsPool.Dal.Abstractions;
using CarsPool.Dal.Contexts;
using CarsPool.Dal.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPool.Dal.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarsPoolDbContext _dbContext;
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(CarsPoolDbContext dbContext, ILogger<CarRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Car> Get(int id)
        {
            _logger.LogDebug("Searching car by Id: {id}", id);

            using (_dbContext)
            {
                var result = await _dbContext.Cars.FindAsync(id);

                _logger.LogInformation("Searching car by Id: {id} completed. Result: {@result}", id, result);

                return result;
            }
        }

        public async Task<IEnumerable<Car>> GetAll()
        {
            _logger.LogDebug("Retrieving all cars");

            using (_dbContext)
            {
                var cars = _dbContext.Cars.ToList();

                _logger.LogDebug("Retrieving all cars result: {@cars}", cars);

                return cars;
            }
        }

        /// <summary>
        /// Creating car in db.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Car</returns>
        public async Task<Car> Create(Car car)
        {
            using (_dbContext)
            {
                _logger.LogDebug("Creating car: {@car}", car);

                try
                {
                    var result = await _dbContext.Cars.AddAsync(car);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogDebug("Creating car: {@car} result: {@result}", car, result);

                    return result.Entity;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error has occured during saving car {@car} in db", car);
                    throw;
                }

            }
        }

        public async Task Delete(int id)
        {
            using (_dbContext)
            {
                _logger.LogDebug("Deleting car by id: {id}", id);

                // We need to perform both select and delete in one transaction in order to avoid sittuation when car is removed by ohter request between select and delete
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var car = await _dbContext.Cars.FindAsync(id);

                        if (car == null)
                        {
                            // We have nothing to delete
                            throw new CarsPoolDalException($"Car with id {id} doesn't exist.") { ErrorCode = DalErrorCodes.EntityNotExists };
                        }

                        var carDriver = _dbContext.CarDriver.FirstOrDefault(carDriver => carDriver.CarId == id);

                        if (carDriver != null)
                        {
                            // If car was assigned to any driver, we remove this car from driver
                            var driver = await _dbContext.Drivers.FindAsync(carDriver.DriverId);
                            driver.CarId = 0;
                            _dbContext.CarDriver.Remove(carDriver);
                        }

                        _dbContext.Cars.Remove(car);

                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        _logger.LogDebug("Deleting car by id: {id} succeeded", id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred during deleting car by id: {id}", id);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task Update(Car car)
        {
            _logger.LogDebug("Updating car with id: {id} by {@car}", car.Id, car);

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var carToUpdate = await _dbContext.Cars.FindAsync(car.Id);

                        if (carToUpdate == null)
                        {
                            // We have nothing to update
                            throw new CarsPoolDalException($"Car with id {car.Id} doesn't exist.") { ErrorCode = DalErrorCodes.EntityNotExists };
                        }

                        carToUpdate.Manufacturer = car.Manufacturer;
                        carToUpdate.LicenceNumber = car.LicenceNumber;
                        carToUpdate.Color = car.Color;

                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        _logger.LogDebug("Updating car with id: {id} by {@car} succeeded.", car.Id, car);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred during updating car by id: {id}", car.Id);
                        transaction.Rollback();
                        throw;

                    }
                }
            }
        }
    }
}
