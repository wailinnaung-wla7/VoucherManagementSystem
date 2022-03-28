using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VoucherManagementSystem.DBContext;
using VoucherManagementSystem.Helpers;

namespace VoucherManagementSystem.Services
{

    public interface IeVoucherService
    {
        Task<Evoucher> SaveAsync(Evoucher evoucher);
        Task<Evoucher> UpdateAsync(Evoucher evoucher);
        Task<Evoucher> SetInActiveAsync(int id);
    }
    public class eVoucherService : IeVoucherService
    {
        private storedbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public eVoucherService(
            storedbContext context,
            IOptions<AppSettings> appSettings, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<Evoucher> SaveAsync(Evoucher evoucher)
        {

            string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Images");
            string fileName = Guid.NewGuid().ToString("N")+".jpg";
            //string imagePath = folderPath + fileName;
            string imagePath = Path.Combine(folderPath, fileName);

            string base64StringData = evoucher.Image;
            string cleandata = base64StringData.Replace("data:image/png;base64,", "");
            byte[] data = System.Convert.FromBase64String(cleandata);
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            try
            {
                evoucher.Image = imagePath;
                evoucher.CreatedDateTime = DateTime.Now;
                await _context.Evoucher.AddAsync(evoucher);
                await _context.SaveChangesAsync(); 

                return evoucher;
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return null;
            }
        }
        public async Task<Evoucher> UpdateAsync(Evoucher evoucher)
        {
            var existingVoucher = await _context.Evoucher.FirstOrDefaultAsync(a=> a.Id == evoucher.Id);

            if (existingVoucher == null)
                return null;


            existingVoucher.ExpiryDate = evoucher.ExpiryDate;
            existingVoucher.Quantity = evoucher.Quantity;
            existingVoucher.BuyType = evoucher.BuyType;
            existingVoucher.MaximumBuyLimit = evoucher.MaximumBuyLimit;

            try
            {
                _context.Update(existingVoucher);
                await _context.SaveChangesAsync();

                return existingVoucher;
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return null;
            }
        }
        public async Task<Evoucher> SetInActiveAsync(int id)
        {
            var existingVoucher = await _context.Evoucher.FirstOrDefaultAsync(a => a.Id == id);

            if (existingVoucher == null)
                return null;

            existingVoucher.IsActive = false;

            try
            {
                _context.Update(existingVoucher);
                await _context.SaveChangesAsync();

                return existingVoucher;
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return null;
            }
        }
    }
}
