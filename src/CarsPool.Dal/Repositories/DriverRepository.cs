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
    public class DriverRepository : IDriverRepository
    {
        private readonly CarsPoolDbContext _dbContext;
        private readonly ILogger<DriverRepository> _logger;

        public DriverRepository(CarsPoolDbContext dbContext, ILogger<DriverRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Driver> Get(int id)
        {
            _logger.LogDebug("Searching driver by Id: {id}", id);

            using (_dbContext)
            {
                var result = await _dbContext.Drivers.FindAsync(id);

                _logger.LogInformation("Searching driver by Id: {id} completed. Result: {@result}", id, result);

                return result;
            }
        }

        public async Task<IEnumerable<Driver>> GetAll()
        {
            _logger.LogDebug("Retrieving all drivers");

            using (_dbContext)
            {
                var drivers = _dbContext.Drivers.ToList();

                _logger.LogDebug("Retrieving all drivers result: {@drivers}", drivers);

                return drivers;
            }
        }

        public async Task<Driver> Create(Driver driver)
        {
            using (_dbContext)
            {
                _logger.LogDebug("Creating driver: {@driver}", driver);

                using(var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if(driver.CarId != null)
                        {
                            var car = await _dbContext.Cars.FindAsync(driver.CarId);
                            if (car == null)
                            {
                                // Checking if car exists.
                                throw new CarsPoolDalException($"Car with id {driver.CarId} doesn't exists")
                                {
                                    ErrorCode = DalErrorCodes.EntityNotExists
                                };
                            }

                            // Checking if car already belongs to other driver
                            var carDriver = _dbContext.CarDriver.SingleOrDefault(carDriver => carDriver.CarId == driver.CarId && carDriver.DriverId != driver.CarId);
                            if (carDriver != null)
                            {
                                throw new CarsPoolDalException($"Car with id {driver.CarId} already belongs to other driver")
                                {
                                    ErrorCode = DalErrorCodes.CarBelongsToOtherDriver
                                };
                            }
                        }

                        // adding driver
                        var result = await _dbContext.Drivers.AddAsync(driver);
                        await _dbContext.SaveChangesAsync();

                        if(driver.CarId  != null)
                        {
                            // adding link between driver and car
                            var newCarDriver = new CarDriver()
                            {
                                CarId = driver.CarId.Value,
                                DriverId = driver.Id
                            };

                            await _dbContext.CarDriver.AddAsync(newCarDriver);
                            await _dbContext.SaveChangesAsync();
                        }

                        _logger.LogDebug("Creating driver: {@driver} result: {@result}", driver, result);

                        transaction.Commit();
                        return result.Entity;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occured during saving driver {@driver} in db", driver);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task Delete(int id)
        {
            using (_dbContext)
            {
                _logger.LogDebug("Deleting driver by id: {id}", id);

                // We need to perform both select and delete operations in one transaction in order to avoid sittuation when driver is removed by ohter request between select and delete
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var driver = await _dbContext.Drivers.FindAsync(id);

                        if(driver == null)
                        {
                            // We have nothing to delete
                            throw new CarsPoolDalException($"Driver with id {id} doesn't exist.") { ErrorCode = DalErrorCodes.EntityNotExists };
                        }

                        var carDriver = _dbContext.CarDriver.FirstOrDefault(carDriver => carDriver.DriverId == id);

                        if (carDriver != null)
                        {
                            // If any car was assigned to the driver, we remove this car from this driver
                            _dbContext.CarDriver.Remove(carDriver);
                        }

                        _dbContext.Drivers.Remove(driver);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        _logger.LogDebug("Deleting driver by id: {id} succeeded", id);
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

        public async Task Update(Driver driver)
        {
            _logger.LogDebug("Updating driver with id: {id} by {@driver}", driver.Id, driver);

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try 
                    {
                        var driverToUpdate = await _dbContext.Drivers.FindAsync(driver.Id);

                        if (driverToUpdate == null)
                        {
                            // We have nothing to update
                            throw new CarsPoolDalException($"Driver with id {driver.Id} doesn't exist.") { ErrorCode = DalErrorCodes.EntityNotExists };
                        }

                        int? oldCarId = driverToUpdate.CarId;
                        int? newCarId = driver.CarId;

                        driverToUpdate.CarId = driver.CarId;
                        driverToUpdate.FirstName = driver.FirstName;
                        driverToUpdate.SecondName = driver.SecondName;

                        if(oldCarId != newCarId && oldCarId != null)
                        {
                            // Check if car belongs to other driver
                            var otherCarDriver = _dbContext.CarDriver.FirstOrDefault(carDriver => carDriver.DriverId != driver.Id && carDriver.CarId == driver.CarId);
                            if (otherCarDriver != null)
                            {
                                throw new CarsPoolDalException($"Car with id {driver.CarId} already belongs to other driver")
                                {
                                    ErrorCode = DalErrorCodes.CarBelongsToOtherDriver
                                };
                            }
                        }

                                            // Removing old link
                    var oldCarDriver = _dbContext.CarDriver.FirstOrDefault(carDriver => carDriver.DriverId == driver.Id && carDriver.CarId == oldCarId);

                    if (oldCarDriver != null)
                    {
                        _dbContext.CarDriver.Remove(oldCarDriver);
                    }



                    await _dbContext.SaveChangesAsync();

                    // If new car assigned to driver, add new link.
                    if (newCarId != null)
                    {
                        var newCarDriver = new CarDriver() { CarId = driverToUpdate.CarId.Value, DriverId = driverToUpdate.Id };
                        await _dbContext.CarDriver.AddAsync(newCarDriver);
                    }

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    _logger.LogDebug("Updating driver with id: {id} by {@driver} succeeded.", driver.Id, driver);

                }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred during updating driver with id: {id} by {@driver}", driver.Id, driver);
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }
    }
}
