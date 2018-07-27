using QPC.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core
{
    //this class contains all the commom members 
    // that are used in view models and dtos.
    public class BaseModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
