using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Core.ViewModels;
using QPC.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class ProductControllerTests: ControllerBaseTests
    {

        private string query;
        private List<Product> products;
        private ProductViewModel viewModel;
        private ProductsController _controller;
        private Product firstProduct, secondProduct;
        private Mock<IProductRepository> _mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork.SetupGet(uw => uw.ProductRepository).Returns(_mockRepository.Object);

            _controller = new ProductsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");
            
            _mockRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                                .Returns(Task.FromResult(new List<Product>()));
        }

        protected override void InitializeMockData()
        {
            firstProduct = new Product {Id =1, Name = "Blade", Description = "x35 Blade F12" };
            secondProduct = new Product {Id =2, Name = "Blade F1", Description = "x35 Blade F10" };
            products = new List<Product>(){firstProduct, secondProduct};
            viewModel = new ProductViewModel()
            {
                Name = "Blade",
                Description = "x35 Blade F12",
                Products = products
            };
        }

        [TestMethod]
        public async Task GetAllProducts_ShouldReturnTwoItems()
        {
            //Arrange
            query = string.Empty;
            //Act 
            var result = await _controller.Product(query) as ViewResult;
            var model = result.Model as ProductViewModel;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Products.Count);
        }

        [TestMethod]
        public async Task GetProductsFiltered_ShouldReturnOneItem()
        {
            //Arrange
            query ="f10";
            //Act 
            var result = await _controller.Product(query) as ViewResult;
            var model = result.Model as ProductViewModel;

            //Assert
            Assert.AreEqual(1, model.Products.Count);
        }
        
        [TestMethod]
        public async Task AddProduct_ValidProduct()
        {
            //Arrange
            viewModel.Name = "Shaft";
            viewModel.Description = "Shaft x43";
            //Act 
            var result = await _controller.SaveProduct(viewModel) as ViewResult;
            var model = result.Model as ProductViewModel;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, model.Products.Count);
        }

        [TestMethod]
        public async Task AddProduct_ExistingProduct()
        {
            //Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                                .Returns(Task.FromResult(new List<Product>() { firstProduct}));
            //Act 
            var result = await _controller.SaveProduct(viewModel) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Product Already registered.", result.StatusDescription);
        }
        
        [TestMethod]
        public async Task UpdateProduct_ValidProduct()
        {
            //Arrange
            viewModel.Id = 1;
            viewModel.Name = "Blade F12";
            viewModel.Description = "x35 Blade F12";
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                                .Returns(Task.FromResult(firstProduct));
            //Act 
            var result = await _controller.SaveProduct(viewModel) as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateProduct_NonExistingProduct()
        {
            //Arrange
            Product product = null;
            viewModel.Id = 1;
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                                .Returns(Task.FromResult(product));
            //Act 
            var result = await _controller.SaveProduct(viewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }

    }
}
