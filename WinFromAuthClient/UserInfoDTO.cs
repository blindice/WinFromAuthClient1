using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinFromAuthClient
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string FullName { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
    }
}
