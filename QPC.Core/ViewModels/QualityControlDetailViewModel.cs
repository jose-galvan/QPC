using QPC.Core.DTOs;
using QPC.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.ViewModels
{
    public class QualityControlDetailViewModel
    {
        public QualityControlDetailViewModel()
        {
            Desicions = new List<Desicion>
            {
                Desicion.Acepted, Desicion.Rejected, Desicion.Rework
            };

        }
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(250)]
        public string Description { get; set; }


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

        public List<Instruction> Instructions { get; set; }
        public string FinalDesicion { get; set; }
        
        
        public IEnumerable<Desicion> Desicions { get; set; }
    }
}
