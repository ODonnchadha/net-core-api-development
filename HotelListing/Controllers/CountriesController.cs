using AutoMapper;
using HotelListing.DTOs.Countries;
using HotelListing.Entities;
using HotelListing.Interfaces.Repositories;
using HotelListing.Models;
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

        [HttpGet("{id:int}", Name = "GetCountryAsync")]
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

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountriesAsync([FromQuery] RequestParams requestParams)
        {
            try
            {
                var countries = await repository.Countries.GetAllAsync(requestParams);
                var result = mapper.Map<List<CountryReadAll>>(countries);

                return Ok(result);
            }
            catch(Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountryAsync([FromBody] CountryCreate countryCreate)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid HttpPost in {this.GetType().FullName}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = mapper.Map<Country>(countryCreate);

                await repository.Countries.InsertAsync(country);
                await repository.SaveAsync();

                return CreatedAtRoute("GetCountryAsync", new { id = country.Id }, mapper.Map<CountryRead>(country));
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountryAsync(int id, [FromBody] CountryUpdate countryUpdate)
        {
            if (!ModelState.IsValid || id < 1)
            {
                logger.LogError($"Invalid HttpPut in {this.GetType().FullName}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await repository.Countries.GetOneAsync(query => query.Id == id);
                if (null == country)
                {
                    logger.LogError($"Invalid update in {this.GetType().FullName}");
                    return BadRequest("Invalid update.");
                }

                // This overload will map source to destination. Verbatim.
                mapper.Map(countryUpdate, country);

                repository.Countries.Update(country);
                await repository.SaveAsync();

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountryAsync(int id)
        {
            if (id < 1)
            {
                logger.LogError($"Invalid HttpDelete in {this.GetType().FullName}");
                return BadRequest();
            }

            try
            {
                var country = await repository.Countries.GetOneAsync(query => query.Id == id);
                if (null == country)
                {
                    logger.LogError($"Invalid delete in {this.GetType().FullName}");
                    return BadRequest("Invalid delete.");
                }
                await repository.Countries.DeleteAsync(country.Id);
                await repository.SaveAsync();

                return NoContent();
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
