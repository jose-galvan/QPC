using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace QPC.Web.Tests.Controllers.Mvc
{
    [TestClass]
    public class QualityControlControllerTest: ControllerBaseTests
    {


        private Mock<IQualityControlRepository> _mockRepository;

        private QualityControlController _controller;
        private QualityControlViewModel viewModel;
        private RouteValueDictionary expectedRedirectValues;
        private QualityControlDetailViewModel detailViewModel;
        private QualityControl _mockControl;
        private Desicion desicion;
        private List<QualityControl> controls;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _controller = new QualityControlController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");
            
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                        .GetWithDetails(It.IsAny<int>()))
                        .Returns(Task.FromResult(_mockControl));


            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                        .FindByIdAsync(It.IsAny<int>()))
                        .Returns(Task.FromResult(_mockControl));
            
        }
        
        protected override void InitializeMockData()
        {

            desicion = new Desicion { Id = 1, Name = "Rejected" };

            _mockControl = new QualityControl(_user)
            {
                Id = 1,
                Name = "High tolerances",
                Status = QualityControlStatus.Open,
                Defect = new Defect { Name = "DI110", Description = "Out of tolerances" },
                Product = new Product { Name = "Blade", Description = "blade fb1" }
            };
            controls = new List<QualityControl>
                            {
                                _mockControl,
                                new QualityControl {Name ="Documental deviation",
                                        Description ="Prox p40",
                                        Product = new Product {Name ="CFM Fan Blade" },
                                        Defect = new Defect {Name  = "DEQ" },
                                        Inspection = new Inspection { Desicion = desicion }
                                }
                            };

            viewModel = new QualityControlViewModel
            {
                Name = "High tolerances",
                Description = "Dimensions DI310 and C220 out of upper tolerance.",
                Serial = "SN18972123",
                Defect = 1,
                Product = 1
            };
            detailViewModel = new QualityControlDetailViewModel
                    {
                        Id = 1,
                        Name = "No-conforming dimensions",
                        Description = "Dimension DI110 is out of upper tolerance."
                    };
            
            expectedRedirectValues = new RouteValueDictionary
                                            {
                                                { "action", "Index" },
                                                { "controller", "QualityControl" }
                                            };
        }

        [TestMethod]
        public async Task RequestControl_ValidQualityControl()
        {
            // Arrange
            _mockUnitOfWork.Setup(r => r.QualityControlRepository.Add(It.IsAny<QualityControl>()));
            // Act
            var result = await _controller.RequestControl(viewModel);
            var redirectResult = result.As<RedirectToRouteResult>();
            _mockUnitOfWork.Verify(r => r.QualityControlRepository
                            .Add(It.Is<QualityControl>(qc => qc.Name == "High tolerances")));
            // Assert
            redirectResult.RouteValues.Should().Equals(expectedRedirectValues);
        }

        [TestMethod]
        public async Task Update_ExistingQualityControl()
        {
            // Arrange
            var dto = new QualityControlDetailViewModel { Id = 1, Name = "No-conforming dimensions", Description = "Dimension DI110 is out of upper tolerance." };            
            // Act
            var result = await _controller.UpdateControl(dto) as ActionResult;
            // Assert            
            var redirectResult = result.As<RedirectToRouteResult>();           
            redirectResult.RouteValues.Should().Equals(expectedRedirectValues);
        }

        [TestMethod]
        public async Task Update_NonExistingQualityControl()
        {
            // Arrange
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.FindByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult(controlNull));
            // Act
            var result = await _controller.UpdateControl(detailViewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task Detail_SearchExistingControl()
        {
            //Arrange
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
            QualityControl controlNull = null;
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository
                            .GetWithDetails(It.IsAny<int>()))
                            .Returns(Task.FromResult(controlNull));
            //Act 
            var result = await _controller.UpdateControl(1);
            //Assert
            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [TestMethod]
        public async Task Index_Returns_All_Controls_In_DB()
        {
            //Arrange
            _mockUnitOfWork.Setup(uw => uw.QualityControlRepository.GetAllWithDetailsAsync()).Returns(Task.FromResult(controls));
            //Act 
            var viewResult = await _controller.Index();
            var model = viewResult.Model as QualityControlIndexViewModel;
            //Assert
            Assert.AreEqual(2, model.Controls.ToList().Count);
        }

    }
}
