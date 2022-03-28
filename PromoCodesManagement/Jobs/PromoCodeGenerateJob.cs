using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCodesManagement.PromoContext;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodesManagement.Jobs
{
    [DisallowConcurrentExecution]
    public class PromoCodeGenerateJob : IJob
    {
        private readonly ILogger<PromoCodeGenerateJob> _logger;
        private storedbContext _context;
        public PromoCodeGenerateJob(ILogger<PromoCodeGenerateJob> logger, storedbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            List<Promocodes> codes = new List<Promocodes>();
            var purchaseList = await _context.EvoucherPurchase.Where(a => a.Status == "Success" && a.IsGenerated == false).ToListAsync();
            foreach(var purchase in purchaseList)
            {
                var evoucher = await _context.Evoucher.Where(a => a.Id == purchase.EvoucherId).FirstOrDefaultAsync();
                codes = GeneratePromoCodes(purchase.PurchaseQuantity, purchase, evoucher.ExpiryDate);
                await _context.Promocodes.AddRangeAsync(codes);
                purchase.IsGenerated = true;
                _context.Update(purchase);
            }
            await _context.SaveChangesAsync();
            if (purchaseList.Count > 0)
            _logger.LogInformation("PromoCodes Generated");
            else
                _logger.LogInformation("No Info To Be Generated");
            return;
        }
        private List<Promocodes> GeneratePromoCodes(int quantity,EvoucherPurchase voucher,DateTime expire)
        {
            List<Promocodes> promolist = new List<Promocodes>();
            var alphabetic = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numeric = "0123456789";
            var stringChars = new char[11];
            var random = new Random();
            for (int x=0; x < quantity; x++)
            {
                for (int i = 0; i < 5; i++)
                {
                    stringChars[i] = alphabetic[random.Next(alphabetic.Length)];
                }
                for (int i = 5; i < stringChars.Length; i++)
                {
                    stringChars[i] = numeric[random.Next(numeric.Length)];
                }
                int n = stringChars.Length;
                while (n > 1)
                {
                    int k = random.Next(n--);
                    char temp = stringChars[n];
                    stringChars[n] = stringChars[k];
                    stringChars[k] = temp;
                }
                string result = new string(stringChars);
                Promocodes promo = new Promocodes();
                promo.PhoneNumber = voucher.PurchasePhone;
                promo.EvoucherId = voucher.EvoucherId;
                promo.PromoCode = result;
                promo.Qrcode = result;
                promo.ExpireDate = expire;
                promo.Status = "Active";
                promolist.Add(promo);
            }
            return promolist;
        }
    }
}
