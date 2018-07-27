using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.DTOs;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class ProductsControllerTests : ControllerBaseTests
    {
        private string query;
        private List<Product> products;
        private ProductsController _controller;
        private Product firstProduct, secondProduct;
        private Mock<IProductRepository> _mockRepository;
        private ProductDto dto;

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
            firstProduct = new Product { Id = 1, Name = "Blade", Description = "x35 Blade F12" };
            secondProduct = new Product { Id = 2, Name = "Blade F1", Description = "x35 Blade F10" };
            products = new List<Product>() { firstProduct, secondProduct };
            dto = new ProductDto { Id = 1, Name = "Blade", Description = "x35 Blade F12" };
        }

        [TestMethod]
        public async Task GetAllProducts_ShouldReturnTwoItems()
        {
            //Act 
            var result = await _controller.Get();
            //Assert
            result.Should().BeOfType<List<ProductDto>>();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetProductsFiltered_ShouldReturnOneItem()
        {
            //Arrange
            query = "f10";
            _mockUnitOfWork.Setup(uw => uw.ProductRepository
                    .GetAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                    .Returns(Task.FromResult(products));
            //Act 
            var result = await _controller.Get(query);
            //Assert
            result.Should().BeOfType<List<ProductDto>>();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnProduct()
        {
            //Arrange
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.FindByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(firstProduct));
            //Act 
            var result = await _controller.GetById(1);
            //Assert
            result.Should().BeOfType<OkNegotiatedContentResult<ProductDto>>();
        }
        [TestMethod]
        public async Task GetById_UnexistingProduct_ShouldReturnNotFound()
        {
            //Arrange
            Product productNull = null;
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.FindByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(productNull));
            //Act 
            var result = await _controller.GetById(1);
            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddProduct_ShouldReturnOkResult()
        {
            //arrange
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.Add(It.IsAny<Product>()));
            //act 
            var result = await _controller.Add(dto);
            //assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateProduct_ShouldReturnOkResult()
        {
            //arrange
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.FindByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(firstProduct));
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.Update(It.IsAny<Product>()));
            //act 
            var result = await _controller.Update(dto);
            //assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateProduct_UnexistingProduct_ShouldReturnNotFound()
        {
            //arrange
            Product productNull = null;
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.FindByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(productNull));
            _mockUnitOfWork.Setup(uw => uw.ProductRepository.Update(It.IsAny<Product>()));
            //act 
            var result = await _controller.Update(dto);
            //assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
