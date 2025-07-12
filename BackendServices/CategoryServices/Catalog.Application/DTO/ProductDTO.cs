using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name must not empty")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Product name must be alphanumeric")]
        [Display(Name = "Product Name")]
        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "Product name must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Product name must be less than 50 characters long")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 50 characters long")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Product description must not empty")]
        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(10, ErrorMessage = "Product description must be at least 10 characters long")]
        [MaxLength(500, ErrorMessage = "Product description must be less than 500 characters long")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Product description must be between 10 and 500 characters long")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Product unit price must not empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product unit price must be greater than zero")]
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Unit price must be a valid decimal number with up to two decimal places")]
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        [Display(Name ="Create Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }
    }
}
