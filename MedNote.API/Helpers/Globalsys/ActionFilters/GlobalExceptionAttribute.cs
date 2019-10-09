using MedNote.API.Helpers.Globalsys;
using MedNote.API.Helpers.Globalsys.ErrorHelper;
using MedNote.API.Util;
using Globalsys.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace MedNote.API.Helpers.Globalsys.ActionFilters
{
    public class GlobalExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new GlobalsysLogger());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Error(context.Request, "Controller : " + context.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + context.ActionContext.ActionDescriptor.ActionName, context.Exception);
            var stackTrace = Helper.StackTraceFormat(new System.Diagnostics.StackTrace(context.Exception, true), "");
            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(ValidationException))
            {
                //var resp = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(context.Exception.Message), ReasonPhrase = "ValidationException", };
                //throw new HttpResponseException(resp);
                context.Response = context.ActionContext.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = context.Exception.Message, Success = false });

            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.Unauthorized, new ServiceStatus() { StatusCode = (int)HttpStatusCode.Unauthorized, StatusMessage = "UnAuthorized", ReasonPhrase = "UnAuthorized Access" }));
            }
            else if (exceptionType == typeof(ApiException))
            {
                var webapiException = context.Exception as ApiException;
                if (webapiException != null)
                    throw new HttpResponseException(context.Request.CreateResponse(webapiException.HttpStatus, new ServiceStatus() { StatusCode = webapiException.ErrorCode, StatusMessage = webapiException.ErrorDescription, ReasonPhrase = webapiException.ReasonPhrase }));
            }
            else if (exceptionType == typeof(ApiBusinessException))
            {
                var businessException = context.Exception as ApiBusinessException;
                if (businessException != null)
                    throw new HttpResponseException(context.Request.CreateResponse(businessException.HttpStatus, new ServiceStatus() { StatusCode = businessException.ErrorCode, StatusMessage = businessException.ErrorDescription, ReasonPhrase = businessException.ReasonPhrase }));
            }
            else if (exceptionType == typeof(ApiDataException))
            {
                var dataException = context.Exception as ApiDataException;
                if (dataException != null)
                    throw new HttpResponseException(context.Request.CreateResponse(dataException.HttpStatus, new ServiceStatus() { StatusCode = dataException.ErrorCode, StatusMessage = dataException.ErrorDescription, ReasonPhrase = dataException.ReasonPhrase }));
            }
            else if (exceptionType == typeof(CoreException))
            {
                var dataException = context.Exception as CoreException;
                if (dataException != null)
                {
                   // throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = dataException.Message, Success = false }));
                    context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = dataException.Message, Success = false });
                }
            }
            else
            {
                //context.Response = context.ActionContext.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = "", Success = true });
                throw new HttpResponseException(context.ActionContext.Request.CreateResponse(HttpStatusCode.OK, new SuccessResult() { Message = "Houve um erro no servidor. Contate o administrador ou tente novamente mais tarde.", Success = false }));
            }
        }
    }
}