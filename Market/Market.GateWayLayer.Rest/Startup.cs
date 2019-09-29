using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using Market.GateWayLayer.Rest.Common;
using Market.GateWayLayer.Rest.Models;
using Market.InfrastructureLayer.Models;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Market.GateWayLayer.Rest.Startup))]

namespace Market.GateWayLayer.Rest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var corsPolicy = new EnableCorsAttribute("*", "*", "*");
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request =>
                        request.Path.Value == "/token" ?
                            corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
                            Task.FromResult<CorsPolicy>(null)
                }
            });

            OAuthAuthorizationServerOptions option = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                AllowInsecureHttp = true
            };
            app.UseOAuthBearerTokens(option);
            app.UseOAuthAuthorizationServer(option);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserCredential uc = Repository.UserRepository.FindByUserNameAndPassword(context.UserName, context.Password);

            if (uc != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("Username", uc.UserName));
                identity.AddClaim(new Claim("Id", uc.User.Id.ToString()));
                identity.AddClaim(new Claim("Phone", uc.User.Phone));
                identity.AddClaim(new Claim("FirstName", uc.User.FirstName));
                identity.AddClaim(new Claim("LastName", uc.User.LastName));
                identity.AddClaim(new Claim("Address", uc.User.Address));
                identity.AddClaim(new Claim("PostalCode", uc.User.PostalCode));
                identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, uc.User.Role));
                var additionalData = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "role", uc.User.Role
                    }
                });
                var token = new AuthenticationTicket(identity, additionalData);
                context.Validated(token);
            }
            else
            {
                context.SetError("invalid_grant", "Username or password is not valid.");
                context.Rejected();
            }
                
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}