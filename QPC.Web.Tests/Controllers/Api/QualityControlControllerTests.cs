using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Repositories;
using QPC.Web.Controllers.Api;
using QPC.Web.Helpers;
using QPC.Web.Tests.Extensions;

namespace QPC.Web.Tests.Controllers.Api
{
    [TestClass]
    public class QualityControlControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IQualityControlRepository> _mockRepository;
        private QualityControlController _controller;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<QualityControlFactory> _mockFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IQualityControlRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockFactory = new Mock<QualityControlFactory>();

            _mockUnitOfWork.SetupGet(uw => uw.QualityControlRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _controller = new QualityControlController(_mockUnitOfWork.Object, _mockFactory.Object);
            _controller.GetUserId = () => ControllerExtensions.GetGuid("1571");

        }
    }
}
