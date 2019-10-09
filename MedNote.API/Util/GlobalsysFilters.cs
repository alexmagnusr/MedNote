using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MedNote.API.Util
{
    public class GlobalsysFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request
                     .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);

                //BadRequest(actionContext.ModelState);
            }
        }
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {

            return Task.Factory.StartNew(() => {

                if (!actionContext.ModelState.IsValid)
                {
                    actionContext.Response = actionContext.Request
                         .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                }
            });

        }
    }
}