using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetNinja.AutoBoundConfiguration.TestAssembly2
{
    [AutoBind("Another")]
    [ExcludeFromCodeCoverage]
    public class AnotherSettings
    {
        public string Data { get; set; }
    }
}
