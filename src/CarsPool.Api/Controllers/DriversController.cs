using CarsPool.Api.Abstractions;
using CarsPool.Api.Filters;
using CarsPool.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarsPool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _service;

        public DriversController(IDriverService service)
        {
            _service = service;
        }

        // GET: api/<DriversController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DriverResponseModel>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var drivers = await _service.GetAll();
            return Ok(drivers);
        }

        // GET api/<DriversController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DriverResponseModel), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var driver = await _service.GetById(id);

            if(driver == null) 
            { 
                return NotFound(); 
            }

            return Ok(driver);
        }

        // POST api/<DriversController>
        [HttpPost]
        [ProducesResponseType(typeof(DriverResponseModel), 201)]
        public async Task<IActionResult> Post([FromBody] DriverRequestModel driver)
        {
            var newDriver = await _service.Create(driver);
            return CreatedAtAction(nameof(GetById), new { id = newDriver.Id }, newDriver);
        }

        // PUT api/<DriversController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] DriverRequestModel driver)
        {
            await _service.Update(id, driver);
        }

        // DELETE api/<DriversController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.Delete(id);
        }
    }
}
