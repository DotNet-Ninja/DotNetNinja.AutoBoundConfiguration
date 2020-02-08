using System.Diagnostics.CodeAnalysis;

namespace DotNetNinja.AutoBoundConfiguration.TestAssembly
{
    [AutoBind("Other")]
    [ExcludeFromCodeCoverage]
    public class OtherSettings
    {
        public string OtherValue { get; set; }

        public static readonly OtherSettings Default = new OtherSettings {OtherValue = "A Value"};
    }
}
