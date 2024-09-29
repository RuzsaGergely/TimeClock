using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Clock;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TimeClockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClockController : ControllerBase
    {
        private readonly IClockService clockService;
        public ClockController(IClockService clockService)
        {
            this.clockService = clockService;
        }

        [HttpPost("StartStop")]
        public IActionResult StartStopClock()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            // TODO: Fix it, not working.
            clockService.StartStopClock(identity.FindFirst("sub").Value);
            return Ok();
        }
    }
}
