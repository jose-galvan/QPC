using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers;
using QPC.Web.Helpers;
using QPC.Web.Tests.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace QPC.Web.Tests.Controllers.Mvc
{
    [TestClass]
    public class QualityControlControllerTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private QualityControlController _controller;
        private Mock<IUserRepository> _mockUserRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _controller = new QualityControlController(_mockUnitOfWork.Object);

            _controller.GetUserId = () => Extensions.ControllerExtensions.GetGuid("1571");
        }

        [TestMethod]
        public async Task RequestControl_ValidQualityControl()
        {
            // Arrange
            _mockRepository.Setup(r => r.Add(It.IsAny<QualityControl>()));
            var control = new QualityControlViewModel
            {
                Name = "High tolerances", 
                Description = "Dimensions DI310 and C220 out of upper tolerance.", 
                Serial ="SN18972123",
                Defect =1, 
                Product =1
            };

            // Act
            var result = await _controller.RequestControl(control);
            _mockRepository.Verify(r => r.Add(It.Is<QualityControl>(qc => qc.Name == "High tolerances")));

            var redirectResult = result.As<RedirectToRouteResult>();

            var expectedRedirectValues = new RouteValueDictionary
                                            {
                                                { "action", "Index" },
                                                { "controller", "QualityControl" }
                                            };
            // Assert
            redirectResult.RouteValues.Should().Equals(expectedRedirectValues);
        }

        [TestMethod]
        public async Task Update_ExistingQualityControl()
        {
            // Arrange
            var control = new QualityControl
            {
                Id = 1,
                Name = "High tolerances"
            };


            var user = new User { UserId = ControllerExtensions.GetGuid("1571"), UserName = "user@mail.com" };
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
            var dto = new QualityControlDetailViewModel { Id = 1, Name = "No-conforming dimensions", Description = "Dimension DI110 is out of upper tolerance." };

            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            // Act
            var result = await _controller.UpdateControl(dto) as ActionResult;

            // Assert            
            var redirectResult = result.As<RedirectToRouteResult>();
            var expectedRedirectValues = new RouteValueDictionary
                                            {
                                                { "action", "Index" },
                                                { "controller", "QualityControl" }
                                            };
            // Assert
            redirectResult.RouteValues.Should().Equals(expectedRedirectValues);
        }

        [TestMethod]
        public async Task Detail_SearchExistingControl()
        {
            //Arrange
            var control = new QualityControl
            {
                Id =1,
                Name = "High tolerances",
                Status = QualityControlStatus.Open,
                UserCreated = new User { UserName = "jose" },
                LastModificationUser = new User { UserName = "Juan" }, 
                Defect = new Defect { Name = "DI110", Description ="Out of tolerances"},
                Product = new Product {Name ="Blade", Description ="blade fb1" }
            };

            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                        .GetWithDetails(It.IsAny<int>()))
                        .Returns(Task.FromResult(control));
            //Act 
            var viewResult = await _controller.UpdateControl(1) as ViewResult;
            var model = viewResult.Model as QualityControlDetailViewModel;
            //Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public async Task Detail_SearchNonExistingControl()
        {
            //Arrange
            QualityControl control = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                        .FindByIdAsync(It.IsAny<int>()))
                        .Returns(Task.FromResult(control));
            //Act 
            var result = await _controller.UpdateControl(1);
            //Assert
            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [TestMethod]
        public async Task Update_NonExistingQualityControl()
        {
            // Arrange
            QualityControl control = null;

            _mockRepository.Setup(r => r.Update(It.IsAny<QualityControl>()));
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(control));
            var dto = new QualityControlDetailViewModel { Id = 0, Name = "No-conforming dimensions", Description = "Dimension DI110 is out of upper tolerance." };
            
            // Act
            var result = await _controller.UpdateControl(dto) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task Index_Returns_All_Controls_In_DB()
        {
            //Arrange
            var desicion = new Desicion { Id = 1, Name = "Rejected" };

            var controls = new List<QualityControl>
                            {
                                new QualityControl {Name ="High tolerances",
                                        Description ="Dimensional control",
                                        Product = new Product {Name ="Compressor Shaft" }, 
                                        Defect = new Defect {Name  = "Non-conforming tolerances" },
                                        Inspection = new Inspection { Desicion = desicion }
                                },
                                new QualityControl {Name ="Documental deviation",
                                        Description ="Prox p40",
                                        Product = new Product {Name ="CFM Fan Blade" },
                                        Defect = new Defect {Name  = "DEQ" },
                                        Inspection = new Inspection { Desicion = desicion }
                                }
                            };
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.GetAllWithDetailsAsync()).Returns(Task.FromResult(controls));
            //Act 
            var viewResult = await _controller.Index();
            var model = viewResult.Model as QualityControlIndexViewModel;
            //Assert
            Assert.AreEqual(2, model.Controls.ToList().Count);
        }

    }
}
