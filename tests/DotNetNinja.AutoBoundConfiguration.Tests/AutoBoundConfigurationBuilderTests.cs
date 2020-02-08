using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using DotNetNinja.AutoBoundConfiguration.TestAssembly;
using DotNetNinja.AutoBoundConfiguration.TestAssembly2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBoundConfigurationBuilderTests: IDisposable
    {
        public AutoBoundConfigurationBuilderTests()
        {
            _mockRepository = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            _mockRepository.Dispose();
        }
        
        private readonly AutoMock _mockRepository;
        
        [Fact]
        public void Instantiation_WithNullServices_ShouldThrowArgumentNullException()
        {
            var thrown = Assert.Throws<ArgumentNullException>(() => 
                    new AutoBoundConfigurationBuilder(null, _mockRepository.Mock<IConfiguration>().Object));

            Assert.Equal("Value cannot be null. (Parameter 'services')", thrown.Message);
        }

        [Fact]
        public void Instantiation_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            var thrown = Assert.Throws<ArgumentNullException>(() =>
                new AutoBoundConfigurationBuilder(_mockRepository.Mock<IServiceCollection>().Object, null));

            Assert.Equal("Value cannot be null. (Parameter 'configuration')", thrown.Message);
        }

        [Fact]
        public void Instantiation_ShouldInitializeProvider()
        {
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            Assert.NotNull(builder.Provider);
        }

        [Fact]
        public void Instantiation_ShouldInitializeServices()
        {
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            Assert.NotNull(builder.Services);
        }

        [Fact]
        public void For_WithTypeWithoutParameterlessConstructor_ShouldThrowInvalidOperationException()
        {
            var type = typeof(NoParameterlessConstructor);
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(()=> builder.For(type));

            Assert.Equal("Type 'NoParameterlessConstructor' cannot be used with AutoBoundConfiguration because it does not have a parameter-less constructor.",
                            thrown.Message);
        }

        [Fact]
        public void For_WithTypeWithoutAutoBindAttribute_ShouldThrowInvalidOperationException()
        {
            var type = typeof(NotASettingsClass);
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(() => builder.For(type));

            Assert.Equal("Type 'NotASettingsClass' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.",
                thrown.Message);
        }

        [Fact]
        public void For_ShouldAddTypeToProvider()
        {
            var type = typeof(SampleSettings);
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            Assert.True(builder.Provider.Contains<SampleSettings>(), "Expected type was not found in the registrations of the provider.");
        }

        [Fact]
        public void For_ShouldAddTypeToServicesWithSingletonLifetimeScope()
        {
            var type = typeof(SampleSettings);
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();
            

            builder.For(type);

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton && service.ServiceType == type);
        }

        [Fact]
        public void For_ShouldBindInstanceOfTypeToProvidedConfigurationData()
        {
            var type = typeof(SampleSettings);
            _mockRepository.Provide<IConfiguration>(TestConfigurationSource.Default);
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            var settings = builder.Provider.Get<SampleSettings>();
            Assert.Equal("String", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void For_WhenSectionNotSpecified_ShouldBindInstanceOfTypeToExpectedConfigurationData()
        {
            var type = typeof(AutoSampleSettings);
            _mockRepository.Provide<IConfiguration>(TestConfigurationSource.AutoSectionFinding);
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            var settings = builder.Provider.Get<AutoSampleSettings>();
            Assert.Equal("AutoString", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void ForT_WithTypeWithoutAutoBindAttribute_ShouldThrowInvalidOperationException()
        {
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(() => builder.For<NotASettingsClass>());

            Assert.Equal("Type 'NotASettingsClass' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.",
                thrown.Message);
        }

        [Fact]
        public void ForT_ShouldAddTypeToProvider()
        {
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<SampleSettings>();

            Assert.True(builder.Provider.Contains<SampleSettings>(), "Expected type was not found in the registrations of the provider.");
        }

        [Fact]
        public void ForT_ShouldAddTypeToServicesWithSingletonLifetimeScope()
        {
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();


            builder.For<SampleSettings>();

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton 
                                                         && service.ServiceType == typeof(SampleSettings));
        }

        [Fact]
        public void ForT_ShouldBindInstanceOfTypeToProvidedConfigurationData()
        {
            _mockRepository.Provide<IConfiguration>(TestConfigurationSource.Default);
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<SampleSettings>();

            var settings = builder.Provider.Get<SampleSettings>();
            Assert.Equal("String", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }
        
        [Fact]
        public void ForT_WhenSectionNotSpecified_ShouldBindInstanceOfTypeToExpectedConfigurationData()
        {
            _mockRepository.Provide<IConfiguration>(TestConfigurationSource.AutoSectionFinding);
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<AutoSampleSettings>();

            var settings = builder.Provider.Get<AutoSampleSettings>();
            Assert.Equal("AutoString", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void FromAssembly_ShouldAddAllTypesMarkedWithAutoBindToProvider()
        {
            var assembly = typeof(SampleSettings).Assembly;
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssembly(assembly);

            Assert.True(builder.Provider.Contains<SampleSettings>(), "Expected type 'SampleSettings' was not found in the registrations of the provider.");
            Assert.True(builder.Provider.Contains<OtherSettings>(), "Expected type 'OtherSettings' was not found in the registrations of the provider.");
        }

        [Fact]
        public void FromAssembly_ShouldAddAllTypesMarkedWithAutoBindToServicesWithSingletonLifetimeScope()
        {
            var assembly = typeof(SampleSettings).Assembly;
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssembly(assembly);
            
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(OtherSettings));
        }

        [Fact]
        public void FromAssemblies_ShouldAddAllTypesMarkedWithAutoBindToProvider()
        {
            var assembly1 = typeof(SampleSettings).Assembly;
            var assembly2 = typeof(AnotherSettings).Assembly;
            var assemblies = new []{assembly1, assembly2};
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssemblies(assemblies);

            Assert.True(builder.Provider.Contains<SampleSettings>(), 
                            "Expected type 'SampleSettings' was not found in the registrations of the provider.");
            Assert.True(builder.Provider.Contains<OtherSettings>(), 
                            "Expected type 'OtherSettings' was not found in the registrations of the provider.");
            Assert.True(builder.Provider.Contains<OtherSettings>(), 
                            "Expected type 'AnotherSettings' was not found in the registrations of the provider.");

        }

        [Fact]
        public void FromAssemblies_ShouldAddAllTypesMarkedWithAutoBindToServicesWithSingletonLifetimeScope()
        {
            var assembly1 = typeof(SampleSettings).Assembly;
            var assembly2 = typeof(AnotherSettings).Assembly;
            var assemblies = new[] { assembly1, assembly2 };
            _mockRepository.Provide<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));
            _mockRepository.Provide<IServiceCollection>(new ServiceCollection());
            var builder = _mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssemblies(assemblies);

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(OtherSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(AnotherSettings));
        }

    }
}