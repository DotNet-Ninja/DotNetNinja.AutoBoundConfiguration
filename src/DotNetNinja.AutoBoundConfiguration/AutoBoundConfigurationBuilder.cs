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

        private AutoBoundConfigurationProvider _provider;

        public IServiceCollection Services { get; }

        protected IConfiguration Configuration { get; }

        public IAutoBoundConfigurationProvider Provider
        {
            get
            {
                return _provider;
            }
        }

        public AutoBoundConfigurationBuilder GetProvider(out IAutoBoundConfigurationProvider provider)
        {
            provider = Provider;
            return this;
        }

        public AutoBoundConfigurationBuilder GetConfiguration<TConfiguration>(out TConfiguration configuration) 
            where TConfiguration: class, new()
        {
            configuration = Provider.Get<TConfiguration>();
            return this;
        }

        public AutoBoundConfigurationBuilder FromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type => type.GetCustomAttribute<AutoBindAttribute>() != null && type.IsPublic);
            foreach(var type in types)
            {
                For(type);
            }
            return this;
        }

        public AutoBoundConfigurationBuilder FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach(var assembly in assemblies)
            {
                FromAssembly(assembly);
            }
            return this;
        }

        public AutoBoundConfigurationBuilder FromAssemblies(params Assembly[] assemblies)
        {
            return FromAssemblies(assemblies);
        }

        public AutoBoundConfigurationBuilder For<T>() where T: class, new()
        {
            return For(typeof(T)) ;
        }

        public AutoBoundConfigurationBuilder For(Type t)
        {
            if(t.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException($"Type '{t.Name}' cannot be used with AutoBoundConfiguration because it does not have a parameterless constructor. ");
            }
            var attribute = t.GetCustomAttribute<AutoBindAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException($"Type '{t.Name}' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.");
            }
            var instance = Activator.CreateInstance(t);
            Configuration.Bind(attribute.Section, instance);
            _provider.Add(t, instance);
            Services.AddSingleton(t, instance);
            return this;
        }
    }
}
