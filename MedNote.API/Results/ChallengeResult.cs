using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedNote.API.Results
{
    public class ChallengeResult : System.Web.Http.IHttpActionResult
    {
        public string LoginProvider { get; set; }
        public System.Net.Http.HttpRequestMessage Request { get; set; }

        public ChallengeResult(string loginProvider, System.Web.Http.ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}