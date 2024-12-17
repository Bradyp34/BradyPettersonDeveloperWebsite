using Bunit;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using BradyPettersonDeveloperWebsite.Components.Pages;
using BradyPettersonDeveloperWebsite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Bunit.TestDoubles;

public class TaskListTests : TestContext {
    private AppDbContext CreateDbContext () {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Unique database for each test
            .Options;
        var dbContext = new AppDbContext(options);
        SeedDatabase(dbContext);
        return dbContext;
    }

    private void SeedDatabase (AppDbContext dbContext) {
        // Add test projects
        var projects = new List<Project>
        {
            new Project { Id = 1, Projectname = "Project Alpha" },
            new Project { Id = 2, Projectname = "Project Beta" }
        };

        dbContext.Projects.AddRange(projects);

        // Add test tasks
        var tasks = new List<Projecttask>
        {
            new Projecttask { Id = 1, Projectid = 1, Taskname = "Task 1", Stage = 1 },
            new Projecttask { Id = 2, Projectid = 1, Taskname = "Task 2", Stage = 2 },
            new Projecttask { Id = 3, Projectid = 1, Taskname = "Task 3", Stage = 3 },
            new Projecttask { Id = 4, Projectid = 1, Taskname = "Task 4", Stage = 4 },
            new Projecttask { Id = 5, Projectid = 2, Taskname = "Task A", Stage = 1 }
        };

        dbContext.Projecttasks.AddRange(tasks);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task Clicking_New_Task_Navigates_To_NewTask_Page () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("1"); // Select Project with Id=1

        await Task.Delay(100);

        var newTaskButton = component.Find("button.btn-success");
        newTaskButton.Click();

        // Assert
        Assert.EndsWith("/newTask", navigationManager.Uri);
    }

    [Fact]
    public void Renders_Correctly_When_No_Project_Selected () {
        // Arrange & Act
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Assert
        component.MarkupMatches(@"
            <h3>Task List</h3>
            <p>This page displays a task board for a selected project. Select a project from the dropdown to view the tasks for that project.</p>
            <select class=""form-select mb-3"">
                <option value="""">-- Select a Project --</option>
                <option value=""1"">Project Alpha</option>
                <option value=""2"">Project Beta</option>
            </select>
            <p>Please select a project to view the task board.</p>
        ");
    }

    [Fact]
    public void ProjectDropdown_Contains_All_Projects () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");

        // Assert
        var options = select.Children;
        Assert.Equal(3, options.Length); // Including the default option
        Assert.Equal("-- Select a Project --", options[0].TextContent);
        Assert.Equal("Project Alpha", options[1].TextContent);
        Assert.Equal("Project Beta", options[2].TextContent);
    }
}
