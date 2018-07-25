namespace QPC.Core.DTOs
{
    public class QualityControlDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Serial { get; set; }

        public int Product { get; set; }
        public int Defect { get; set; }
    }
}
