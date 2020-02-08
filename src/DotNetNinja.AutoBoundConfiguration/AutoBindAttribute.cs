using System;

namespace DotNetNinja.AutoBoundConfiguration
{
    public class AutoBindAttribute: Attribute
    {
        public AutoBindAttribute(string section)
        {
            Section = section ?? throw new ArgumentNullException(nameof(section));
        }

        public string Section { get; set; }
    }
}
