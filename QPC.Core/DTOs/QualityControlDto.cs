using System.ComponentModel.DataAnnotations;

namespace QPC.Core.DTOs
{
    public class QualityControlDto
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Serial { get; set; }

        [Required]
        [Display(Name = "Control")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Product { get; set; }
        [Required]
        public int Defect { get; set; }
    }
}
