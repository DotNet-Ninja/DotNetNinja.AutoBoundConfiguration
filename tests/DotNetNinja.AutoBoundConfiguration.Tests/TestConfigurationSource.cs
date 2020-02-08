using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public static class TestConfigurationSource
    {
        public static IConfigurationRoot Default
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Sample:StringSetting", "String"),
                        new KeyValuePair<string, string>("Sample:IntegerSetting", "1234")
                    });
                return builder.Build();
            }
        }

        public static IConfigurationRoot AutoSectionFinding
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("AutoSampleSettings:StringSetting", "AutoString"),
                        new KeyValuePair<string, string>("AutoSampleSettings:IntegerSetting", "1234")
                    });
                return builder.Build();
            }
        }
    }
}