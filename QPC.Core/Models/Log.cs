
using System;

namespace QPC.Core.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public User UserCreated { get; set; }
        public DateTime DateCreated { get; set; }    
    }
}
