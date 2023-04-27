using System.ComponentModel.DataAnnotations;

namespace AssignmentSrcShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }
    }
}
