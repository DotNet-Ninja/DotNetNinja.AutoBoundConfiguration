using System.Diagnostics.CodeAnalysis;

namespace DotNetNinja.AutoBoundConfiguration.TestAssembly
{
    [AutoBind("Sample")]
    [ExcludeFromCodeCoverage]
    public class SampleSettings
    {
        public string StringSetting { get; set; }

        public int IntegerSetting { get; set; }

        public static readonly SampleSettings Default = new SampleSettings {StringSetting = "A String", IntegerSetting = 1234};
    }
}
