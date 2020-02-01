using System;
using System.Collections.Generic;

namespace DotNetNinja.AutoBoundConfiguration
{
    internal class AutoBoundConfigurationProvider : IAutoBoundConfigurationProvider
    {
        internal Dictionary<Type, object> Configurations { get; } = new Dictionary<Type, object>();

        public TConfiguration Get<TConfiguration>() where TConfiguration : class, new()
        {
            if (!Configurations.ContainsKey(typeof(TConfiguration)))
            {
                return null;
            }
            return Configurations[typeof(TConfiguration)] as TConfiguration;
        }

        // TODO: Handle case when type is added more than once more elegantly.  Potentially check if Section is same and ignore if it is,
        // Otherwise throw exception.  Currently throws ArgumentException with message "An item with the same key has already been added. ..."
        internal void Add<TConfiguration>(Type type, TConfiguration instance) where TConfiguration: class, new()
        {
            if(type!=instance.GetType())
            {
                throw new InvalidOperationException($"Cannot add instance of type {instance.GetType().FullName} for type {type.FullName}.");
            }
            Configurations.Add(type, instance);
        }
    }
}
