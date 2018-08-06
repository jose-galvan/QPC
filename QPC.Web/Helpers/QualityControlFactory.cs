using QPC.Core;
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
        #region QualityControl Factory Methods

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

        public ListItemViewModel CreateViewModel(QualityControl control)
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

        #endregion

        #region Instruction Factory Methods
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
        #endregion

        #region Inspection Factory Methods
        public Inspection Create(InspectionViewModel viewmodel, User user)
        {
            return new Inspection(user)
            {
                Desicion = viewmodel.Desicions.Single(d => d.Id == viewmodel.FinalDesicison),
                Comments = viewmodel.Comments
            };
        }
        public InspectionDto Create(Inspection model)
        {
            return new InspectionDto
            {
                Id = model.Id,
                Desicion = model.Desicion.Id,
                Comments = model.Comments
            };
        }
        public Inspection Create(InspectionDto dto, User user)
        {
            return new Inspection(user)
            {
                Comments = dto.Comments
            };
        }
        #endregion

        #region Product Factory Methods
        public Product Create(BaseModel viewModel, User user)
        {
            return new Product(user)
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };
        }

        public ProductDto Create(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description
            };
        }
        #endregion

        #region Defect Factory Methods

        public Defect Create(DefectViewModel viewModel, User user)
        {
            return new Defect(user)
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Product = viewModel.Products.Single(p => p.Id == viewModel.Product)
            };
        }

        public Defect Create(DefectDto dto, User user)
        {
            return new Defect(user)
            {
                Name = dto.Name,
                Description = dto.Description,
                ProductId = dto.ProductId
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

        public DefectDto CreateDto(Defect defect)
        {
            return new DefectDto
            {
                Id = defect.Id,
                Name = defect.Name,
                Description = defect.Description,
                ProductId = defect.ProductId,
                Product = defect.Product.Name
            };
        }

        #endregion

        #region Helpers
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
        #endregion
    }
}