using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPC.Core.Models;
using System.Collections.Generic;
using QPC.Core.Repositories;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using QPC.Core.DTOs;
using System.Web.Http.Results;
using System.Linq.Expressions;
using QPC.Api.Controllers;

namespace QPC.Api.Tests.Controllers
{
    [TestClass]
    public class DefectsControllerTests : ControllerBaseTests
    {
        string query;
        List<Defect> defects;
        List<Product> products;
        Defect firstDefect, secondDefect, defectUpdated;
        Product firstProduct, secondProduct;

        private Mock<IDefectRepository> _mockRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private DefectsController _controller;
        private DefectDto dto;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDefectRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.SetupGet(uw => uw.DefectRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);

            _controller = new DefectsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => GetGuid("1571");

            _mockUnitOfWork.Setup(uw => uw.DefectRepository.FindByIdAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(firstDefect));

            _mockUnitOfWork.Setup(uw => uw.DefectRepository.GetWithProductAsync())
                           .Returns(Task.FromResult(defects));

            _mockUnitOfWork.Setup(uw => uw.DefectRepository.Update(It.IsAny<Defect>()));

        }

        protected override void InitializeMockData()
        {
            firstProduct = new Product { Id = 1, Name = "Blade", Description = "x35 Blade F12" };
            secondProduct = new Product { Id = 2, Name = "Blade F1", Description = "x35 Blade F10" };
            firstDefect = new Defect { Id = 1, Name = "DI321", Description = "x35 Blade F12", Product = firstProduct };
            secondDefect = new Defect { Id = 2, Name = "CO410", Description = "x35 Blade F10", Product = firstProduct };
            products = new List<Product>() { firstProduct, secondProduct };
            defects = new List<Defect>() { firstDefect, secondDefect };
            defectUpdated = new Defect { Id = 2, Name = "CO410", Description = "x35 Blade F10", Product = firstProduct };

            dto = new DefectDto { Id = 1, Name = "DI230", Description = "x35 Blade F12", ProductId = firstProduct.Id };
        }

        [TestMethod]
        public async Task GetById_ShouldReturnDefect()
        {
            //Act 
            var result = await _controller.GetById(1);
            //Assert
            result.Should().BeOfType<OkNegotiatedContentResult<DefectDto>>();
        }

        [TestMethod]
        public async Task GetAllDefects_ShouldReturnTwoItems()
        {

            //Act 
            var result = await _controller.Get();
            //Assert
            result.Should().BeOfType<List<DefectDto>>();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetDefectsFiltered_ShouldReturnOneItem()
        {
            //Arrange
            query = "DI";
            _mockUnitOfWork.Setup(uw => uw.DefectRepository
                    .GetAsync(It.IsAny<Expression<Func<Defect, bool>>>()))
                    .Returns(Task.FromResult(defects));
            //Act 
            var result = await _controller.Get(query);
            //Assert
            result.Should().BeOfType<List<DefectDto>>();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task AddDefect_ShouldReturnOkResult()
        {
            //Arrange
            _mockUnitOfWork.Setup(uw => uw.DefectRepository.Add(It.IsAny<Defect>()));
            //Act
            var result = await _controller.Add(dto);
            //Assert
            _mockUnitOfWork.Verify(uw => uw.DefectRepository
                            .Add(It.Is<Defect>(qc => qc.Name == "DI230")));

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateDefect_ValidDefect_ShouldOkResult()
        {
            //Act
            var result =await _controller.Update(dto);
            //Assert
            _mockUnitOfWork.Verify(uw => uw.DefectRepository
                            .Update(It.Is<Defect>(qc => qc.Name == "DI230")));
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task UpdateDefect_UnexistingDefect_ShouldBadRequest()
        {
            //Arrange
            Defect defectNull=null;
            _mockUnitOfWork.Setup(uw => uw.DefectRepository.FindByIdAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(defectNull));
            //Act
            var result = await _controller.Update(dto);
            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
