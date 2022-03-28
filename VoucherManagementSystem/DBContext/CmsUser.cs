using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VoucherManagementSystem.DBContext
{
    public partial class CmsUser
    {
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phonenum { get; set; }
        public string Address { get; set; }
    }
}
