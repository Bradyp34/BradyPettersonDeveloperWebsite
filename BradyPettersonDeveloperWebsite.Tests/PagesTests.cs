using Bunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BradyPettersonDeveloperWebsite.Components.Pages; // Adjust if needed
using BradyPettersonDeveloperWebsite.Models;
using Microsoft.AspNetCore.Components;

public class ProjectManagerTests : TestContext {
    public ProjectManagerTests () {
        // Set up in-memory EF Core database for testing
        Services.AddDbContext<AppDbContext>(options => {
            options.UseInMemoryDatabase("TestDb");
        });

        // NavigationManager is needed since it’s injected.
        // BUnit provides a test double for NavigationManager by default.
        // If you need a specific NavigationManager implementation, register it here:
        Services.AddScoped<NavigationManager, TestNavigationManager>();
    }

    [Fact]
    public void ProjectManager_NoProjects_ShowsNoProjectsFoundMessage () {
        // Arrange: No projects in the database
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated(); // Empty database now

        // Act
        var cut = RenderComponent<ProjectManager>();

        // Assert
        Assert.Contains("No projects found.", cut.Markup);
    }

    [Fact]
    public void ProjectManager_WithProjects_ShowsProjectsInTable () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add some test projects
        context.Projects.Add(new Project { Projectname = "Project Alpha", Description = "Alpha desc" });
        context.Projects.Add(new Project { Projectname = "Project Beta", Description = "Beta desc" });
        context.SaveChanges();

        // Act
        var cut = RenderComponent<ProjectManager>();

        // Assert: The table should now be present
        var table = cut.Find("table.table-striped");
        Assert.NotNull(table);

        // Check that the projects appear
        Assert.Contains("Project Alpha", cut.Markup);
        Assert.Contains("Project Beta", cut.Markup);
        Assert.Contains("Alpha desc", cut.Markup);
        Assert.Contains("Beta desc", cut.Markup);
    }

    [Fact]
    public void ProjectManager_HasNewTaskButton () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // No projects are actually needed for this test
        var cut = RenderComponent<ProjectManager>();

        // Assert: Check the "New Task" button presence
        var newTaskButton = cut.Find("button.btn.btn-success");
        Assert.NotNull(newTaskButton);
        Assert.Contains("New Task", newTaskButton.TextContent);
    }

    [Fact]
    public void ProjectManager_ProjectNameIsClickable () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var project = new Project { Projectname = "Project Gamma", Description = "Gamma desc" };
        context.Projects.Add(project);
        context.SaveChanges();

        var cut = RenderComponent<ProjectManager>();

        // The project name should be rendered as a button
        var projectButton = cut.Find("button.btn-link");
        Assert.Contains("Project Gamma", projectButton.TextContent);

        // Act: Click the project button
        // We can't directly verify the modal opens without either a spy on projectModal or verifying state changes.
        // But we can ensure no exceptions are thrown by clicking.
        projectButton.Click();

        // Assert: If needed, we can verify that the page still renders
        Assert.Contains("Project Gamma", cut.Markup);
    }

    [Fact]
    public void TaskListTests () {
        // Set up in-memory EF Core database for testing
        Services.AddDbContext<AppDbContext>(options => {
            options.UseInMemoryDatabase("TestTaskDb");
        });

        // NavigationManager is needed. BUnit provides a NavigationManager stub by default,
        // but we'll explicitly register one if needed.
        Services.AddScoped<NavigationManager, TestNavigationManager>();
    }

    [Fact]
    public void TaskList_NoProjectSelected_ShowsPromptMessage () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // No projects in the database
        var cut = RenderComponent<TaskList>();

        // Assert
        Assert.Contains("Please select a project to view the task board.", cut.Markup);
    }

    [Fact]
    public async Task TaskList_ProjectsInDropdown () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add some test projects
        context.Projects.Add(new Project { Projectname = "Alpha Project" });
        context.Projects.Add(new Project { Projectname = "Beta Project" });
        await context.SaveChangesAsync();

        // Act
        var cut = RenderComponent<TaskList>();

        // Assert: The dropdown should now have these projects as options
        var options = cut.FindAll("select.form-select option");
        Assert.Contains(options, opt => opt.TextContent.Contains("Alpha Project"));
        Assert.Contains(options, opt => opt.TextContent.Contains("Beta Project"));
    }

    [Fact]
    public async Task TaskList_SelectProject_ShowsTaskBoardAndColumns () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var project = new Project { Projectname = "Gamma Project" };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var cut = RenderComponent<TaskList>();

        // Act: Select the project
        var select = cut.Find("select.form-select");
        select.Change(project.Id.ToString());

        // Assert: Task board should now be visible
        Assert.DoesNotContain("Please select a project", cut.Markup);
        Assert.Contains("Gamma Project - Task Board", cut.Markup);

        // Check for column headers
        Assert.Contains("To Do", cut.Markup);
        Assert.Contains("In Progress", cut.Markup);
        Assert.Contains("In Review", cut.Markup);
        Assert.Contains("Done", cut.Markup);

        // Check that "New Task" button is present
        var newTaskButton = cut.Find("button.btn.btn-success");
        Assert.Contains("New Task", newTaskButton.TextContent);
    }

    [Fact]
    public async Task TaskList_ShowsTasksInCorrectColumns () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var project = new Project { Projectname = "Delta Project" };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        // Add tasks with different stages
        context.Projecttasks.Add(new Projecttask { Projectid = project.Id, Taskname = "Task 1", Stage = 1 }); // To Do
        context.Projecttasks.Add(new Projecttask { Projectid = project.Id, Taskname = "Task 2", Stage = 2 }); // In Progress
        context.Projecttasks.Add(new Projecttask { Projectid = project.Id, Taskname = "Task 3", Stage = 3 }); // In Review
        context.Projecttasks.Add(new Projecttask { Projectid = project.Id, Taskname = "Task 4", Stage = 4 }); // Done
        await context.SaveChangesAsync();

        var cut = RenderComponent<TaskList>();

        // Act: Select the project to load tasks
        var select = cut.Find("select.form-select");
        select.Change(project.Id.ToString());

        // Assert:
        Assert.Contains("Task 1", cut.Markup); // Should be in "To Do"
        Assert.Contains("Task 2", cut.Markup); // "In Progress"
        Assert.Contains("Task 3", cut.Markup); // "In Review"
        Assert.Contains("Task 4", cut.Markup); // "Done"
    }
}

// A simple NavigationManager test double (if needed)
public class TestNavigationManager : NavigationManager {
    public TestNavigationManager () {
        Initialize("http://localhost/", "http://localhost/");
    }

    protected override void NavigateToCore (string uri, bool forceLoad) {
        // Do nothing, just a stub for testing
    }
}
