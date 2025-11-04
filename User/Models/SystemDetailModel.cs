using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Models
{
    public class SystemDetailModel
    {
        public string ItemName { get; set; }
        public string Quantity { get; set; }    // keep string if bindings expect string; change to int if you prefer
        public string BrandModel { get; set; }
        public string SerialNumber { get; set; }
    }
}
