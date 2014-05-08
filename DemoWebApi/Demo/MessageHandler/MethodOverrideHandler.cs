using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace DemoWebApi.Demo.MessageHandler
{
    public class MethodOverrideHandler : DelegatingHandler
    {
        private const string Key = "X-HTTP-Method-Override";

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            //do something before
            if (request.Method == HttpMethod.Post && request.Headers.Contains(Key))
            {
                var method = request.Headers.GetValues(Key).FirstOrDefault();
                request.Method = new HttpMethod(method);
            }

            var  response = base.SendAsync(request, cancellationToken);

            //do something after

            return response;
        }
    }

    public class SomeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}