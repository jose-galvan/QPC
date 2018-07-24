using QPC.Core.Models;
using System.Collections.Generic;

namespace QPC.Core.ViewModels
{
    public class DefectViewModel
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        
        public int Product { get; set; }
        
        public string Description { get; set; }


        public string SearchCriteria { get; set; }
        public List<DefectItemViewModel> Defects { get; set; }
        public List<Product> Products { get; set; }
    }
}
