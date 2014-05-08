using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DemoWebApi.Data;
using DemoWebApi.Demo.Ioc;
using Microsoft.Practices.Unity;

namespace DemoWebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new DbInitializer());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region IoC

            //var container = new UnityContainer();

            //container.RegisterType<IBoardRepository, BoardRepository>();
            //container.RegisterType<ICardRepository, CardRepository>();

            //GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            #endregion
        }
    }
}