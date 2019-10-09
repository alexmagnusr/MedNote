using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys.Exceptions;
using Globalsys.Repositories;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net;
using Globalsys;
using System.Net.Http;
using System.Reflection;
using Globalsys.Extensoes;
using MedNote.Infra;

namespace MedNote.API.Helpers.Globalsys.ActionFilters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IUnidadeTrabalho unidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IRepositorioPermissao repPermissao = Fabrica.Instancia.ObterRepositorio<IRepositorioPermissao>(unidadeTrabalho);
            HttpActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            HttpControllerDescriptor controllerDescriptor = actionDescriptor.ControllerDescriptor;
            var queryString = filterContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            if (!filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() &&
                !filterContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                string actionName = String.Empty;
                if (actionDescriptor.ActionName.ToUpper().Equals("Get".ToUpper()) && (!queryString.IsEmpty() && queryString.Keys.Contains("id")))
                    actionName = String.Format("{0}/{1}", actionDescriptor.ActionName, "id");
                else
                    actionName = actionDescriptor.ActionName;
                if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    throw new CoreException("Acesso negado. Você não possui permissão para acessar a página selecionada.");
                if (!repPermissao.PossuiPermissao(controllerDescriptor.ControllerName, actionName))
                    throw new CoreException(String.Format("Acesso negado. Você não possui permissão para acessar a página  {0}/{1}.", controllerDescriptor.ControllerName, actionName));
            }
        }
    }
}
