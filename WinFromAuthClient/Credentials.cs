using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFromAuthClient
{
    public class Credentials
    {
        public string token { get; set; }

        public class info
        {
            public int Id { get; set; }
            public string Username { get; set; }

            public string FullName { get; set; }

            public int SupplierId { get; set; }

            public string SupplierName { get; set; }
        }
    }
}
