using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QPC.Core.Repositories;
using Moq;
using QPC.Web.Controllers;
using QPC.Web.Helpers;
using QPC.Web.Tests.Extensions;
using System.Collections.Generic;
using QPC.Core.Models;
using QPC.Core.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Linq;

namespace QPC.Web.Tests.Controllers.MVC
{
    [TestClass]
    public class DefectsControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDefectRepository> _mockRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ILogRepository> _mockLogRepository;

        private DefectsController _controller;
        private Mock<QualityControlFactory> _mockFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IDefectRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogRepository = new Mock<ILogRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockFactory = new Mock<QualityControlFactory>();

            _mockUnitOfWork.SetupGet(uw => uw.DefectRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.LogRepository).Returns(_mockLogRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.ProductRepository).Returns(_mockProductRepository.Object);

            _controller = new DefectsController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => ControllerExtensions.GetGuid("1571");
        }

        [TestMethod]
        public async Task GetAllDefects_ShouldReturnTwoItems()
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

            var defects = new List<Defect>()
            {
                new Defect {
                    Id = 1,
                    Name = "DI321",
                    Description = "x35 Blade F12",
                    Product = products[0]
                },
                new Defect {
                    Id = 2,
                    Name = "CO410",
                    Description = "x35 Blade F10",
                    Product = products[1]
                }
            };
            
            _mockRepository.Setup(r => r.GetWithProductAsync())
                                .Returns(Task.FromResult(defects));

            _mockProductRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

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
            string query = "321";
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

            var defects = new List<Defect>()
            {
                new Defect {
                    Id = 1,
                    Name = "DI321",
                    Description = "x35 Fan F12",
                    Product = products[0]
                },
                new Defect {
                    Id = 2,
                    Name = "CO410",
                    Description = "x35 Blade F10",
                    Product = products[1]
                }
            };

            _mockRepository.Setup(r => r.GetWithProductAsync())
                                .Returns(Task.FromResult(defects));

            _mockProductRepository.Setup(r => r.GetAllAsync())
                    .Returns(Task.FromResult(products));
            //Act 
            var result = await _controller.Defect(query) as ViewResult;
            var model = result.Model as DefectViewModel;

            //Assert
            Assert.AreEqual(1, model.Defects.Count());
        }


        [TestMethod]
        public async Task AddDefect_ValidDefect()
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
                    Id =1,
                    Name = "Blade",
                    Description = "x35 Blade F12",
                },
                new Product {
                    Id = 2,
                    Name = "Blade F1",
                    Description = "x35 Blade F10",
                }
            };

            var defects = new List<Defect>()
            {
                new Defect {
                    Id = 1,
                    Name = "DI321",
                    Description = "x35 Fan F12",
                    Product = products[0]
                },
                new Defect {
                    Id = 2,
                    Name = "CO410",
                    Description = "x35 Blade F10",
                    Product = products[1]
                }
            };

            var viewModel = new DefectViewModel()
            {
                Name = "DIM442",
                Description = "Dimension 442, 0 +-0.5",
                Product = 1

            };
            _mockProductRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

            _mockRepository.Setup(r => r.GetWithProductAsync())
                                .Returns(Task.FromResult(defects));
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

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
            var user = new User
            {
                UserId = ControllerExtensions.GetGuid("1571"),
                UserName = "user@mail.com"
            };
            var products = new List<Product>()
            {
                new Product {
                    Id =1,
                    Name = "Blade",
                    Description = "x35 Blade F12",
                },
                new Product {
                    Id = 2,
                    Name = "Blade F1",
                    Description = "x35 Blade F10",
                }
            };
            var defect = new Defect
            {
                Id = 2,
                Name = "CO410",
                Description = "x35 Blade F10",
                Product = products[1]
            };
            var defects = new List<Defect>(){ defect };


            var viewModel = new DefectViewModel()
            {
                Id = 2,
                Name = "DIM442",
                Description = "Dimension 442, 0 +-0.5",
                Product = 1

            };
            _mockProductRepository.Setup(r => r.GetAllAsync())
                                .Returns(Task.FromResult(products));

            _mockRepository.Setup(r => r.GetWithProductAsync())
                                .Returns(Task.FromResult(defects));
            _mockRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(defect));

            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act 
            var result = await _controller.SaveDefect(viewModel) as ViewResult;
            var model = result.Model as DefectViewModel;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.Defects.Count());
        }

    }
}
