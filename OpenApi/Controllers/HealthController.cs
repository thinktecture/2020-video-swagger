using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenApi.Controllers
{
    /// <summary>
    /// Provides health information about this system.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Should return Ok when called.
        /// </summary>
        /// <returns>An OK result.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Ping() => Ok();

        /// <summary>
        /// Somehow kills the process.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(200)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Kill()
        {
            RaiseEvent();
            return Ok();
        }

        private void RaiseEvent() => RaiseEventAsync();

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      private async void RaiseEventAsync() => throw new Exception("Boom");
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
