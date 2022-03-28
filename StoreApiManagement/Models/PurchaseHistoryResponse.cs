using StoreApiManagement.StoreContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApiManagement.Models
{
    public class PurchaseHistoryResponse
    {
        public List<Promocodes> UnusedPromoCodes { get; set; }
        public List<Promocodes> UsedPromoCodes { get; set; }
    }
}
