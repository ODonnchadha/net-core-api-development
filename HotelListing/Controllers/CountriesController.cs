using AutoMapper;
using HotelListing.Interfaces.Repositories;
using HotelListing.DTOs.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWorkRepository repository;
        public CountriesController(ILogger<CountriesController> logger, IMapper mapper, IUnitOfWorkRepository repository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountriesAsync()
        {
            try
            {
                var countries = await repository.Countries.GetAllAsync();
                var result = mapper.Map<List<CountryReadAll>>(countries);

                return Ok(result);
            }
            catch(Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountryAsync(int id)
        {
            try
            {
                var country = await repository.Countries.GetOneAsync(
                    query => query.Id == id,
                    new List<string> { "Hotels" });
                var result = mapper.Map<CountryRead>(country);

                return Ok(result);
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
