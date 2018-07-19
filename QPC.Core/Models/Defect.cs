using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Defect: QPCBaseClass
    {

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
