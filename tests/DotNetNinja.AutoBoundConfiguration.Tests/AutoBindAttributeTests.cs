using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBindAttributeTests
    {
        [Fact]
        public void Instantiation_WithOutSection_ShouldSetSectionNull()
        {
            var attribute = new AutoBindAttribute();

            Assert.Null(attribute.Section);
        }

        [Fact]
        public void Instantiation_WithValidSection_ShouldSetSection()
        {
            const string section = "TestSection";

            var attribute = new AutoBindAttribute(section);

            Assert.Equal(section, attribute.Section);
        }
    }
}
