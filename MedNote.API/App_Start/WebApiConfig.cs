using MedNote.API.Helpers.Globalsys.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VIX.API.Helpers.Globalsys.ActionFilters;

namespace AtiveEngenharia.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new LoggingFilterAttribute());
            config.Filters.Add(new GlobalExceptionAttribute());
        }
    }
}
