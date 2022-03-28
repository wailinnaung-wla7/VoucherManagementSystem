using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StoreApiManagement.Helpers;
using StoreApiManagement.StoreContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApiManagement.Services
{
    public interface IeVoucherPurchaseService
    {
        Task<List<Paymentmethods>> GetPaymentmethods();
        Task<EvoucherPurchase> SaveAsync(EvoucherPurchase evoucher);
        Task<PaymentHistory> SavePaymentAsync(PaymentHistory payment);
    }

    public class eVoucherPurchaseService : IeVoucherPurchaseService
    {
        private storedbContext _context;
        private readonly AppSettings _appSettings;

        public eVoucherPurchaseService(
            storedbContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<EvoucherPurchase> SaveAsync(EvoucherPurchase evoucher)
        {
            try
            {
                var voucher = await _context.Evoucher.Where(a => a.Id == evoucher.EvoucherId).FirstOrDefaultAsync();
                voucher.Quantity = voucher.Quantity - evoucher.PurchaseQuantity;
                _context.Evoucher.Update(voucher);
                await _context.EvoucherPurchase.AddAsync(evoucher);
                await _context.SaveChangesAsync();

                return evoucher;
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return null;
            }
        }

        public async Task<List<Paymentmethods>> GetPaymentmethods()
        {
            try
            {
                List<Paymentmethods> methods = new List<Paymentmethods>();
                methods = await _context.Paymentmethods.ToListAsync();
                return methods;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<PaymentHistory> SavePaymentAsync(PaymentHistory payment)
        {
            try
            {
                var purchasehistory =await  _context.EvoucherPurchase.Where(a => a.Id == payment.EvoucherpurchaseId).FirstOrDefaultAsync();
                if (payment.Status =="Success")
                {
                    purchasehistory.Status = payment.Status;
                }
                else
                {
                    purchasehistory.Status = "Payment Failed";
                    var voucher = await _context.Evoucher.Where(a => a.Id == purchasehistory.EvoucherId).FirstOrDefaultAsync();
                    voucher.Quantity += purchasehistory.PurchaseQuantity;
                    _context.Evoucher.Update(voucher);
                }
                _context.EvoucherPurchase.Update(purchasehistory);
                await _context.PaymentHistory.AddAsync(payment);
                await _context.SaveChangesAsync();

                return payment;
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return null;
            }
        }


    }
}
