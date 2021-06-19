using AutoMapper;
using HotelListing.DTOs.Hotels;
using HotelListing.Entities;
using HotelListing.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly ILogger<HotelsController> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWorkRepository repository;
        public HotelsController(ILogger<HotelsController> logger, IMapper mapper, IUnitOfWorkRepository repository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet()]
        [ResponseCache(CacheProfileName = "CACHE_DURATION")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelsAsync()
        {
            try
            {
                var hotels = await repository.Hotels.GetAllAsync();
                var result = mapper.Map<List<HotelReadAll>>(hotels);

                return Ok(result);
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{id:int}", Name = "GetHotelAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelAsync(int id)
        {
            var hotel = await repository.Hotels.GetOneAsync(
                query => query.Id == id,
                new List<string> { "Country" });
            var result = mapper.Map<HotelRead>(hotel);

            return Ok(result);
        }


        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotelAsync([FromBody] HotelCreate hotelCreate)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid HttpPost in {this.GetType().FullName}");
                return BadRequest(ModelState);
            }

            var hotel = mapper.Map<Hotel>(hotelCreate);

            await repository.Hotels.InsertAsync(hotel);
            await repository.SaveAsync();

            return CreatedAtRoute("GetHotelAsync", new { id = hotel.Id }, mapper.Map<HotelRead>(hotel));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelUpdate hotelUpdate)
        {
            if (!ModelState.IsValid || id < 1)
            {
                logger.LogError($"Invalid HttpPut in {this.GetType().FullName}");
                return BadRequest(ModelState);
            }

            var hotel = await repository.Hotels.GetOneAsync(query => query.Id == id);
            if (null == hotel)
            {
                logger.LogError($"Invalid update in {this.GetType().FullName}");
                return BadRequest("Invalid update.");
            }

            // This overload will map source to destination. Verbatim.
            mapper.Map(hotelUpdate, hotel);

            repository.Hotels.Update(hotel);
            await repository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotelAsync(int id)
        {
            if (id < 1)
            {
                logger.LogError($"Invalid HttpDelete in {this.GetType().FullName}");
                return BadRequest();
            }

            var hotel = await repository.Hotels.GetOneAsync(query => query.Id == id);
            if (null == hotel)
            {
                logger.LogError($"Invalid delete in {this.GetType().FullName}");
                return BadRequest("Invalid delete.");
            }
            await repository.Hotels.DeleteAsync(hotel.Id);
            await repository.SaveAsync();

            return NoContent();
        }
    }
}
