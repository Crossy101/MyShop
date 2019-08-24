using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyShop.Core.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The name does not reach the required length or is too long!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        public string Image { get; set; }
    }
}
