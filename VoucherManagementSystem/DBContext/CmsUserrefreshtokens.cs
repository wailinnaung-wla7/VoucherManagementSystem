using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VoucherManagementSystem.DBContext
{
    public partial class CmsUserrefreshtokens
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Token { get; set; }
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= Expires;
        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;

        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string Createdbyip { get; set; }
        public DateTime? Revoked { get; set; }
        public string Revokedbyip { get; set; }
        public string Replacedbytoken { get; set; }
    }
}
