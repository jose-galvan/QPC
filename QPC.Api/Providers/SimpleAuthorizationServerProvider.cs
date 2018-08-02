using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Unity;
using Microsoft.AspNet.Identity;
using QPC.Api.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Web;

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
            AuthenticationManager = context.OwinContext.Authentication;

            _userManager = UnityConfig.Container
            .Resolve<UserManager<IdentityUser, Guid>>();

            IdentityUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            ClaimsIdentity oAuthIdentity = await SignInAsync(user, true);
            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(oAuthIdentity);
        }
        private async Task<ClaimsIdentity> SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
            return identity;
        }
        private IAuthenticationManager AuthenticationManager{get; set;}
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }

}