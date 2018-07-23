using QPC.Core.Models;
using System.Collections.Generic;

namespace QPC.Core.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string SearchCriteria { get; set; }
        public List<Product> Products { get; set; }
        public List<Defect> Defects { get; set; }

    }
}
