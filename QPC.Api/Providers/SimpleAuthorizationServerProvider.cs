using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Unity;
using Microsoft.AspNet.Identity;
using QPC.Api.Identity;

namespace QPC.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
        private UserManager<IdentityUser, Guid> _userManager;

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
	    {
	        context.Validated();
	    }
	
	    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
	    {
            //Retrives instance of UserManager from Unity Container.
            _userManager = UnityConfig.Container
                        .Resolve<UserManager<IdentityUser, Guid>>();
            
	        IdentityUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
	        {
	            context.SetError("invalid_grant", "The user name or password is incorrect.");
	            return;
	        }
	        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
	        identity.AddClaim(new Claim("sub", context.UserName));
	        identity.AddClaim(new Claim("role", "user"));
	
	        context.Validated(identity);
	    }
	}

}