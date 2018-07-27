using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Controllers.Api;
using QPC.Web.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class InstructionControllerTests : ControllerBaseTests
    {
        private Mock<IQualityControlRepository> _mockRepository;
        private Mock<IInstructionRepository> _mockInstructionRepository;
        private InstructionController _controller;

        private string _userId;
        private InstructionDto instructionDto;
        private Instruction instruction;
        private QualityControl control;
        private Instruction secondInstruction;
        private List<Instruction> instructions;

        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockInstructionRepository = new Mock<IInstructionRepository>();


            _controller = new InstructionController(_mockUnitOfWork.Object, _mockFactory.Object);

            //Mocks SetUp
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.InstructionRepository).Returns(_mockInstructionRepository.Object);
            //Mock User Identity
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@mail.com");
            _mockUnitOfWork.Setup(uw => uw.InstructionRepository.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instruction));
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
            _mockUnitOfWork.Setup(uw => uw.InstructionRepository.GetWithQualityControl(It.IsAny<int>())).Returns(Task.FromResult(instruction));

        }

        protected override void InitializeMockData()
        {
            instructionDto = new InstructionDto
                {
                    Name = "Dimensional Control",
                    Description = "Control Dimensions DI010 and DI0320",
                    QualityControlId = 1
                };

            instruction = new Instruction
                {
                    Id = 1,
                    Name = "CMM Control",
                    Description = "remeasure DI010 and DI0320",
                    QualityControlId = 1,
                    Status = InstructionStatus.Performed
                };
            secondInstruction = new Instruction
            {
                Id = 1,
                Name = "Shipping",
                Description = "Shipping Product",
                QualityControlId = 1,
                Status = InstructionStatus.Pending
            };
            control = new QualityControl
                {
                    Id = 1,
                    Name = "High tolerances",
                    Defect = new Defect { Name = "Dimensional" },
                    Status = QualityControlStatus.Open,
                    Instructions = new List<Instruction>() { instruction, secondInstruction }
            };
            instruction.QualityControl = control;
        }

        [TestMethod]
        public async Task AddInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Act
            var result = await _controller.AddInstructionAsync(instructionDto);
            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task AddInstruction_NonExistingControl_ShouldReturnNotFound()
        {
            // Arrange
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(controlNull));
            // Act
            var result = await _controller.AddInstructionAsync(instructionDto);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddInstruction_ClosedControl_ShouldReturnBadRequest()
        {
            // Arrange
            control.Status = QualityControlStatus.Closed;
            // Act
            var result = await _controller.AddInstructionAsync(instructionDto);
            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Arrange
            control.Status = QualityControlStatus.Open;
            instruction.Status = InstructionStatus.Pending;
            // Act
            var result = await _controller.UpdateInstructionAsync(1);
            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_PerformedInstruction_ShouldReturnBadRquest()
        {
            // Arrange
            instruction.Status = InstructionStatus.Performed;
            instruction.QualityControl = control;
            // Act
            var result = await _controller.UpdateInstructionAsync(1);
            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task UpdateInstruction_InstructionInClosedControl_ShouldReturnBadRquest()
        {
            // Arrange
            instruction.Status = InstructionStatus.Pending;
            control.Status = QualityControlStatus.Closed;
            instruction.QualityControl = control;
            // Act
            var result = await _controller.UpdateInstructionAsync(1);
            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }
        
        [TestMethod]
        public async Task UpdateInstruction_NonExistingInstruction_ShouldReturnNotFound()
        {
            // Arrange
            Instruction instructionNull = null;
            _mockUnitOfWork.Setup(uw => uw.InstructionRepository.GetWithQualityControl(It.IsAny<int>())).Returns(Task.FromResult(instructionNull));
            // Act
            var result = await _controller.UpdateInstructionAsync(1);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_ValidInstruction_ShouldReturnOk()
        {
            // Arrange
            instruction.Status = InstructionStatus.Pending;
            control.Status = QualityControlStatus.Open;
            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_PerformedInstruction_ShouldReturnBadRequest()
        {
            // Arrange
            instruction.Status = InstructionStatus.Performed;
            // Act
            var result = await _controller.CancelInstructionAsync(1);
            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_InstructionInClosedControl_ShouldReturnBadRequest()
        {
            // Arrange
            instruction.Status = InstructionStatus.Pending;
            control.Status = QualityControlStatus.Closed;
            // Act
            var result = await _controller.CancelInstructionAsync(1);

            // Assert
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public async Task CancelInstruction_NonExistingInstruction_ShouldReturnNotFound()
        {
            // Arrange
            Instruction instructionNull = null;
            _mockUnitOfWork.Setup(uw => uw.InstructionRepository.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(instructionNull));
            // Act
            var result = await _controller.CancelInstructionAsync(1);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
     
        [TestMethod]
        public async Task GetInstructionByControl_ShouldReturnOkNegotiatedContentResult()
        {
            //Arrange
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                           .GetWithDetailsAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(control));
                    
            //Act
            var result =await  _controller.GetByProduct(1);
            //Assert
            result.Should().BeOfType<OkNegotiatedContentResult<ICollection<Instruction>>>();
        }

        [TestMethod]
        public async Task GetInstructionByControl_ShouldNotFoundResult()
        {
            //Arrange
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                           .GetWithDetailsAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(controlNull));

            //Act
            var result = await _controller.GetByProduct(1);
            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
