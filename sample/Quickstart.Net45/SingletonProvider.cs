using System;
using System.Collections.Generic;

namespace Quickstart.Net45
{
    public class SingletonContainer : IServiceProvider
    {
        private readonly IDictionary<Type, object> _objects = new Dictionary<Type, object>();

        public void Add<T>(object o)
        {
            _objects.Add(typeof(T), o);
        }

        public object GetService(Type serviceType)
        {
            return _objects[serviceType];
        }
    }
}