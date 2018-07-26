using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class InspectionsControllerTests: ControllerBaseTests
    {
        
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IDesicionRepository> _mockDesicionRepository;
        private InspectionsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockDesicionRepository = new Mock<IDesicionRepository>();

            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.DesicionRepository).Returns(_mockDesicionRepository.Object);

            _controller = new InspectionsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");

            _mockDesicionRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(desicions));

        }
        QualityControl control;
        private List<Desicion> desicions;
        private InspectionViewModel viewModel;

        protected override void InitializeMockData()
        {
            _user = new User { UserId = GetGuid("1571"), UserName = "user@mail.com" };
            control = new QualityControl
            {
                Id = 1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>(),
                Status = QualityControlStatus.Open
            };
            desicions = new List<Desicion>()
            {
                new Desicion { Id =1,  Name = "Acepted" },
                new Desicion { Id =2, Name = "Rejected" },
                new Desicion { Id =3, Name = "Rework" }
            };

            viewModel = new InspectionViewModel()
            {
                QualityControlId = 1,
                FinalDesicison = 1,
                Comments = "Compliant after dimensional control"
            };
        }

        [TestMethod]
        public async Task Inspect_ValidInspection()
        {
            //Arrange
            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            //Act 
            var result = await _controller.Inspect(viewModel) as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Inspect_ControlClosed()
        {
            //Arrange
            control.Status = QualityControlStatus.Closed;
            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            //Act 
            var result = await _controller.Inspect(viewModel) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Current status does not allow to add more instructions.", result.StatusDescription);
        }


        [TestMethod]
        public async Task Inspect_WithInstructionStillPending()
        {
            //Arrange
            control.Status = QualityControlStatus.Open;
            control.Instructions = new List<Instruction>()
            {
                new Instruction {
                    Name ="CMM",
                    Status = InstructionStatus.Pending }
            };            
            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            //Act 
            var result = await _controller.Inspect(viewModel) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("All instructions should be performed before final inspection.", result.StatusDescription);
        }
        
        [TestMethod]
        public async Task Inspect_NonExisting()
        {
            //Arrange
            QualityControl controlNullable = null;
            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(controlNullable));

            //Act 
            var result = await _controller.Inspect(viewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
