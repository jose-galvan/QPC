using System.Web.Http;
using QPC.Api.Models;
using System.Threading.Tasks;
using QPC.Api.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Collections.Generic;

namespace QPC.Web.Controllers.Api
{
    [RoutePrefix("api/Account")]
	public class AccountController : ApiController
	{
        private readonly UserManager<IdentityUser, Guid> _userManager;

        public AccountController(UserManager<IdentityUser, Guid> userManager)
        {
            _userManager = userManager;
        }

        // POST api/Account/Register
        [AllowAnonymous]
	    [Route("Register")]
	    public async Task<IHttpActionResult> Register([FromBody]RegisterViewModel userModel)
	    {
	        if (!ModelState.IsValid)
	            return BadRequest(ModelState);
            //Register User
            var user = new IdentityUser() { UserName = userModel.Email };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            IHttpActionResult errorResult = GetErrorResult(result);
	        if (errorResult != null)
	            return errorResult;

	        return Ok();
	    }

        [HttpGet]
        [Route("Username")]
        public IHttpActionResult GetUserClaims()
        {
            return Ok(User.Identity.Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
	    {
	        if (result == null)
	        {
	            return InternalServerError();
	        }

	        if (!result.Succeeded)
	        {
	            if (result.Errors != null)
	            {
	                foreach (string error in result.Errors)
	                {
	                    ModelState.AddModelError("", error);
	                }
	            }

	            if (ModelState.IsValid)
	            {
	                // No ModelState errors are available to send, so just return an empty BadRequest.
	                return BadRequest();
	            }

	            return BadRequest(ModelState);
	        }

	        return null;
	    }
	}
}
