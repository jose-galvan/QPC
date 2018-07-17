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

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class DesicionControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private DesicionsController _controller;
        private Mock<IUserRepository> _mockUserRepository;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _controller = new DesicionsController(_mockUnitOfWork.Object);

            //Mocks SetUp
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            //Mock User Identity
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@mail.com");
        }

        [TestMethod]
        public async Task SetDesicion_ValidDesicion_ShouldReturnOk()
        {
            //Arrange
            var desicion = new DesicionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion = Desicion.Acepted };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));

            //Act
            var result = await _controller.AddDesicion(desicion);
            //Assert

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task SetDesicion_NonExistingDesicion_ShouldReturnNotFound()
        {
            //Arrange
            var desicion = new DesicionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion = Desicion.Acepted };
            QualityControl quaityControl =null;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));

            //Act
            var result = await _controller.AddDesicion(desicion);
            //Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task SetDesicion_ClosedControl_ShouldReturnBadRequest()
        {
            //Arrange
            var desicion = new DesicionDto { QualityControlId = 1, Comments = "Instructions' results OK", Desicion = Desicion.Acepted };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Closed };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));

            //Act
            var result = await _controller.AddDesicion(desicion);
            //Assert

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }


    }
}
