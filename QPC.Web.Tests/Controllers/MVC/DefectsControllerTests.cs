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

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class DefectsControllerTests: ControllerBaseTests
    {
        string query;
        List<Defect> defects;
        List<Product> products;
        Defect firstDefect, secondDefect, defectUpdated;
        Product firstProduct, secondProduct;
        
        private Mock<IDefectRepository> _mockRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private DefectsController _controller;
        private DefectViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IDefectRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            
            _mockUnitOfWork.SetupGet(uw => uw.DefectRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);

            _controller = new DefectsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");
            
            _mockRepository.Setup(r => r.GetWithProductAsync())
                                .Returns(Task.FromResult(defects));

            _mockProductRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));
        }

        protected override void InitializeMockData()
        {
            firstProduct = new Product { Id = 1, Name = "Blade", Description = "x35 Blade F12" };
            secondProduct = new Product { Id = 2, Name = "Blade F1", Description = "x35 Blade F10" };
            firstDefect = new Defect{Id = 1, Name = "DI321",Description = "x35 Blade F12",Product = firstProduct};
            secondDefect = new Defect{ Id = 2, Name = "CO410", Description = "x35 Blade F10", Product = firstProduct };
            products = new List<Product>(){firstProduct, secondProduct};
            defects = new List<Defect>(){firstDefect, secondDefect};
            defectUpdated = new Defect{ Id = 2,Name = "CO410", Description = "x35 Blade F10",Product = firstProduct};
            viewModel = new DefectViewModel()
            {
                Name = "DIM442",
                Description = "Dimension 442, 0 +-0.5",
                Product = 1
            };
        }

        [TestMethod]
        public async Task GetAllDefects_ShouldReturnTwoItems()
        {
            //Arrange
            query = string.Empty;
            //Act 
            var result = await _controller.Defect(query) as ViewResult;
            var model = result.Model as DefectViewModel;
            //Assert
            Assert.AreEqual(2, model.Defects.ToList().Count);
        }

        [TestMethod]
        public async Task GetDefectsFiltered_ShouldReturnOneItem()
        {
            //Arrange
            query = "321";
            //Act 
            var result = await _controller.Defect(query) as ViewResult;
            var model = result.Model as DefectViewModel;
            //Assert
            Assert.AreEqual(1, model.Defects.Count());
        }
        
        [TestMethod]
        public async Task AddDefect_ValidDefect()
        {
            //Act 
            var result = await _controller.SaveDefect(viewModel) as ViewResult;
            var model = result.Model as DefectViewModel;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, model.Defects.Count());
        }

        [TestMethod]
        public async Task UdateDefect_ValidDefect()
        {
            //Arrange
            defects = new List<Defect>(){ defectUpdated };
            viewModel.Id = 2;
            viewModel.Name = "DIM442";
            viewModel.Description = "Dimension 442, 0 +-0.5";
            viewModel.Product = 1;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(defectUpdated));                        
            //Act 
            var result = await _controller.SaveDefect(viewModel) as ViewResult;
            var model = result.Model as DefectViewModel;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Defects.Count());
        }
        
    }
}
