using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PromoCodesManagement.PromoContext
{
    public partial class Promocodes
    {
        public long Id { get; set; }
        public int? EvoucherId { get; set; }
        public string PhoneNumber { get; set; }
        public string PromoCode { get; set; }
        public string Qrcode { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Status { get; set; }
    }
}
