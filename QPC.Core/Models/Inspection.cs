
namespace QPC.Core.Models
{
    public class Inspection: QPCBaseModel
    {
        public Inspection()
        {

        }
        public Inspection(User user ): base(user)
        {
        }
        public Desicion Desicion { get; set; }
        public string Comments { get; set; }
        
    }

}
