using AutoMapper;
using HotelListing.Interfaces.Repositories;
using HotelListing.DTOs.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelAsync(int id)
        {
            try
            {
                var hotel = await repository.Hotels.GetOneAsync(
                    query => query.Id == id,
                    new List<string> { "Country" });
                var result = mapper.Map<HotelRead>(hotel);

                return Ok(result);
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }


        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] HotelCreate hotelCreate)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid HttpPost in {this.GetType().FullName}");
                return BadRequest(ModelState);
            }

            try
            {
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
