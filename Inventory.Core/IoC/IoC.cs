using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Core.IoC
{
    public sealed class IoC : IDisposable
    {
        private static IoC ioc;

        private readonly ServiceProvider serviceCollection;

        public static void IoCInitialize(ServiceProvider serviceCollection)
        {
            if (ioc != null)
                throw new Exception("Cant create a new IoC");

            ioc = new IoC(serviceCollection);
        }

        private IoC(ServiceProvider serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            ioc = this;
        }

        public static T Get<T>()
        {
            return ioc.serviceCollection.GetService<T>();
        }

        public void Dispose()
        {
            serviceCollection?.Dispose();
        }
    }
}
