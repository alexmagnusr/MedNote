using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Globalsys.Exceptions;
using Globalsys.Extensoes;
using System.Security.Claims;
using Microsoft.Owin.Security;
using MedNote.API.Util;
using MedNote.API.Entities;
using AtiveEngenharia.API;

namespace MedNote.API.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            bool activeDirectory = false;
            bool celular = false;
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            activeDirectory = context.Parameters["activeDirectory"].IsEmpty() ? false : bool.Parse(context.Parameters["activeDirectory"]);
            celular = context.Parameters["celular"].IsEmpty() ? false : bool.Parse(context.Parameters["celular"]);

            context.OwinContext.Set<bool>("activeDirectory", activeDirectory);
            context.OwinContext.Set<string>("IMEI", context.Parameters["IMEI"]);
            context.OwinContext.Set<bool>("celular", celular);

            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                context.SetError("invalid_clientId", "ClientId deve ser enviado.");
                return Task.FromResult<object>(null);
            }

            using (AuthRepository _repo = new AuthRepository())
            {
                client = _repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Cliente '{0}' Não está registrado no sistema.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "O segredo do cliente deve ser enviado.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "O segredo do cliente é inválido.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "O cliente está inativo.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            bool activeDirectory = context.OwinContext.Get<bool>("activeDirectory");
            string IMEI = context.OwinContext.Get<string>("IMEI");
            bool celular = context.OwinContext.Get<bool>("celular");

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            try
            {

                var teste = Models.Seguranca.UsuarioModel.Instancia.UsuariPossuiLogin(context.UserName);
            }
            catch (Exception ex)
            {
                
            }
            using (AuthRepository _repo = new AuthRepository())
            {
                
                var user = Models.Seguranca.UsuarioModel.Instancia.UsuariPossuiLogin(context.UserName);
                if (user == null || Models.Seguranca.UsuarioModel.Instancia.UsuariPossuiLogin(context.UserName).IsEmpty())
                {
                    context.SetError("invalid_grant", "O login não está cadastrado no sistema.");
                    return;
                }
                

                //if (user.LoginAd)
                //{
                //    if (!Globalsys.Util.Tools.ValidarLoginActiveDirectory(context.UserName, context.Password))
                //    {
                //        context.SetError("invalid_grant", "Login ou senha inválido no Active Directory ou o usúario não está cadastrado no sistema.");
                //        return;
                //    }
                //}
                //else
                //{
                    //if (!context.UserName.IsValidCPF())
                    //{
                    //    context.SetError("invalid_grant", "O número de CPF está incorreto.");
                    //    return;
                    //}
                    //else
                    //{
                        var userLogin = Models.Seguranca.UsuarioModel.Instancia.Login(context.UserName, context.Password);
                        if (userLogin == null)
                        {
                            context.SetError("invalid_grant", "O login ou senha estão incorretos.");
                            return;
                        }
                    //}
                //}
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaims(Models.Seguranca.UsuarioModel.Instancia.ObterPermissoesUsuario(context.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token é emitido para um clientId diferente.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
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