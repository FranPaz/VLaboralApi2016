using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using VlaboralApi.Infrastructure;

namespace VlaboralApi.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        //funcion para validar el cliente, en este caso siempre sera un cliente valido yq que el unico cliente sea el frontend de la app
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
        
        
        //funcion que recibe y valida el nombre y contraseña del usuario y lo valida contra la bd
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed) //tambien valida que el usuario haya recibido y confirmado el email de confirmacion 
            {
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            //agrego claims adicionales para tener ademas del token, el nombre de usuario y el rol
            // los claim son atributos nombre-valor que dan info sobre el usuario que quiere conectarse
            //oAuthIdentity.AddClaim(new Claim("EmpleadorId", user.EmpleadorId.ToString())); 

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket); //SE GENERA EL TICKET DE ACCESO 

        }
    }
}