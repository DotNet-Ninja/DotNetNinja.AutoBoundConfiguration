using System;
using System.Collections.Generic;

namespace DotNetNinja.AutoBoundConfiguration
{
    internal class AutoBoundConfigurationProvider : IAutoBoundConfigurationProvider
    {
        internal AutoBoundConfigurationProvider()
        {
            Registrations = new Dictionary<Type, object>();
        }

        internal AutoBoundConfigurationProvider(Dictionary<Type, object> registrations)
        {
            Registrations = registrations??new Dictionary<Type, object>();
        }

        internal Dictionary<Type, object> Registrations { get; }

        public TConfiguration Get<TConfiguration>() where TConfiguration : class, new()
        {
            if (!Registrations.ContainsKey(typeof(TConfiguration)))
            {
                return null;
            }
            return Registrations[typeof(TConfiguration)] as TConfiguration;
        }

        public bool Contains<TConfiguration>() where TConfiguration : class, new()
        {
            var key = typeof(TConfiguration);
            return Registrations.ContainsKey(key);
        }

        // TODO: Handle case when type is added more than once more elegantly.  Potentially check if Section is same and ignore if it is,
        // TODO: Otherwise throw exception.  Currently throws ArgumentException with message "An item with the same key has already been added. ..."
        public void Add<TConfiguration>(Type type, TConfiguration instance) where TConfiguration: class, new()
        {
            if(type!=instance.GetType())
            {
                throw new InvalidOperationException($"Cannot add instance of type {instance.GetType().FullName} for type {type.FullName}.");
            }
            Registrations.Add(type, instance);
        }
    }
}
