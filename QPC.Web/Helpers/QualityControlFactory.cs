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
        
        public static QualityControlDetailViewModel Create(QualityControl control)
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
                LastModificationUser = control.LastModificationUser.UserName
            };
        }

        public static QualityControl Create(QualityControlViewModel model, User user)
        {
            var control = new QualityControl(user);
            control.ProductId = model.Product;
            control.DefectId = model.Defect;
            control.Name = model.Name;
            control.Serial = model.Serial;
            control.Description = model.Description;
            control.Status = QualityControlStatus.Open;
            return control;
        }

        public static Instruction Create(InstructionViewModel model)
        {
            return new Instruction
            {
                QualityControlId = model.QualityControlId,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,
                Status = InstructionStatus.Pending
            };
        }
        public static Instruction Create(InstructionDto model)
        {
            return new Instruction
            {
                QualityControlId = model.QualityControlId,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,
                Status = InstructionStatus.Pending
            };
        }

        public static Inspection Create(InspectionViewModel vm)
        {
            return new Inspection
            {
                Desicion = vm.Desicions.Single(d => d.Id == vm.FinalDesicison),
                Comments = vm.Comments
            };
        }

        public static Inspection Create(InspectionDto dto)
        {
            return new Inspection
            {
                Comments = dto.Comments
            };
        }

        public static ListItemViewModel CreateItem(QualityControl control)
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

        public static string GetDescription<T>(T enumerationValue)
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