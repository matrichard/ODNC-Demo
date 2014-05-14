using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Validation.Providers;
using DemoWebApi.Demo;
using DemoWebApi.Demo.Ioc;
using DemoWebApi.Demo.Jsonp;
using DemoWebApi.Demo.MessageHandler;
using DemoWebApi.Demo.Version;
using Newtonsoft.Json.Serialization;

namespace DemoWebApi
{
    public static class WebApiConfig
    {
        public readonly static string DefaultApiRoute = "DefaultApi";

        public static void Register(HttpConfiguration config)
        {

            #region Routes

            config.Routes.MapHttpRoute(
                name: "SubControllerApi",
                routeTemplate: "api/{parentController}/{parentId}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            #region Action

            config.Routes.MapHttpRoute(
                name: "ActionRoute",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { action = RouteParameter.Optional }
            );

            #endregion

            #region Extension Route

            //config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
            //config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", "application/json");

            //config.Routes.MapHttpRoute(
            //    name: "ExtensionRoute",
            //    routeTemplate: "api/{controller}.{ext}/{id}/{action}",
            //    defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional, ext = RouteParameter.Optional }
            //);

            #endregion

            #region SubController Route

            

            #endregion

            #region Versionning Route

            #region Versionning

            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpControllerActivator), new ControllerActivator());

            #endregion

            config.Routes.MapHttpRoute(
                name: "Version",
                routeTemplate: "api/{version}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            #endregion

            #endregion

            #region MessageHandler

            config.MessageHandlers.Add(new MethodOverrideHandler());

            #endregion

            #region CamelCasing

            config
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();

            #endregion

            #region IoC

            // See global.asax

            #endregion

            #region ModelValidatorProvider

            //config
            //    .Services
            //    .RemoveAll(
            //        typeof(System.Web.Http.Validation.ModelValidatorProvider), 
            //        v => v is InvalidModelValidatorProvider);

            #endregion

            #region JSONP

            //config.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            #endregion
        }
    }
}
