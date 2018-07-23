using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Product: QPCBaseModel
    {
        public Product()
        {

        }
        public Product(User user): base(user)
        {

        }
        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual ICollection<Defect> Defects { get; set; }

        public void Update(User user, string name, string description)
        {
            Name = name;
            Description = description;
            SetTraceabilityValues(user);
        }
    }
}
