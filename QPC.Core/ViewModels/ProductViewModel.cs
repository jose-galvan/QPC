using QPC.Core.Models;
using System.Collections.Generic;

namespace QPC.Core.ViewModels
{
    public class ProductViewModel: BaseModel
    {
        public string SearchCriteria { get; set; }
        public List<Product> Products { get; set; }
        public List<Defect> Defects { get; set; }
    }
}
