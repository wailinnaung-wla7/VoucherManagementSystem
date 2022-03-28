using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApiManagement.Models;
using StoreApiManagement.Services;
using StoreApiManagement.StoreContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApiManagement.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class StoreApiManagementController : ControllerBase
    {
        private IUserService _userService;
        private IeVoucherService _eVoucherService;
        private IeVoucherPurchaseService _eVoucherPurchaseService;
        private IPromoCodeService _PromoCodeService;

        public StoreApiManagementController(IUserService userService , IeVoucherService eVoucherService, IeVoucherPurchaseService eVoucherPurchaseService, IPromoCodeService PromoCodeService)
        {
            _userService = userService;
            _eVoucherService = eVoucherService;
            _eVoucherPurchaseService = eVoucherPurchaseService;
            _PromoCodeService = PromoCodeService;
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

        [HttpGet("GetEVouchers")]
        public async Task<List<Evoucher>> ListAsync()
        {          
            var queryResult = await _eVoucherService.ListAsync();

            return queryResult;
        }
        [HttpGet("GetPaymentMethods")]
        public async Task<List<Paymentmethods>> GetPaymentMethods()
        {
            var queryResult = await _eVoucherPurchaseService.GetPaymentmethods();

            return queryResult;
        }
        [HttpGet("GetEVoucherDetail/{id}")]
        public async Task<Evoucher> EVoucherDetail(int id)
        {
            var detail = await _eVoucherService.GetDetail(id);
            return detail;
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut([FromBody] EvoucherPurchase resource)
        {
            resource.Status = "Pending";
            resource.IsGenerated = false;
            var result = await _eVoucherPurchaseService.SaveAsync(resource);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("MakePayment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentHistory resource)
        {
            resource.Status = "Success";

            if (resource.PaymentMethodId == 1 || resource.PaymentMethodId == 2)
            {
                if (resource.AccountNumber.Count() < 16 || resource.AccountNumber.Count() > 16 || resource.Cvv == 0 ||resource.ExpireDate <= DateTime.Now )
                {
                    resource.Status = "Fail";
                }

            }
            // check credit/debit card validation stuff and get response 

            var result = await _eVoucherPurchaseService.SavePaymentAsync(resource);

            if (result == null || resource.Status == "Fail")
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("VerifyPromoCode/{code}")]
        public async Task<PromoCodeStatusResponse> VerifyPromoCode(string code)
        {
            var detail = await _PromoCodeService.GetStatus(code);
            return detail;
        }
        [HttpPost("PurchaseHistory")]
        public async Task<PurchaseHistoryResponse> ListAsync(PurchaseHistoryRequest request)
        {
            var queryResult = await _PromoCodeService.ListAsync(request.id);

            return queryResult;
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
