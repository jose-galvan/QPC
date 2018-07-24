using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class DefectViewModel
    {
        public int? Id { get; set; }

        [Required][StringLength(50)]
        [Display(Name = "Control")]
        public string Name { get; set; }

        [Required]
        public int Product { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }


        public string SearchCriteria { get; set; }
        public List<DefectItemViewModel> Defects { get; set; }
        public List<Product> Products { get; set; }
    }
}
