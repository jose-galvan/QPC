using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QPC.Web.Models
{
    public class Progress
    {
        public int ControlId { get; set; }
        public int CurrentStage { get; set; }
        public int HighestStage { get; set; }
    }
}