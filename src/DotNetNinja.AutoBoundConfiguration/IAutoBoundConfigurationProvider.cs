namespace DotNetNinja.AutoBoundConfiguration
{
    public interface IAutoBoundConfigurationProvider
    {
        TConfiguration Get<TConfiguration>() where TConfiguration : class, new();
        bool Contains<TConfiguration>() where TConfiguration : class, new();
    }
}