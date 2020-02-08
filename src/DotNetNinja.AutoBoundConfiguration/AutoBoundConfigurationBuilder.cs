using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetNinja.AutoBoundConfiguration
{
    public class AutoBoundConfigurationBuilder
    {
        public AutoBoundConfigurationBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _provider = new AutoBoundConfigurationProvider();
        }
        
        private readonly AutoBoundConfigurationProvider _provider;

        public IServiceCollection Services { get; }

        protected IConfiguration Configuration { get; }

        public IAutoBoundConfigurationProvider Provider => _provider;

        public AutoBoundConfigurationBuilder FromAssembly(Assembly assembly)
        {
            AddFromAssembly(assembly);
            return this;
        }

        private void AddFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type => type.GetCustomAttribute<AutoBindAttribute>() != null && type.IsPublic);
            foreach (var type in types)
            {
                For(type);
            }
        }

        public AutoBoundConfigurationBuilder FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach(var assembly in assemblies)
            {
                AddFromAssembly(assembly);
            }
            return this;
        }

        public AutoBoundConfigurationBuilder For<T>() where T: class, new()
        {
            return For(typeof(T)) ;
        }

        public AutoBoundConfigurationBuilder For(Type t)
        {
            if(t.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException($"Type '{t.Name}' cannot be used with AutoBoundConfiguration because it does not have a parameter-less constructor.");
            }
            var attribute = t.GetCustomAttribute<AutoBindAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException($"Type '{t.Name}' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.");
            }
            var instance = Convert.ChangeType(Activator.CreateInstance(t), t);
            Configuration.Bind(attribute.Section, instance);
            _provider.Add(t, instance);
            Services.AddSingleton(t, instance);
            return this;
        }
    }
}
