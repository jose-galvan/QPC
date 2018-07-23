using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using Moq;
using QPC.Web.Controllers;
using QPC.Core.Models;
using System.Collections.Generic;
using QPC.Core.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using QPC.Web.Tests.Extensions;
using System.Linq;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class InstructionsControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private InstructionsController _controller;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<QualityControlFactory> _mockFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockFactory = new Mock<QualityControlFactory>();

            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _controller = new InstructionsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => ControllerExtensions.GetGuid("1571");
        }

        [TestMethod]
        public async Task AddInstructon_ValidInstruction()
        {
            //Arrange
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            var control = new QualityControl
            {
                Id =1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>(),
                Status = QualityControlStatus.Open
            };

            var viewModel = new InstructionViewModel
            {
                QualityControlId = 1,
                Name = "CMM",
                Description = "Control dimension DI312",
                Comments = "N/A"
            };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.AddInstruction(viewModel) as ViewResult;
            var model = result.Model as InstructionViewModel;
            //Assert
            Assert.AreEqual(1, model.Instructions.ToList().Count);
        }

        [TestMethod]
        public async Task AddInstructon_ValidInstructionInClosedControl()
        {
            //Arrange
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            var control = new QualityControl
            {
                Id = 1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>(),
                Status = QualityControlStatus.Closed
            };

            var viewModel = new InstructionViewModel
            {
                QualityControlId = 1,
                Name = "CMM",
                Description = "Control dimension DI312",
                Comments = "N/A"
            };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.AddInstruction(viewModel) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Current status does not allow to add more instructions.", result.StatusDescription);

        }

        [TestMethod]
        public async Task AddInstructon_NonExistingControl()
        {
            //Arrange
            QualityControl control = null;

            var viewModel = new InstructionViewModel
            {
                QualityControlId = 1,
                Name = "CMM",
                Description = "Control dimension DI312",
                Comments = "N/A"
            };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));

            //Act 
            var result = await _controller.AddInstruction(viewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
