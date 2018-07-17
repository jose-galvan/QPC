using QPC.Core.Models;
using System.Collections.Generic;

namespace QPC.Core.ViewModels
{
    public class QualityControlsViewModel
    {
        public string Header { get; set; }
        public IEnumerable<QualityControl> Controls { get; set; }

        public string SearchCriteria { get; set; }

    }
}

