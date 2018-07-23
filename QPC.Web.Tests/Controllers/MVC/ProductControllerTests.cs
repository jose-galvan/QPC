using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Repositories;
using QPC.Web.Controllers;
using QPC.Web.Helpers;
using QPC.Web.Tests.Extensions;
using System.Threading.Tasks;
using QPC.Core.Models;
using QPC.Core.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILogRepository> _mockLogRepository;

        private ProductsController _controller;
        private Mock<QualityControlFactory> _mockFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IProductRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogRepository = new Mock<ILogRepository>();

            _mockFactory = new Mock<QualityControlFactory>();

            _mockUnitOfWork.SetupGet(uw => uw.ProductRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.LogRepository).Returns(_mockLogRepository.Object);

            _controller = new ProductsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => ControllerExtensions.GetGuid("1571");
        }

        [TestMethod]
        public async Task GetAllProducts_ShouldReturnTwoItems()
        {
            //Arrange
            string query = string.Empty;
            var products = new List<Product>()
            {
                new Product {
                    Name = "Blade",
                    Description = "x35 Blade F12",
                },
                new Product {
                    Name = "Blade F1",
                    Description = "x35 Blade F10",
                }
            };
            

            _mockRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

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
            string query ="f10";
            var products = new List<Product>()
            {
                new Product {
                    Name = "Blade",
                    Description = "x35 Blade F12",
                },
                new Product {
                    Name = "Blade F10",
                    Description = "x35 Blade F10",
                }
            };


            _mockRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

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
            var user = new User
            {
                UserId = ControllerExtensions.GetGuid("1571"),
                UserName = "user@mail.com"
            };
            var products = new List<Product>();
            var viewModel = new ProductViewModel()
            {
                Name = "Blade",
                Description = "x35 Blade F12",
                Products = products
            };
            _mockRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

            _mockRepository.Setup(r => r.GetAsync(It.IsAny< Expression<Func<Product, bool>>>()))
                                .Returns(Task.FromResult(products));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.SaveProduct(viewModel) as ViewResult;
            var model = result.Model as ProductViewModel;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Products.Count);
        }

        [TestMethod]
        public async Task AddProduct_ExistingProduct()
        {
            //Arrange
            var user = new User
            {
                UserId = ControllerExtensions.GetGuid("1571"),
                UserName = "user@mail.com"
            };
            var products = new List<Product>()
            {
                new Product {
                    Name = "Blade",
                    Description = "x35 Blade F12",
                }
            };
            var viewModel = new ProductViewModel()
            {
                Name = "Blade",
                Description = "x35 Blade F12",
                Products = products
            };

            _mockRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                                .Returns(Task.FromResult(products));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

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
            var user = new User
            {
                UserId = ControllerExtensions.GetGuid("1571"),
                UserName = "user@mail.com"
            };

            var product =
                new Product
                {
                    Id = 1,
                    Name = "Blade",
                    Description = "x35 Blade F12",
                };

            var viewModel = new ProductViewModel()
            {
                Id = 1,
                Name = "Blade F12",
                Description = "x35 Blade F12"
            };

            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                                .Returns(Task.FromResult(product));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.SaveProduct(viewModel) as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateProduct_NonExistingProduct()
        {
            //Arrange
            var user = new User
            {
                UserId = ControllerExtensions.GetGuid("1571"),
                UserName = "user@mail.com"
            };

            Product product = null;

            var viewModel = new ProductViewModel()
            {
                Id = 1,
                Name = "Blade F12",
                Description = "x35 Blade F12"
            };

            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                                .Returns(Task.FromResult(product));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.SaveProduct(viewModel) as HttpNotFoundResult;

            // Assert            
            Assert.AreEqual(404, result.StatusCode);
        }


    }
}
