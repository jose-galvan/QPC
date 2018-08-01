using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers;
using QPC.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class InstructionsControllerTests: ControllerBaseTests
    {
        private Mock<IQualityControlRepository> _mockRepository;
        private InstructionsController _controller;
        private QualityControl control;
        private InstructionViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockFactory = new Mock<QualityControlFactory>();
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _controller = new InstructionsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
        }

        protected override void InitializeMockData()
        {
            control = new QualityControl
            {
                Id = 1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>(),
                Status = QualityControlStatus.Open
            };
            viewModel = new InstructionViewModel
            {
                QualityControlId = 1,
                Name = "CMM",
                Description = "Control dimension DI312",
                Comments = "N/A"
            };
        }

        [TestMethod]
        public async Task AddInstructon_ValidInstruction()
        {
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
            control.Status = QualityControlStatus.Closed;
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
            QualityControl controlNull = null;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(controlNull));
            //Act 
            var result = await _controller.AddInstruction(viewModel) as HttpNotFoundResult;
            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }

    }
}
