using CarsPool.Api.Abstractions;
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
    public class CarsController : ControllerBase
    {
        private readonly ICarService _service;

        public CarsController(ICarService service)
        {
            _service = service;
        }

        // GET: api/<CarsController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CarResponseModel>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _service.GetAll();
            return Ok(cars);
        }

        // GET api/<CarsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CarResponseModel), 200)]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _service.GetById(id);

            if(car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        // POST api/<CarsController>
        [HttpPost]
        [ProducesResponseType(typeof(CarResponseModel), 201)]
        public async Task<IActionResult> Post([FromBody] CarRequestModel car)
        {
            var newCar = await _service.Create(car);
            return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, newCar);
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CarRequestModel car)
        {
            await _service.Update(id, car);
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
           await _service.Delete(id);
        }
    }
}
