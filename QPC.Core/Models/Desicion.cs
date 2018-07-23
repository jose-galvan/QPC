using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPC.Core.Models
{
    public class Desicion:QPCBaseModel
    {
        public Desicion()
        {

        }
        public Desicion(User user) : base(user)
        {
        }

        public string Name { get; set; }
    }
}
