using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
