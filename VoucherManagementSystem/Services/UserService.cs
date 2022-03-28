using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VoucherManagementSystem.DBContext;
using VoucherManagementSystem.Helpers;
using VoucherManagementSystem.Models;

namespace VoucherManagementSystem.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
    }

    public class UserService : IUserService
    {
        private storedbContext _context;
        private readonly AppSettings _appSettings;

        public UserService(
            storedbContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            CmsUserrefreshtokens refreshtoken = new CmsUserrefreshtokens();
            var user = await _context.CmsUser.FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            refreshtoken = generateRefreshToken(ipAddress);
            refreshtoken.Userid = user.Userid;

            _context.Add(refreshtoken);
            await _context.SaveChangesAsync();

            return new AuthenticateResponse(user, jwtToken, refreshtoken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            
                var lstRefreshToken = await _context.CmsUserrefreshtokens.Where(u => u.Token == token).SingleOrDefaultAsync();
                var user = await _context.CmsUser.SingleOrDefaultAsync(u => u.Userid == lstRefreshToken.Userid);

                if (lstRefreshToken == null) return null;

                if (!lstRefreshToken.IsActive) return null;

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress);
                newRefreshToken.Userid = user.Userid;
                lstRefreshToken.Revoked = DateTime.UtcNow;
                lstRefreshToken.Revokedbyip = ipAddress;
                lstRefreshToken.Replacedbytoken = newRefreshToken.Token;

                _context.Add(newRefreshToken);
                _context.Update(lstRefreshToken);
                await _context.SaveChangesAsync();

                // generate new jwt
                var jwtToken = generateJwtToken(user);
                return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
            
      
        }
        // helper methods

        private string generateJwtToken(CmsUser user)
        {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Userid.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

        }

        private CmsUserrefreshtokens generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new CmsUserrefreshtokens
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    Createdbyip = ipAddress
                };
            }
        }
    }
}
