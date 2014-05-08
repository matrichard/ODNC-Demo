using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Moq;

namespace DemoWebApi.Tests.Utils
{
    public static class WebApiTester
    {
        /// <summary>
        /// la requete pour la route.
        /// </summary>
        /// <param name="config">
        /// la config.
        /// </param>
        /// <param name="request">
        /// la requete.
        /// </param>
        /// <returns>
        /// la <see cref="RouteInfo"/>.
        /// </returns>
        public static RouteInfo RequeteRoute(HttpConfiguration config, HttpRequestMessage request)
        {
            var mock = new Mock<IHttpRouteData>();

            // creation du controller
            var controllerContext = new HttpControllerContext(config, mock.Object, request);

            var routeData = config.Routes.GetRouteData(request);
            if (routeData == null)
            {
                return null;
            }

            SupprimerParametreRoutageOptionnel(routeData.Values);

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerContext.RouteData = routeData;

            // get controller type
            var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
            controllerContext.ControllerDescriptor = controllerDescriptor;

            var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);

            return new RouteInfo
            {
                Controller = controllerDescriptor.ControllerType,
                Action = actionMapping.ActionName,
                Params = routeData.Values
            };
        }

        private static void SupprimerParametreRoutageOptionnel(IDictionary<string, object> valeursRoute)
        {
            var paramOptionnel = valeursRoute
                .Where(x => x.Value == RouteParameter.Optional)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in paramOptionnel)
            {
                valeursRoute.Remove(key);
            }
        }
    }
}
