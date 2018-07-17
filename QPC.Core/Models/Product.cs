using System.Collections.Generic;

namespace QPC.Core.Models
{
    public class Product: QPCBaseClass
    {
        public virtual ICollection<Defect> Products { get; set; }
    }
}
