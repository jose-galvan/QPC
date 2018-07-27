using System.ComponentModel.DataAnnotations;

namespace QPC.Core.DTOs
{
    public class QualityControlDto : BaseModel
    {
        public string Serial { get; set; }
        public int Product { get; set; }
        public int Defect { get; set; }
    }
}
