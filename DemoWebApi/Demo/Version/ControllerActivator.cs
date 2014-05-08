using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace DemoWebApi.Demo.Version
{
    public class ControllerActivator : IHttpControllerActivator
    {
        private readonly object cacheCle = new object();

        public IHttpController Create(
            HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            Func<IHttpController> activator;

            object value;
            if (controllerDescriptor.Properties.TryGetValue(cacheCle, out value))
            {
                activator = (Func<IHttpController>) value;
            }
            else
            {
                var instance = (IHttpController) request.GetDependencyScope().GetService(controllerType);
                if (instance != null)
                {
                    return instance;
                }

                NewExpression newInstanceExpression = Expression.New(controllerType);
                activator = Expression.Lambda<Func<IHttpController>>(newInstanceExpression).Compile();
                controllerDescriptor.Properties.TryAdd(cacheCle, activator);
            }

            return activator();
        }
    }
}