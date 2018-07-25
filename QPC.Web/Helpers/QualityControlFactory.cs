using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace QPC.Web.Helpers
{
    public class QualityControlFactory
    {

        public QualityControlDetailViewModel Create(QualityControl control)
        {
            return new QualityControlDetailViewModel
            {
                Id = control.Id,

                Serial = control.Serial,
                Name = control.Name,
                Description = control.Description,

                Status = GetDescription(control.Status),
                Defect = control.Defect.Name,
                DefectDescription = control.Defect.Description,
                Product = control.Product.Name,
                ProductDescription = control.Product.Description,
                LastModificationDate = control.LastModificationDate.ToString(),
                CreateDate = control.CreateDate.ToString(),
                UserCreated = control.UserCreated.UserName,
                LastModificationUser = control.LastModificationUser.UserName,
                CanSave = !control.Instructions
                            .Any(i => i.Status == InstructionStatus.Pending) &&
                          control.Status != QualityControlStatus.Closed ? true : false
            };
        }

        public QualityControl Create(QualityControlViewModel model, User user)
        {
            return new QualityControl(user)
            {
                ProductId = model.Product,
                DefectId = model.Defect,
                Name = model.Name,
                Serial = model.Serial,
                Description = model.Description,
                Status = QualityControlStatus.Open,
            };
        }

        public QualityControl Create(QualityControlDto dto, User user)
        {
            return new QualityControl(user)
            {
                ProductId = dto.Product,
                DefectId = dto.Defect,
                Name = dto.Name,
                Serial = dto.Serial,
                Description = dto.Description,
                Status = QualityControlStatus.Open,
            };
        }

        public Instruction Create(InstructionViewModel model, User user)
        {
            return new Instruction(user)
            {
                QualityControlId = model.QualityControlId,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,
                Status = InstructionStatus.Pending
            };
        }
        public Instruction Create(InstructionDto model, User user)
        {
            return new Instruction(user)
            {
                QualityControlId = model.QualityControlId,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,
                Status = InstructionStatus.Pending
            };
        }

        public Inspection Create(InspectionViewModel viewmodel, User user)
        {
            return new Inspection(user)
            {
                Desicion = viewmodel.Desicions.Single(d => d.Id == viewmodel.FinalDesicison),
                Comments = viewmodel.Comments
            };
        }

        public Inspection Create(InspectionDto dto, User user)
        {
            return new Inspection(user)
            {
                Comments = dto.Comments
            };
        }
        
        public Product Create(ProductViewModel viewModel, User user)
        {
            return new Product(user)
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };
        }

        public Defect Create(DefectViewModel viewModel, User user)
        {
            return new Defect(user)
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Product = viewModel.Products.Single(p => p.Id == viewModel.Product)
            };
        }

        public DefectItemViewModel Create(Defect defect)
        {
            return new DefectItemViewModel
            {
                Id = defect.Id,
                Name = defect.Name,
                Description = defect.Description,
                Product = defect.Product.Name
            };
        }

        public ListItemViewModel CreateItem(QualityControl control)
        {
            return new ListItemViewModel
            {
                Id = control.Id,
                Name = control.Name,
                Description = control.Description,
                Status = GetDescription(control.Status),
                Product = control.Product.Name,
                Defect = control.Defect.Name,
                Desicion = control.Inspection != null 
                    ? control.Inspection.Desicion.Name : string.Empty
            };
        }

        private string GetDescription<T>(T enumerationValue)
        where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

    }
}