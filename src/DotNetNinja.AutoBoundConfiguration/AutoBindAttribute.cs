using System;

namespace DotNetNinja.AutoBoundConfiguration
{
    public class AutoBindAttribute: Attribute
    {
        public AutoBindAttribute()
        {
        }

        public AutoBindAttribute(string section)
        {
            Section = section;
        }

        public string Section { get; set; }
    }
}
