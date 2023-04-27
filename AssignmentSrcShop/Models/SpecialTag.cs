using System.ComponentModel.DataAnnotations;

namespace AssignmentSrcShop.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string SpecialTagName { get; set; }
    }
}
