using System;

namespace OpenApi
{
    /// <summary>
    /// Represents a weather forecast.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The date this forecast applies to.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The forecast temperature in degrees Celsius.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// The forecast temperature in degrees Fahrenheit.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// A brief textual description of the forecast.
        /// </summary>
        public string Summary { get; set; }
    }
}
