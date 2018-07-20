using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPC.Web.Controllers.Api;
using Moq;
using QPC.Core.Repositories;
using QPC.Web.Tests.Extensions;
using System.Threading.Tasks;
using QPC.Core.DTOs;
using QPC.Core.Models;
using FluentAssertions;
using System.Web.Http.Results;
using QPC.Web.Helpers;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class InspectionsControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IDesicionRepository> _mockDesicionRepository;
        private InspectionsController _controller;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<QualityControlFactory> _mockFactory;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockDesicionRepository = new Mock<IDesicionRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockFactory = new Mock<QualityControlFactory>();


            _controller = new InspectionsController(_mockUnitOfWork.Object, _mockFactory.Object);

            //Mocks SetUp
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.DesicionRepository).Returns(_mockDesicionRepository.Object);
            //Mock User Identity
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@mail.com");
        }

        [TestMethod]
        public async Task Inspection_ValidDesicion_ShouldReturnOk()
        {
            //Arrange
            var desicion = new Desicion { Id = 1, Name = "Acepted" };
            var inspection = new InspectionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion = 1 };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockDesicionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(desicion));

            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task Inspection_NonExistingDesicion_ShouldReturnNotFound()
        {
            //Arrange
            var desicion = new Desicion { Id = 1, Name = "Acepted" };
            var inspection = new InspectionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion = 2  };
            QualityControl quaityControl =null;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockDesicionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(desicion));

            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task Inspection_ClosedControl_ShouldReturnBadRequest()
        {
            //Arrange
            var desicion = new Desicion { Id = 1, Name = "Acepted" };
            var inspection = new InspectionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion =1 };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Closed };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockDesicionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(desicion));

            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }


    }
}
