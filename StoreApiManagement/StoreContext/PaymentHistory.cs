using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StoreApiManagement.StoreContext
{
    public partial class PaymentHistory
    {
        public long Id { get; set; }
        public int PaymentMethodId { get; set; }
        public long EvoucherpurchaseId { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public int? Cvv { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Status { get; set; }
    }
}
