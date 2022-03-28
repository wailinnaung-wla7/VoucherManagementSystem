using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VoucherManagementSystem.DBContext
{
    public partial class Evoucher
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethodId { get; set; }
        public int DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public string BuyType { get; set; }
        public int MaximumBuyLimit { get; set; }
        public int? GiftPerUserLimit { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
