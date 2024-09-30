using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Clock;
using Services.DTO;
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
        public IActionResult StartStopClock(ClockStartStopDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst("user")?.Value;

            if (userName == null)
                return Unauthorized();

            clockService.StartStopClock(userName, dto);
            return Ok();
        }

        [HttpGet("Status")]
        public IActionResult GetClockStatus()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst("user")?.Value;
            if (userName == null)
                return Unauthorized();
            return Ok(clockService.GetClockStatus(userName));
        }
    }
}
