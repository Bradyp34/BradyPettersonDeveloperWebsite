using Bunit;
using Xunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using BradyPettersonDeveloperWebsite.Components.Layout; // Adjust to the actual namespace of your component

public class LayoutTests : TestContext {
    [Fact]
    public void NavBar_ShouldContainBrandName () {
        // Arrange: Render the component that contains the navbar
        var cut = RenderComponent<NavMenu>(); // Replace 'NavMenu' with the actual component name

        // Act: Find the element representing the navbar brand
        var brandElement = cut.Find("a.navbar-brand");

        // Assert: Check that the brand link contains the correct text
        Assert.Equal("BradyPetterson'sWebsite", brandElement.TextContent.Trim());
    }

    [Fact]
    public void NavBar_ShouldContainNavigationToggler () {
        var cut = RenderComponent<NavMenu>();

        // The toggler is an <input type="checkbox" class="navbar-toggler" ...> element
        var toggler = cut.Find("input.navbar-toggler");
        Assert.NotNull(toggler);
        Assert.Equal("checkbox", toggler.GetAttribute("type"));
    }

    [Theory]
    [InlineData("", "Home")]
    [InlineData("projectManager", "Project Manager")]
    [InlineData("taskList", "Task List")]
    [InlineData("ideaBoard", "Idea Board")]
    [InlineData("userControl", "User Control")]
    public void NavBar_ShouldContainExpectedNavLinks (string expectedHref, string expectedText) {
        var cut = RenderComponent<NavMenu>();

        // Find all NavLink elements
        // Since NavLink is a Blazor component that renders as <a> underneath, we can look for `.nav-link`
        var navLinks = cut.FindAll(".nav-link");

        // Ensure we can find the one we're looking for
        var link = navLinks.FirstOrDefault(l => l.TextContent.Contains(expectedText));
        Assert.NotNull(link);

        // Verify that the href attribute matches what we expect
        // Note: NavLink typically sets the href attribute on the underlying <a> tag
        //       If your routing uses a different base path or a leading slash, adjust accordingly.
        Assert.Equal(expectedHref, link.GetAttribute("href"));
    }
}
