using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPC.Core.Repositories;
using QPC.Web.Controllers;
using QPC.Web.Helpers;
using Moq;
using QPC.Web.Tests.Extensions;
using System.Threading.Tasks;
using QPC.Core.Models;
using System.Collections.Generic;
using QPC.Core.ViewModels;
using System.Web.Mvc;
using System.Net;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class InspectionsControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IDesicionRepository> _mockDesicionRepository;
        private Mock<ILogRepository> _mockLogRepository;

        private InspectionsController _controller;
        private Mock<QualityControlFactory> _mockFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDesicionRepository = new Mock<IDesicionRepository>();
            _mockLogRepository = new Mock<ILogRepository>();

            _mockFactory = new Mock<QualityControlFactory>();

            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.DesicionRepository).Returns(_mockDesicionRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.LogRepository).Returns(_mockLogRepository.Object);

            _controller = new InspectionsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => ControllerExtensions.GetGuid("1571");
        }

        [TestMethod]
        public async Task Inspect_ValidInspection()
        {
            //Arrange
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            var control = new QualityControl
            {
                Id = 1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>(),
                Status = QualityControlStatus.Open
            };

            var viewModel = new InspectionViewModel()
            {
                QualityControlId = 1,
                FinalDesicison = 1,
                Comments = "Compliant after dimensional control"
            };

            var desicions = new List<Desicion>()
            {
                new Desicion { Id =1,  Name = "Acepted" },
                new Desicion { Id =2, Name = "Rejected" }
            };

            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _mockDesicionRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(desicions));

            //Act 
            var result = await _controller.Inspect(viewModel) as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Inspect_ControlClosed()
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

            var desicions = new List<Desicion>()
            {
                new Desicion { Id =1,  Name = "Acepted" },
                new Desicion { Id =2, Name = "Rejected" },
                new Desicion { Id =3, Name = "Rework" }
            };

            var viewModel = new InspectionViewModel()
            {
                QualityControlId = 1,
                FinalDesicison = 1,
                Comments = "Compliant after dimensional control"
            };

            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _mockDesicionRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(desicions));
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
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            var control = new QualityControl
            {
                Id = 1,
                Name = "Dimensional Control",
                Description = "Control dimensions DIM312 in CMM3",
                Instructions = new List<Instruction>()
                                {
                                    new Instruction {Name ="CMM", Status = InstructionStatus.Pending }
                                },
                Status = QualityControlStatus.Open
            };

            var desicions = new List<Desicion>()
            {
                new Desicion { Id =1,  Name = "Acepted" },
                new Desicion { Id =2, Name = "Rejected" },
                new Desicion { Id =3, Name = "Rework" }
            };

            var viewModel = new InspectionViewModel()
            {
                QualityControlId = 1,
                FinalDesicison = 1,
                Comments = "Compliant after dimensional control"
            };

            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _mockDesicionRepository.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(desicions));
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
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            QualityControl control = null;

            var viewModel = new InspectionViewModel()
            {
                QualityControlId = 1,
                FinalDesicison = 1,
                Comments = "Compliant after dimensional control"
            };

            _mockRepository.Setup(r => r.GetWithDetails(It.IsAny<int>())).Returns(Task.FromResult(control));

            //Act 
            var result = await _controller.Inspect(viewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
