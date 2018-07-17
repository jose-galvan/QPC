
using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class QualityControlViewModel
    {

        [Required]
        public string Serial { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Product { get; set; }
        [Required]
        public int Defect { get; set; }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Defect> Defects { get; set; }
    }
}
