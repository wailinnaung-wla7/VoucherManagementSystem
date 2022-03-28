using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PromoCodesManagement.PromoContext
{
    public partial class EvoucherPurchase
    {
        public long Id { get; set; }
        public long Userid { get; set; }
        public string PurchasePhone { get; set; }
        public int EvoucherId { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public long? PaymentId { get; set; }
        public bool? IsGenerated { get; set; }
        public string Status { get; set; }
    }
}
