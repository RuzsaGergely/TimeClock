﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Identity;
using System.Threading.Tasks;

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
            var authToken = _identityService.AuthorizeUser(loginDto.Username.ToLower(), loginDto.Password);
            if (authToken.Token != "403")
                return Ok(authToken);
            return Forbid();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegistrationDto registrationDto)
        {
            var result = await _identityService.CreateUser(registrationDto);
            if (result > 0)
                return BadRequest();
            return Ok();
        }
    }
}
