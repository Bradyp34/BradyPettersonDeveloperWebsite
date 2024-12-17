using Bunit;
using Xunit;
using BradyPettersonDeveloperWebsite.Components.Layout;
using Microsoft.AspNetCore.Components;

public class LayoutTests : TestContext {
    [Fact]
    public void NavMenu_RendersAllNavigationLinks () {
        // Arrange
        var component = RenderComponent<NavMenu>();

        // Act
        var navLinks = component.FindAll("a.nav-link");

        // Assert
        Assert.Equal(4, navLinks.Count);
        Assert.Contains(navLinks, link => link.TextContent.Contains("Home"));
        Assert.Contains(navLinks, link => link.TextContent.Contains("Task List"));
        Assert.Contains(navLinks, link => link.TextContent.Contains("Feature List"));
        Assert.Contains(navLinks, link => link.TextContent.Contains("User Control"));
    }

    [Fact]
    public void NavMenu_HomeLink_HasCorrectHref () {
        // Arrange
        var component = RenderComponent<NavMenu>();

        // Act
        var homeLink = component.Find("a.nav-link[href='']");

        // Assert
        Assert.NotNull(homeLink);
        Assert.Contains("Home", homeLink.TextContent);
    }

    [Fact]
    public void NavMenu_TaskListLink_HasCorrectHref () {
        // Arrange
        var component = RenderComponent<NavMenu>();

        // Act
        var taskListLink = component.Find("a.nav-link[href='taskList']");

        // Assert
        Assert.NotNull(taskListLink);
        Assert.Contains("Task List", taskListLink.TextContent);
    }

    [Fact]
    public void NavMenu_FeatureListLink_HasCorrectHref () {
        // Arrange
        var component = RenderComponent<NavMenu>();

        // Act
        var featureListLink = component.Find("a.nav-link[href='featureList']");

        // Assert
        Assert.NotNull(featureListLink);
        Assert.Contains("Feature List", featureListLink.TextContent);
    }

    [Fact]
    public void NavMenu_UserControlLink_HasCorrectHref () {
        // Arrange
        var component = RenderComponent<NavMenu>();

        // Act
        var userControlLink = component.Find("a.nav-link[href='userControl']");

        // Assert
        Assert.NotNull(userControlLink);
        Assert.Contains("User Control", userControlLink.TextContent);
    }
}
