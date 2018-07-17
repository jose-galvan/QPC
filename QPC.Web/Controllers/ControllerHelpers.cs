using QPC.Core.Models;
using QPC.Core.Repositories;
using System;
using System.Runtime.CompilerServices;

namespace QPC.Web.Controllers
{
    public abstract class ControllerHelpers
    {
        private IUnitOfWork _unitOfWork;

        public ControllerHelpers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected virtual void LogException(Exception exception, User user, [CallerMemberName]string propertyName = null)
        {
            var log = new Log
            {
                Message = exception.Message,
                ClassName = this.GetType().Name,
                MethodName = propertyName,
                UserCreated = user,
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

        public static Guid GetGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }
    }
}