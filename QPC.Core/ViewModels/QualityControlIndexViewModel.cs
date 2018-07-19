using QPC.Core.Models;
using System.Collections.Generic;

namespace QPC.Core.ViewModels
{
    public class QualityControlIndexViewModel
    {
        public string Header { get; set; }
        public IEnumerable<ListItemViewModel> Controls { get; set; }

        public string SearchCriteria { get; set; }

    }
}

