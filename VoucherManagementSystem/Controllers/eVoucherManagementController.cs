using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherManagementSystem.DBContext;
using VoucherManagementSystem.Models;
using VoucherManagementSystem.Services;

namespace VoucherManagementSystem.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class eVoucherManagementController : ControllerBase
    {
        private IUserService _userService;
        private IeVoucherService _eVoucherService;

        public eVoucherManagementController(IUserService userService,IeVoucherService eVoucherService)
        {
            _userService = userService;
            _eVoucherService = eVoucherService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        [HttpPost("AddEVoucher")]
        public async Task<IActionResult> AddEVoucher([FromBody] Evoucher resource)
        {

            var result = await _eVoucherService.SaveAsync(resource);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut("UpdateEvoucher")]
        public async Task<IActionResult> UpdateEvoucher([FromBody] Evoucher resource)
        {

            var result = await _eVoucherService.UpdateAsync(resource);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPost("SetEvoucherInActive")]
        public async Task<IActionResult> SetEvoucherInActive([FromBody]SetInactiveEvoucherRequest request)
        {

            var result = await _eVoucherService.SetInActiveAsync(request.ID);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        #region helpers
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        #endregion
    }
}
