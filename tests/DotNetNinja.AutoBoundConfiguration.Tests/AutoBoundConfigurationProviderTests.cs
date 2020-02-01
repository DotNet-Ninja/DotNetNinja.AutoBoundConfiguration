using DotNetNinja.AutoBoundConfiguration.TestAssembly;
using System;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBoundConfigurationProviderTests
    {
        [Fact]
        public void Add_ShouldAddInstanceToConfigurations()
        {
            var instance = new SampleSettings();
            var provider = new AutoBoundConfigurationProvider();

            provider.Add(typeof(SampleSettings), instance);

            Assert.True(provider.Configurations.ContainsKey(typeof(SampleSettings)), 
                "An instance of SampleSettings was added but was not found in the internal Configurations dictionary.");
            Assert.Same(instance, provider.Configurations[typeof(SampleSettings)]);
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
    }
}
