using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Product: QPCBaseClass
    {

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual ICollection<Defect> Products { get; set; }
    }
}
