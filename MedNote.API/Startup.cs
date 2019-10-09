using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using MedNote.API.Providers;
using System.Collections.Generic;
using MedNote.API.Entities;
using MedNote.API.Util;
using System.Web.Http;
using System.Linq;
using AtiveEngenharia.API;

[assembly: OwinStartup(typeof(Startup))]

namespace AtiveEngenharia.API
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
      
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
          
          
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
       
            InitializeData();
        }
        private static List<Client> BuildClientsList()
        {

            List<Client> ClientsList = new List<Client>
            {
                new Client
                { Id = "ngAuthApp",
                    Secret= Helper.GetHash("abc@123"),
                    Name="Globalsys Soluções em TI - Front-end Application",
                    ApplicationType =  ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "*" //COLOCAR O DOMINIO DA APLICAÇÂO
                },
                new Client
                { Id = "consoleApp",
                    Secret=Helper.GetHash("123@abc"),
                    Name="Globalsys Soluções em TI - Aplicativo",
                    ApplicationType = ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
        private void InitializeData()
        {
            using (AuthContext context = new AuthContext())
            {
                if (context.Clients.Count() > 0)
                {
                    return;
                }

                context.Clients.AddRange(BuildClientsList());
                context.SaveChanges();

            }
        }
    }

}
