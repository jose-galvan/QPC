
using QPC.Core.Models;

namespace QPC.Core.DTOs
{
    public class InstructionDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }

        public int QualityControlId { get; set; }

        public InstructionStatus? Status { get; set; }
    }
}
