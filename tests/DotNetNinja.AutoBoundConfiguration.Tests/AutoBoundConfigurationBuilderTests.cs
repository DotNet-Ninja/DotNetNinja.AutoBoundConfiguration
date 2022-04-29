using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using DotNetNinja.AutoBoundConfiguration.TestAssembly;
using DotNetNinja.AutoBoundConfiguration.TestAssembly2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBoundConfigurationBuilderTests
    {
        [Fact]
        public void Instantiation_WithNullServices_ShouldThrowArgumentNullException()
        {
            using var mockRepository = AutoMock.GetLoose();
            var thrown = Assert.Throws<ArgumentNullException>(() =>
                new AutoBoundConfigurationBuilder(null, mockRepository.Mock<IConfiguration>().Object));

            Assert.Equal("Value cannot be null. (Parameter 'services')", thrown.Message);
        }

        [Fact]
        public void Instantiation_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            using var mockRepository = AutoMock.GetLoose();
            var thrown = Assert.Throws<ArgumentNullException>(() =>
                new AutoBoundConfigurationBuilder(mockRepository.Mock<IServiceCollection>().Object, null));

            Assert.Equal("Value cannot be null. (Parameter 'configuration')", thrown.Message);
        }

        [Fact]
        public void Instantiation_ShouldInitializeProvider()
        {
            using var mockRepository = AutoMock.GetLoose();
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            Assert.NotNull(builder.Provider);
        }

        [Fact]
        public void Instantiation_ShouldInitializeServices()
        {
            using var mockRepository = AutoMock.GetLoose();
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            Assert.NotNull(builder.Services);
        }

        [Fact]
        public void For_WithTypeWithoutParameterlessConstructor_ShouldThrowInvalidOperationException()
        {
            using var mockRepository = AutoMock.GetLoose();
            var type = typeof(NoParameterlessConstructor);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(() => builder.For(type));

            Assert.Equal(
                "Type 'NoParameterlessConstructor' cannot be used with AutoBoundConfiguration because it does not have a parameter-less constructor.",
                thrown.Message);
        }

        [Fact]
        public void For_WithTypeWithoutAutoBindAttribute_ShouldThrowInvalidOperationException()
        {
            using var mockRepository = AutoMock.GetLoose();
            var type = typeof(NotASettingsClass);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(() => builder.For(type));

            Assert.Equal(
                "Type 'NotASettingsClass' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.",
                thrown.Message);
        }

        [Fact]
        public void For_ShouldAddTypeToProvider()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces());
            var type = typeof(SampleSettings);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            Assert.True(builder.Provider.Contains<SampleSettings>(),
                "Expected type was not found in the registrations of the provider.");
        }

        [Fact]
        public void For_ShouldAddTypeToServicesWithSingletonLifetimeScope()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var type = typeof(SampleSettings);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();


            builder.For(type);

            Assert.Contains(builder.Services,
                service => service.Lifetime == ServiceLifetime.Singleton && service.ServiceType == type);
        }

        [Fact]
        public void For_ShouldBindInstanceOfTypeToProvidedConfigurationData()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(TestConfigurationSource.Default).AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var type = typeof(SampleSettings);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            var settings = builder.Provider.Get<SampleSettings>();
            Assert.Equal("String", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void For_WhenSectionNotSpecified_ShouldBindInstanceOfTypeToExpectedConfigurationData()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(TestConfigurationSource.AutoSectionFinding).AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var type = typeof(AutoSampleSettings);
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For(type);

            var settings = builder.Provider.Get<AutoSampleSettings>();
            Assert.Equal("AutoString", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void ForT_WithTypeWithoutAutoBindAttribute_ShouldThrowInvalidOperationException()
        {
            using var mockRepository = AutoMock.GetLoose();
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            var thrown = Assert.Throws<InvalidOperationException>(() => builder.For<NotASettingsClass>());

            Assert.Equal(
                "Type 'NotASettingsClass' cannot be used with AutoBoundConfiguration because it does not have a AutoBind attribute.",
                thrown.Message);
        }

        [Fact]
        public void ForT_ShouldAddTypeToProvider()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces());
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<SampleSettings>();

            Assert.True(builder.Provider.Contains<SampleSettings>(),
                "Expected type was not found in the registrations of the provider.");
        }

        [Fact]
        public void ForT_ShouldAddTypeToServicesWithSingletonLifetimeScope()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();


            builder.For<SampleSettings>();

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
        }

        [Fact]
        public void ForT_ShouldBindInstanceOfTypeToProvidedConfigurationData()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(TestConfigurationSource.Default).AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<SampleSettings>();

            var settings = builder.Provider.Get<SampleSettings>();
            Assert.Equal("String", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void ForT_WhenSectionNotSpecified_ShouldBindInstanceOfTypeToExpectedConfigurationData()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(TestConfigurationSource.AutoSectionFinding).AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.For<AutoSampleSettings>();

            var settings = builder.Provider.Get<AutoSampleSettings>();
            Assert.Equal("AutoString", settings.StringSetting);
            Assert.Equal(1234, settings.IntegerSetting);
        }

        [Fact]
        public void FromAssembly_ShouldAddAllTypesMarkedWithAutoBindToProvider()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces());
            var assembly = typeof(SampleSettings).Assembly;
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssembly(assembly);

            Assert.True(builder.Provider.Contains<SampleSettings>(),
                "Expected type 'SampleSettings' was not found in the registrations of the provider.");
            Assert.True(builder.Provider.Contains<OtherSettings>(),
                "Expected type 'OtherSettings' was not found in the registrations of the provider.");
        }

        [Fact]
        public void FromAssembly_ShouldAddAllTypesMarkedWithAutoBindToServicesWithSingletonLifetimeScope()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var assembly = typeof(SampleSettings).Assembly;
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssembly(assembly);

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(OtherSettings));
        }

        [Fact]
        public void FromAssemblies_ShouldAddAllTypesMarkedWithAutoBindToProvider()
        {
            using var mockRepository = AutoMock.GetLoose(c =>
                c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                    .AsImplementedInterfaces());
            var assembly1 = typeof(SampleSettings).Assembly;
            var assembly2 = typeof(AnotherSettings).Assembly;
            var assemblies = new[]
            {
                assembly1,
                assembly2
            };
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

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
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var assembly1 = typeof(SampleSettings).Assembly;
            var assembly2 = typeof(AnotherSettings).Assembly;
            var assemblies = new[]
            {
                assembly1,
                assembly2
            };
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssemblies(assemblies);

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(OtherSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(AnotherSettings));
        }

        [Fact]
        public void FromAssemblyOf_ShouldAddAllTypesMarkedWithAutoBindToProvider()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces());
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssemblyOf<SampleSettings>();

            Assert.True(builder.Provider.Contains<SampleSettings>(),
                "Expected type 'SampleSettings' was not found in the registrations of the provider.");
            Assert.True(builder.Provider.Contains<OtherSettings>(),
                "Expected type 'OtherSettings' was not found in the registrations of the provider.");
        }

        [Fact]
        public void FromAssemblyOf_ShouldAddAllTypesMarkedWithAutoBindToServicesWithSingletonLifetimeScope()
        {
            using var mockRepository = AutoMock
                .GetLoose(c =>
                {
                    c.RegisterInstance(new ConfigurationRoot(new List<IConfigurationProvider>()))
                        .AsImplementedInterfaces();
                    c.RegisterInstance(new ServiceCollection()).AsImplementedInterfaces();
                });
            var builder = mockRepository.Create<AutoBoundConfigurationBuilder>();

            builder.FromAssemblyOf<SampleSettings>();

            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(SampleSettings));
            Assert.Contains(builder.Services, service => service.Lifetime == ServiceLifetime.Singleton
                                                         && service.ServiceType == typeof(OtherSettings));
        }
    }
}