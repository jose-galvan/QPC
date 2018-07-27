using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class DefectViewModel : BaseModel
    {

        [Required]
        public int Product { get; set; }
        
        public string SearchCriteria { get; set; }
        public List<DefectItemViewModel> Defects { get; set; }
        public List<Product> Products { get; set; }
    }
}
