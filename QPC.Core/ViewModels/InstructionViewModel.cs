using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class InstructionViewModel
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(250)]
        public string Description { get; set; }

        [Required, StringLength(250)]
        public string Comments { get; set; }
        public int QualityControlId { get; set; }

        public IEnumerable<Instruction> Instructions { get; set; }
    }
}
