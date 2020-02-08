using System.Collections.Generic;
using DotNetNinja.AutoBoundConfiguration;

namespace SampleApplication.Configuration
{
    [AutoBind("Links")]
    public class LinkSettings
    {
        public LinkSettings()
        {
            Items = new List<LinkSetting>();
        }

        public List<LinkSetting> Items { get; set; }
    }
}