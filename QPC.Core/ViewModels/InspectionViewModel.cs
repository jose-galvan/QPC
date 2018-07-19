using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class InspectionViewModel
    {
        public InspectionViewModel()
        {
        }

        public int QualityControlId { get; set; }

        [Required, Display(Description ="Desicion")]
        public int FinalDesicison { get; set; }
        [Required]
        public string Comments { get; set; }

        public List<Desicion> Desicions { get; set; }


        public bool CanInspect { get; set; }
    }
}
