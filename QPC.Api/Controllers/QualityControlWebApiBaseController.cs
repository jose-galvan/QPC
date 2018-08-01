using Microsoft.AspNet.Identity;
using QPC.Core.Models;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace QPC.Api.Controllers
{
    public class QualityControlWebApiBaseController : ApiController
    {
        internal IUnitOfWork _unitOfWork;
        internal QualityControlFactory _factory;
        public Func<Guid> GetUserId;

        internal QualityControlWebApiBaseController(IUnitOfWork unitOfWork, QualityControlFactory factory)
        {
            _factory = factory;
            _unitOfWork = unitOfWork;
            GetUserId = () => GetGuid(User.Identity.GetUserId());
        }

        protected virtual async Task<User> GetUserAsync()
        {
            return await _unitOfWork.UserRepository.FindByIdAsync(GetUserId());
        }

        protected virtual Guid GetGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }


        protected virtual async Task LogExceptionAsync(Exception exception, [CallerMemberName]string propertyName = null)
        {
            var log = new Log
            {
                Message = exception.Message,
                ClassName = this.GetType().Name,
                MethodName = propertyName,
                UserCreated = await GetUserAsync(),
                DateCreated = DateTime.Now
            };
            try
            {
                _unitOfWork.LogRepository.Add(log);
            }
            catch
            {
                return;
            }
        }
    }
}
