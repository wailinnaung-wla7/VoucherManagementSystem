using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StoreApiManagement.Helpers;
using StoreApiManagement.StoreContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApiManagement.Services
{
    public interface IeVoucherService
    {
        Task<List<Evoucher>> ListAsync();
        Task<Evoucher> GetDetail(int id);
    }
    public class eVoucherService : IeVoucherService
    {
        private storedbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _distributedCache;

        public eVoucherService(
            storedbContext context,
            IOptions<AppSettings> appSettings,IDistributedCache distributedCache)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _distributedCache = distributedCache;
        }

        public async Task<List<Evoucher>> ListAsync()
        {
            var cacheKey = "eVoucherList";
            string serializedCustomerList;
            List<Evoucher> EVoucherList = new List<Evoucher>();
            var redisCustomerList = await _distributedCache.GetAsync(cacheKey);
            if (redisCustomerList != null)
            {
                serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
                EVoucherList = JsonConvert.DeserializeObject<List<Evoucher>>(serializedCustomerList);
            }
            else
            {
                EVoucherList =await  _context.Evoucher.Where(a=> a.IsActive == true && a.ExpiryDate < DateTime.Now).ToListAsync();
                serializedCustomerList = JsonConvert.SerializeObject(EVoucherList);
                redisCustomerList = Encoding.UTF8.GetBytes(serializedCustomerList);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));
                await _distributedCache.SetAsync(cacheKey, redisCustomerList, options);
            }

            return EVoucherList;
        }
        public async Task<Evoucher> GetDetail(int id)
        {
            var existingVoucher = await _context.Evoucher.FirstOrDefaultAsync(a => a.Id == id);

            return existingVoucher;
        }
    }
}
