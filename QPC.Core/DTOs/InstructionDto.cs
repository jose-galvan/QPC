
using QPC.Core.Models;

namespace QPC.Core.DTOs
{
    public class InstructionDto: BaseModel
    {
        public string Comments { get; set; }
        public int QualityControlId { get; set; }
        public InstructionStatus? Status { get; set; }
    }
}
