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
    public class QualityControl: QPCBaseClass
    {
        public QualityControl()
        {
            Instructions = new Collection<Instruction>();
        }

        public QualityControl(User user)
        {
            Instructions = new Collection<Instruction>();
            SetTraceabilityValues(user);
            Status = QualityControlStatus.Open;
        }

        [Required, StringLength(15, ErrorMessage ="{0} Should be at least 10 caracters long.")]
        public string Serial { get; set; }

        public QualityControlStatus Status { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int DefectId { get; set; }
        public Defect Defect { get; set; }

        public virtual ICollection<Instruction> Instructions { get; set; }
        
        public FinalDesicion Desicion { get; set; }


        public void AddInstruction(InstructionViewModel vm, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");

            var instruction = new Instruction
            {
                QualityControlId = Id,
                Name = vm.Name,
                Description = vm.Description,
                Comments = vm.Comments,
                Status = InstructionStatus.Pending
            };

            SetTraceabilityValues(user);
            instruction.SetTraceabilityValues(user);
            Status = QualityControlStatus.InProgress;

            Instructions.Add(instruction);
        }

        public void Update(QualityControlDetailViewModel vm, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");
            Name = vm.Name;
            Description = vm.Name;
            SetTraceabilityValues(user);
        }

        public void SetFinalDesicion(DesicionDto dto, User user)
        {
            if (Status == QualityControlStatus.Closed)
                throw new Exception("Current status does not allow to add more instructions.");

            Status = QualityControlStatus.Closed;
            var finalDesicion = new FinalDesicion
            {
                Comments = dto.Comments,
                Desicion = dto.Desicion
            };
            Desicion = finalDesicion;
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
