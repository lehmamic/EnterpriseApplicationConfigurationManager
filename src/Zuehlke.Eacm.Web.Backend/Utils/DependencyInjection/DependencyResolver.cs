using System;
using System.Collections.Generic;
using CQRSlite.Config;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Zuehlke.Eacm.Web.Backend.Utils.DependencyInjection
{
    public class DependencyResolver : IServiceLocator
    {
        private readonly IServiceProvider serviceProvider;

        public DependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider.ArgumentNotNull(nameof(serviceProvider));
        }

        public T GetService<T>()
        {
            return (T) this.GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                return this.serviceProvider.GetService(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }
    }
}
