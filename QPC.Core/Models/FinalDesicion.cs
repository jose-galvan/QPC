
namespace QPC.Core.Models
{
    public class FinalDesicion: QPCBaseClass
    {
        public Desicion Desicion { get; set; }
        public string Comments { get; set; }
        
    }

    public enum Desicion
    {
        Pendign = 0,
        Acepted =1,
        Rejected =2, 
        Rework = 3
    }
}
