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
    public class InstructionControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IInstructionRepository> _mockInstructionRepository;
        private InstructionController _controller;
        private Mock<IUserRepository> _mockUserRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockInstructionRepository = new Mock<IInstructionRepository>();
            _controller = new InstructionController(_mockUnitOfWork.Object);

            //Mocks SetUp
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.InstructionRepository).Returns(_mockInstructionRepository.Object);
            //Mock User Identity
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@mail.com");
        }

        [TestMethod]
        public async Task AddInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Arrange
            var instruction = new InstructionDto { Name = "Dimensional Control", Description = "Control Dimensions DI010 and DI0320", QualityControlId = 1 };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.AddInstructionAsync(instruction);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task AddInstruction_NonExistingControl_ShouldReturnNotFound()
        {
            // Arrange
            var instruction = new InstructionDto { Name = "Dimensional Control", Description = "Control Dimensions DI010 and DI0320", QualityControlId = 1 };
            QualityControl quaityControl = null;
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.AddInstructionAsync(instruction);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddInstruction_ClosedControl_ShouldReturnBadRequest()
        {
            // Arrange
            var instruction = new InstructionDto { Name = "Dimensional Control", Description = "Control Dimensions DI010 and DI0320", QualityControlId = 1 };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Closed };
            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(quaityControl));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.AddInstructionAsync(instruction);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Arrange
            var instruction = new Instruction {Id =1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1};
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };
            
            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.UpdateInstructionAsync(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_PerformedInstruction_ShouldReturnBadRquest()
        {
            // Arrange
            var instruction = new Instruction { Id = 1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1, Status = InstructionStatus.Performed };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.UpdateInstructionAsync(1);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_InstructionInClosedControl_ShouldReturnBadRquest()
        {
            // Arrange
            var instruction = new Instruction { Id = 1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1, Status = InstructionStatus.Pending};
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Closed };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.UpdateInstructionAsync(1);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }
        
        [TestMethod]
        public async Task UpdateInstruction_NonExistingInstruction_ShouldReturnNotFound()
        {
            // Arrange
            Instruction instruction = null;
            
            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));

            // Act
            var result = await _controller.UpdateInstructionAsync(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Arrange
            var instruction = new Instruction { Id = 1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1, Status = InstructionStatus.Pending };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_PerformedInstruction_ShouldReturnBadRequest()
        {
            // Arrange
            var instruction = new Instruction { Id = 1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1, Status = InstructionStatus.Performed };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Open };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_InstructionInClosedControl_ShouldReturnBadRequest()
        {
            // Arrange
            var instruction = new Instruction { Id = 1, Name = "CMM Control", Description = "remeasure DI010 and DI0320", QualityControlId = 1, Status = InstructionStatus.Pending };
            var quaityControl = new QualityControl { Name = "High tolerances", Defect = new Defect { Name = "Dimensional" }, Status = QualityControlStatus.Closed };
            instruction.QualityControl = quaityControl;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_NonExistingInstruction_ShouldReturnNotFound()
        {
            // Arrange
            Instruction instruction = null;

            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };

            _mockInstructionRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(user));

            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }



    }
}
