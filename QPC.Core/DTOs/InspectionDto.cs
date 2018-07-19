using QPC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPC.Core.DTOs
{
    public  class InspectionDto
    {
        public int? Id { get; set; }
        public int Desicion { get; set; }
        public string Comments { get; set; }

        public int QualityControlId { get; set; }
    }
}
