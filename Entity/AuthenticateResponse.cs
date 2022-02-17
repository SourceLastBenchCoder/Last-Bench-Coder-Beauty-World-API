using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Entity
{
    public class AuthenticateResponse
    {
        public int UserUniqueId { get; set; }
        public string UserId { get; set; }
        public string EmailId { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}