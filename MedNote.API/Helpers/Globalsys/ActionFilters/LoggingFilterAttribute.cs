using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using Globalsys.Extensoes;
using System.Net.Http;
using System.Net;
using MedNote.API.Helpers.Globalsys;

namespace VIX.API.Helpers.Globalsys.ActionFilters
{
    public class LoggingFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new GlobalsysLogger());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Info(filterContext.Request, "Controller : " + filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + filterContext.ActionDescriptor.ActionName, "JSON", filterContext.ActionArguments);
            base.OnActionExecuting(filterContext);
        }
        /*
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {

            
            var test = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

            Object aa = (Object)actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
            if (objectContent != null)
            {
                var type = objectContent.ObjectType; //type of the returned object
                var value = objectContent.Value; //holding the returned value
            }
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
            HttpStatusCode.OK, new SuccessResult() { Success = true , Data = new JsonResult { Data = actionExecutedContext.Response.Content } });
            base.OnActionExecuted(actionExecutedContext);
        }
        */
    }
}