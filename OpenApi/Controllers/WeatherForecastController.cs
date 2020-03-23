using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Controllers
{
    /// <summary>
    /// Returns data related to weather forecasts.
    /// </summary>
    [ApiController]
    [Authorize("ApiPolicy")]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/>.
        /// </summary>
        /// <param name="logger">An instance of an <see cref="ILogger"/>.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns five weather forecasts.
        /// </summary>
        /// <returns>A list of five <see cref="WeatherForecast"/> objects.</returns>
        [HttpGet, MapToApiVersion("1")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Call the new v2 version but still retain old behaviour
            return GetPages(0, 5).Entries;
        }

        /// <summary>
        /// Gets a user-defined page of weather forecasts.
        /// </summary>
        /// <param name="skip">The amount of forecasts to skip.</param>
        /// <param name="take">The amount of forecasts to take.</param>
        /// <returns>An amount of <paramref name="take"/> weather forecasts starting after <paramref name="skip"/> days.</returns>
        [HttpGet, MapToApiVersion("2")]
        [SwaggerOperation(Tags = new[] { "Weather", "Changed" })]
        public PagedResult<WeatherForecast> GetPages(
            [FromQuery, SwaggerParameter("How many entries to skip", Required = true)]
            int skip = 0,
            [FromQuery, SwaggerParameter("How many entries to take", Required = true)]
            int take = 5)
        {
            var rng = new Random();
            return new PagedResult<WeatherForecast>()
            {
                Entries = Enumerable.Range(1, take).Select(index => new WeatherForecast()
                {
                    Date = DateTime.Now.AddDays(index + skip),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToArray(),
                StartIndex = skip,
                TotalAmount = rng.Next(skip + take, skip + take + 100),
            };
        }
    }
}
