using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Controllers.Api;
using QPC.Web.Tests.Extensions;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class InspectionsControllerTests : ControllerBaseTests
    {
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IDesicionRepository> _mockDesicionRepository;
        private InspectionsController _controller;
        private string _userId;
        private Desicion desicion;
        private InspectionDto inspection;
        private QualityControl control;

        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockDesicionRepository = new Mock<IDesicionRepository>();


            _controller = new InspectionsController(_mockUnitOfWork.Object, _mockFactory.Object);

            //Mocks SetUp
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.DesicionRepository).Returns(_mockDesicionRepository.Object);
            //Mock User Identity
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@mail.com");

            _mockDesicionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(desicion));
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
        }

        protected override void InitializeMockData()
        {
            desicion = new Desicion { Id = 1, Name = "Acepted" };
            inspection = new InspectionDto
                    {
                        QualityControlId = 1,
                        Comments = "Instructions' results OK",
                        Desicion = 1
                    };
            control = new QualityControl
                    {
                        Name = "High tolerances",
                        Defect = new Defect { Name = "Dimensional" },
                        Status = QualityControlStatus.Open
                    };

        }

        [TestMethod]
        public async Task Inspection_ValidDesicion_ShouldReturnOk()
        {
            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert
            result.Should().BeOfType<OkResult>();
        }
        
        [TestMethod]
        public async Task Inspection_ClosedControl_ShouldReturnBadRequest()
        {
            //Arrange
            control.Status = QualityControlStatus.Closed;
            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task Inspection_NonExistingDesicion_ShouldReturnNotFound()
        {
            //Arrange
            QualityControl controlNull = null;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(controlNull));
            //Act
            var result = await _controller.AddDesicion(inspection);
            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }


    }
}
