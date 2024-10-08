﻿using BCrypt.Net;
using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly TCDbContext _context;
        private readonly IConfiguration configuration;
        public IdentityService(TCDbContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        private string HashPassword(string plaintextPw)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(plaintextPw, 15);
        }

        private bool VerifyPassword(string plaintextPw, string pwHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(plaintextPw, pwHash);
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.DisplayName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user", user.Username)
            };

            DateTime expiration = DateTime.Now.AddMinutes(120);
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims,
                expires: expiration,
                signingCredentials: credentials);

            AuthResponseDto dto = new AuthResponseDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expiration
            };

            return dto;
        }

        public AuthResponseDto AuthorizeUser(string username, string password)
        {
            var userEntity = _context.Users.Where(x=>x.Username ==  username).FirstOrDefault();
            if(userEntity != null)
            {
                if(VerifyPassword(password, userEntity.PasswordHash))
                {
                    return GenerateToken(userEntity);
                }
            }
            return new AuthResponseDto() { Token = "403", Expires = DateTime.Now};
        }

        public async Task<int> CreateUser(RegistrationDto registrationDto)
        {
            if(!_context.Users.Any(x=>x.Username.ToLower() == registrationDto.Username.ToLower() || x.Email.ToLower() == registrationDto.Email.ToLower()))
            {
                User user = new User()
                {
                    Email = registrationDto.Email.ToLower(),
                    DisplayName = registrationDto.DisplayName,
                    Username = registrationDto.Username.ToLower(),
                    PasswordHash = HashPassword(registrationDto.Password),
                    Deleted = false,
                    ClockEntries = new List<ClockEntry>()
                };
                await _context.Users.AddAsync(user);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<User> GetUser(int userId)
        {
            var result = await _context.Users.Where(x => x.Id == userId).Include(x => x.ClockEntries).FirstOrDefaultAsync();
            if(result != null)
                return result;
            return new User();
        }

        public async Task<User> GetUser(string username)
        {
            var result = await _context.Users.Where(x => x.Username.ToLower() == username).Include(x => x.ClockEntries).FirstOrDefaultAsync();
            if (result != null)
                return result;
            return new User();
        }
    }
}
