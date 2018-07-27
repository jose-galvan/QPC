namespace QPC.Core.ViewModels
{
    public class QualityControlDetailViewModel: BaseModel
    {
        
        public string Serial { get; set; }

        public string Status { get; set; }
        
        public string Product { get; set; }
        public string ProductDescription { get; set; }

        public string Defect { get; set; }
        public string DefectDescription { get; set; }
        
        public string CreateDate { get; set; }
        public string UserCreated { get; set; }

        public string LastModificationUser { get; set; }
        public string LastModificationDate { get; set; }

        public bool CanSave { get; set; }

    }
}
