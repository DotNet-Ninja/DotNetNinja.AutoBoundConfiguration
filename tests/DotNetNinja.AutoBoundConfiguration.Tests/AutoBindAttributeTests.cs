using System;
using Xunit;

namespace DotNetNinja.AutoBoundConfiguration.Tests
{
    public class AutoBindAttributeTests
    {
        [Fact]
        public void Intantiation_WithNullSection_ShouldThowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AutoBindAttribute(null));
        }

        [Fact]
        public void Intantiation_WithValidSection_ShouldSetSection()
        {
            const string section = "TestSection";

            var attibute = new AutoBindAttribute(section);

            Assert.Equal(section, attibute.Section);
        }
    }
}
