using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Identity;

namespace TimeClockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private IIdentityService _identityService;
        public AuthenticationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("Auth")]
        public IActionResult AuthorizeUser(LoginDto loginDto)
        {
            var authToken = _identityService.AuthorizeUser(loginDto.Username, loginDto.Password);
            if(authToken != "")
                return Ok(authToken);
            return Forbid();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegistrationDto registrationDto)
        {
            var result = await _identityService.CreateUser(registrationDto);
            if(result > 0)
                return BadRequest();
            return Ok();
        }
    }
}
