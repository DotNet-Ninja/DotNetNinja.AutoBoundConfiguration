using System;

namespace DotNetNinja.AutoBoundConfiguration
{
    public class AutoBindAttribute: Attribute
    {
        public AutoBindAttribute(string section=null)
        {
            Section = section;
        }

        public string Section { get; set; }
    }
}
