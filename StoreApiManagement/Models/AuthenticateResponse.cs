using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StoreApiManagement.StoreContext;

namespace StoreApiManagement.Models
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(CusUser user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Name = user.FullName;
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
