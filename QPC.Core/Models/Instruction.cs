
using QPC.Core.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace QPC.Core.Models
{
    public class Instruction: QPCBaseClass
    {
        public InstructionStatus Status { get; set; }
        public string Comments { get; set; }

        [Required]
        public int QualityControlId { get; set; }
        public QualityControl QualityControl { get; set; }


        public void Update(User user)
        {
            if (Status == InstructionStatus.Performed)
                throw new Exception("Current Instruction status does not allow modifications");
            
            if(QualityControl.Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to update instructions.");

            Status = InstructionStatus.Performed;
            SetTraceabilityValues(user);
            QualityControl.SetTraceabilityValues(user);
        }

        public void Cancel(User user)
        {
            if (Status == InstructionStatus.Performed)
                throw new Exception("Performed Instructions cannot be canceled");

            if (QualityControl.Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to update instructions.");

            Status = InstructionStatus.Canceled;

            SetTraceabilityValues(user);
            QualityControl.SetTraceabilityValues(user);
        }
    }

    public enum InstructionStatus
    {
        Pending =1, 
        Performed = 2,
        Canceled = 3
    }
}
