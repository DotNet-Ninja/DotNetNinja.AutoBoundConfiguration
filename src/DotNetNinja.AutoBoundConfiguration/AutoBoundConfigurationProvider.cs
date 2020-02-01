using System;
using System.Collections.Generic;

namespace DotNetNinja.AutoBoundConfiguration
{
    public class AutoBoundConfigurationProvider : IAutoBoundConfigurationProvider
    {
        private Dictionary<Type, object> _configurations = new Dictionary<Type, object>();

        public TConfiguration Get<TConfiguration>() where TConfiguration : class, new()
        {
            return _configurations[typeof(TConfiguration)] as TConfiguration;
        }

        internal void Add<TConfiguration>(Type type, TConfiguration instance) where TConfiguration: class, new()
        {
            if(type!=instance.GetType())
            {
                throw new InvalidOperationException($"Cannot add instance of type {instance.GetType().FullName} for type {type.FullName}.");
            }
            _configurations.Add(type, instance);
        }
    }
}
