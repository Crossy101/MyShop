using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string basketID { get; set; }
        public string productID { get; set; }
        public int quantity { get; set; }
    }
}
