using System;
using Autofac.Extras.Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class IServiceCollectionExtensionsTests: IDisposable
    {
        public IServiceCollectionExtensionsTests()
        {
            _mocks = AutoMock.GetLoose();
            _configuration = _mocks.Mock<IConfiguration>().Object;
        }

        public void Dispose()
        {
            _mocks?.Dispose();
        }

        private readonly AutoMock _mocks = null;
        private readonly IConfiguration _configuration;

        [Fact]
        public void AddAutoBoundConfigurations_ShouldReturnNonNullAutoBoundConfigurationBuilder()
        {   
            var services = new ServiceCollection();

            var result = services.AddAutoBoundConfigurations(_configuration);

            Assert.NotNull(result);
            Assert.IsType<AutoBoundConfigurationBuilder>(result);
        }

        [Fact]
        public void AddAutoBoundConfigurations_ReturnedBuilder_ShouldUseProvidedServices()
        {
            var services = new ServiceCollection();

            var result = services.AddAutoBoundConfigurations(_configuration);

            Assert.NotNull(result);
            Assert.Same(services, result.Services);
        }
    }
}