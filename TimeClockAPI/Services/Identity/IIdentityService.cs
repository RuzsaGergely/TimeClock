using Domain;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Identity
{
    public interface IIdentityService
    {
        public string AuthorizeUser(string username, string password);
        public Task<int> CreateUser(RegistrationDto registrationDto);
        public Task<User> GetUser(int userId);
        public Task<User> GetUser(string username);
    }
}
