using DotNetNinja.AutoBoundConfiguration;

namespace SampleApplication.Configuration
{
    [AutoBind("Sample")]
    public class SampleSettings
    {
        public string StringSetting{ get; set; }
        public int IntegerSetting { get; set; }
    }
}
