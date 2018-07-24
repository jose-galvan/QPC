using System;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Defect: QPCBaseModel
    {
        public Defect()
        {

        }

        public Defect(User user) : base(user)
        {
        }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public void Update(User user, string name, string description)
        {
            SetTraceabilityValues(user);
            Name = name;
            Description = description;
        }
    }
}
