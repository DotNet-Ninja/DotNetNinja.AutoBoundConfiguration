using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace DotNetNinja.AutoBoundConfiguration.TestAssembly
{
    [ExcludeFromCodeCoverage]
    public class NoParameterlessConstructor
    {
        public NoParameterlessConstructor(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}