using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class QualityControlControllerTests : ControllerBaseTests
    {
        private QualityControlDto dto;
        private QualityControlController _controller;
        private Mock<IQualityControlRepository> _mockRepository;
        private QualityControl control;
        private List<QualityControl> controls;
        private QualityControl secondControl;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IQualityControlRepository>();
            
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _controller = new QualityControlController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");

            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.Update(It.IsAny<QualityControl>()));

            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(control));

            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.GetAllWithDetailsAsync())
                            .Returns(Task.FromResult(controls));
        }

        protected override void InitializeMockData()
        {
            dto = new QualityControlDto
            {
                Id = 1,
                Name = "High tolerances",
                Description = "Dimensions DI310 and C220 out of upper tolerance.",
                Serial = "SN18972123",
                Defect = 1,
                Product = 1
            };
            control = new QualityControl
            {
                Id = 1,
                Name = "High tolerances",
                Description = "Dimensions DI310 and C220 out of upper tolerance.",
                Defect = new Defect { Name = "Scratch" },
                Product = new Product { Name = "Blade F23" },
                Status = QualityControlStatus.InProgress
            };
            secondControl = new QualityControl
            {
                Name = "MPI",
                Description = "Scratch in zone D3",
                Defect = new Defect { Name = "Scratch" },
                Product = new Product { Name = "Blade F23" },
                Status = QualityControlStatus.Open
            };
            controls = new List<QualityControl>(){control, secondControl};
        }

        [TestMethod]
        public async Task RequestControl_ValidQualityControl_ShouldReturnOk()
        {
            // Arrange
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.Add(It.IsAny<QualityControl>()));
            // Act
            var result = await _controller.Create(dto);
            _mockUnitOfWork.Verify(uw => uw.QualityControlRepository.Add(It.Is<QualityControl>(qc => qc.Name == "High tolerances")));
            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateControl_ValidQualityControl_ShouldReturnOk()
        {
            // Arrange
            dto.Name = "Non Conforming dimensions";
            dto.Description = "Dimensions DI310 and C220 are non comforming based on standard tolerances.";
            // Act
            var result = await _controller.Update(dto);
            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateControl_ClosedControl_ShouldBadRequest()
        {
            // Arrange
            control.Status = QualityControlStatus.Closed;
            // Act
            var result = await _controller.Update(dto);
            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task UpdateControl_NonExistingControl_ShouldNotFound()
        {
            // Arrange
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(controlNull));
            // Act
            var result = await _controller.Update(dto);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [TestMethod]
        public async Task GetControls_ShouldReturnAllcontrols()
        {
            //Act
            var result =await  _controller.GetAll();
            //Assert
            result.Should().BeOfType<List<ListItemViewModel>>();
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetById_ShouldReturnControl()
        {
            //Arrange
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(control));
            //Act
            var result = await _controller.GetById(1);
            //Assert
            result.Should().BeOfType<QualityControl>();
        }

        [TestMethod]
        public async Task GetById_NonExistingControl_ShouldReturnNotFound()
        {
            //Arrange
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(controlNull));
            //Act
            var result = await _controller.GetById(1);
            //Assert
            Assert.IsNull(result);
        }
    }
}
