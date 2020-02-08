using DotNetNinja.AutoBoundConfiguration.TestAssembly;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBoundConfigurationProviderTests
    {
        private static readonly Dictionary<Type,object> TestRegistrations = new Dictionary<Type, object>
        {
            {typeof(SampleSettings), SampleSettings.Default },
            {typeof(OtherSettings), OtherSettings.Default }
        };

        [Fact]
        public void Instantiation_ShouldInitializeEmptyRegistrations()
        {
            var provider = new AutoBoundConfigurationProvider();

            Assert.NotNull(provider.Registrations);
            Assert.Empty(provider.Registrations);
        }

        [Fact]
        public void Instantiation_WithNullRegistrations_ShouldInitializeEmptyRegistrations()
        {
            var provider = new AutoBoundConfigurationProvider(null);

            Assert.NotNull(provider.Registrations);
            Assert.Empty(provider.Registrations);
        }

        [Fact]
        public void Instantiation_WithRegistrations_ShouldInitializeWithProvidedRegistrations()
        {

            var provider = new AutoBoundConfigurationProvider(TestRegistrations);

            Assert.NotNull(provider.Registrations);
            Assert.Same(TestRegistrations, provider.Registrations);
        }

        [Fact]
        public void Add_ShouldAddInstanceToConfigurations()
        {
            var instance = new SampleSettings();
            var provider = new AutoBoundConfigurationProvider();

            provider.Add(typeof(SampleSettings), instance);

            Assert.True(provider.Registrations.ContainsKey(typeof(SampleSettings)), 
                "An instance of SampleSettings was added but was not found in the internal Registrations dictionary.");
            Assert.Same(instance, provider.Registrations[typeof(SampleSettings)]);
        }

        [Fact]
        public void Add_WhenTypeAndInstanceTypeAreNotSame_ShouldThrowInvalidOperationExceptionWithExpectedMessage()
        {
            var instance = new SampleSettings();
            var provider = new AutoBoundConfigurationProvider();

            var thrown = Assert.Throws<InvalidOperationException>(()=> provider.Add(typeof(OtherSettings), instance));

            Assert.Equal("Cannot add instance of type DotNetNinja.AutoBoundConfiguration.TestAssembly.SampleSettings for type DotNetNinja.AutoBoundConfiguration.TestAssembly.OtherSettings.", 
                thrown.Message);
        }

        [Fact]
        public void Add_WhenTypeHasAlreadyBeenAdded_ShouldThrow()
        {
            var instance = new SampleSettings();
            var provider = new AutoBoundConfigurationProvider();

            provider.Add(typeof(SampleSettings), instance);

            var thrown = Assert.Throws<ArgumentException>(() => provider.Add(typeof(SampleSettings), instance));

            Assert.StartsWith("An item with the same key has already been added.", thrown.Message);
        }

        [Fact]
        public void Get_WhenTypeIsRegistered_ShouldReturnRegisteredSettingsInstance()
        {
            var sampleSettings = SampleSettings.Default;
            var otherSettings = OtherSettings.Default;
            var provider = new AutoBoundConfigurationProvider();
            provider.Registrations.Add(typeof(SampleSettings), sampleSettings);
            provider.Registrations.Add(typeof(OtherSettings), otherSettings);

            var settings = provider.Get<SampleSettings>();

            Assert.NotNull(settings);
            Assert.Same(sampleSettings, settings);
        }

        [Fact]
        public void Get_WhenTypeIsNotRegistered_ShouldReturnNull()
        {
            var provider = new AutoBoundConfigurationProvider();

            var settings = provider.Get<SampleSettings>();

            Assert.Null(settings);
        }

        [Fact]
        public void Contains_WhenTypeIsRegistered_ShouldReturnTrue()
        {
            var provider = new AutoBoundConfigurationProvider(TestRegistrations);

            var result = provider.Contains<SampleSettings>();

            Assert.True(result);
        }

        [Fact]
        public void Contains_WhenTypeIsNotRegistered_ShouldReturnFalse()
        {
            var provider = new AutoBoundConfigurationProvider();

            var result = provider.Contains<SampleSettings>();

            Assert.False(result);
        }

    }
}
