using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class InstructionViewModel: BaseModel
    {
        [Required, StringLength(250)]
        public string Comments { get; set; }
        public int QualityControlId { get; set; }

        public IEnumerable<Instruction> Instructions { get; set; }

        public bool CanSave { get; set; }
    }
}
