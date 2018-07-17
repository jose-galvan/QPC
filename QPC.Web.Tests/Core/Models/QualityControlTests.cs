using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.DTOs;

namespace QPC.Web.Tests.Core.Models
{
    [TestClass]
    public class QualityControlTests
    {
        private Mock<QualityControl> _qualityControl;

        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _qualityControl = new Mock<QualityControl>();
        }


        public void AddInstruction_ValidInstruction()
        {
            //Arrange
            var dto = new InstructionDto { Name = "CMM Control", Description = "Measure DI110 CO290" };
            var user = new User { UserId = Extensions.ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            //Act 
            //_qualityControl.Object.AddInstruction(dto, user);
            //Assert

        }
    }
}
