using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Defect: QPCBaseClass
    {
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
