using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace DemoWebApi.Demo.Ioc
{
    public class UnityDependencyScope : IDependencyScope
    {
        public UnityDependencyScope(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        protected IUnityContainer Container { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IHttpController).IsAssignableFrom(serviceType))
            {
                return this.Container.Resolve(serviceType, new ResolverOverride[0]);
            }

            return null;

        }
        
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.ResolveAll(serviceType, new ResolverOverride[0]);
        }
        
        protected virtual void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                if (this.Container != null)
                {
                    this.Container.Dispose();
                    this.Container = null;
                }
            }
        }
    }
}