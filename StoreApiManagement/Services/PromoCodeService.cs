using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QRCoder;
using StoreApiManagement.Helpers;
using StoreApiManagement.Models;
using StoreApiManagement.StoreContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApiManagement.Services
{
    public interface IPromoCodeService
    {
        Task<PromoCodeStatusResponse> GetStatus(string promocode);
        Task<PurchaseHistoryResponse> ListAsync(long id);
    }
    public class PromoCodeService : IPromoCodeService
    {
        private storedbContext _context;
        private readonly AppSettings _appSettings;

        public PromoCodeService(
            storedbContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public async Task<PromoCodeStatusResponse> GetStatus(string promocode)
        {
            PromoCodeStatusResponse response = new PromoCodeStatusResponse();
            var Promo = await _context.Promocodes.FirstOrDefaultAsync(a => a.PromoCode == promocode);
            response.Status = Promo.Status;
            if (Promo.ExpireDate > DateTime.Now)
                response.Status = "Expired";

            return response;
        }
        public async Task<PurchaseHistoryResponse> ListAsync(long userid)
        {
            PurchaseHistoryResponse response = new PurchaseHistoryResponse();
            var user = await _context.CusUser.Where(b => b.Id == userid).FirstOrDefaultAsync();

            response.UnusedPromoCodes = await _context.Promocodes.Where(a => a.PhoneNumber == user.PhoneNumber
            && a.Status == "Active" && a.ExpireDate > DateTime.Now ).ToListAsync();

            response.UsedPromoCodes = await _context.Promocodes.Where(a => a.PhoneNumber == user.PhoneNumber
            && a.Status == "Used").ToListAsync();

            var expirecodes = await _context.Promocodes.Where(a => a.PhoneNumber == user.PhoneNumber
            && a.Status == "Active" && a.ExpireDate < DateTime.Now).ToListAsync();

            expirecodes.Where(w => w.ExpireDate < DateTime.Now).ToList().ForEach(s => s.Status = "Expired");
            response.UsedPromoCodes.AddRange(expirecodes);


            //response.UsedPromoCodes.Where(w => w.ExpireDate < DateTime.Now).ToList().ForEach(s => s.Status = "Expired");

            // string test = "test";
            //QRCodeGenerator QrGenerator = new QRCodeGenerator();
            //QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(test, QRCodeGenerator.ECCLevel.Q);
            //QRCode QrCode = new QRCode(QrCodeInfo);
            //Bitmap QrBitmap = QrCode.GetGraphic(60);
            //byte[] BitmapArray = QrBitmap.BitmapToByteArray();
            //string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));

            //Create QR Codes in Consumer Side using promo Code Numbers
            return response;
        }

       

    }
}
