using QPC.Core.DTOs;
using QPC.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QPC.Core.Models
{
    public class QualityControl: QPCBaseModel
    {
        public QualityControl()
        {
            Instructions = new Collection<Instruction>();
        }

        public QualityControl(User user):base(user)
        {
            Instructions = new Collection<Instruction>();
            Status = QualityControlStatus.Open;
        }

        [Required, StringLength(15, ErrorMessage ="{0} Should be at least 10 caracters long.")]
        public string Serial { get; set; }



        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }


        public QualityControlStatus Status { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int DefectId { get; set; }
        public Defect Defect { get; set; }

        public virtual ICollection<Instruction> Instructions { get; set; }
        
        public Inspection Inspection { get; set; }


        public void AddInstruction(Instruction instruction, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");
            
            SetTraceabilityValues(user);
            Status = QualityControlStatus.InProgress;
            Instructions.Add(instruction);
        }

        public void Update(QualityControlDetailViewModel vm, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");
            Name = vm.Name;
            Description = vm.Description;
            SetTraceabilityValues(user);
        }

        public void SetInspection(Inspection inspection, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");

            if (Instructions.Any(i => i.Status == InstructionStatus.Pending))
                throw new Exception("All instructions should be performed before final inspection.");
            
            Status = QualityControlStatus.Closed;
            Inspection = inspection;
            SetTraceabilityValues(user);
        }
    }

    public enum QualityControlStatus
    {
        [Description("Open")]
        Open =1,

        [Description("In Progress")]
        InProgress = 2,

        [Description("Closed")]
        Closed = 3
    }



}
