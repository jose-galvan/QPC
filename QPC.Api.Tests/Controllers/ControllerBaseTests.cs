using Moq;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System;
using System.Threading.Tasks;

namespace QPC.Api.Tests.Controllers
{
    //Contains the common mocks for all unit test classes ¿
    public abstract class ControllerBaseTests
    {
        public User _user;
        internal Mock<IUnitOfWork> _mockUnitOfWork;
        internal Mock<IUserRepository> _mockUserRepository;
        internal Mock<ILogRepository> _mockLogRepository;
        internal Mock<QualityControlFactory> _mockFactory;

        internal ControllerBaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogRepository = new Mock<ILogRepository>();
            _mockFactory = new Mock<QualityControlFactory>();
            _mockUnitOfWork.SetupGet(uw => uw.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.SetupGet(uw => uw.LogRepository).Returns(_mockLogRepository.Object);


            _user = new User { UserId = GetGuid("1571"), UserName = "user@mail.com" };
            _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_user));
            InitializeMockData();
        }

        //Should contain all the data that is used to tests in inherited unit tests classes
        protected abstract void InitializeMockData();

        protected Guid GetGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }
    }
}
