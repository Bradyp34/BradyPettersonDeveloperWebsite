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
    public void Renders_Correctly_When_No_Project_Selected () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act & Assert: Check only the main content without the modal
        component.Find("h3").MarkupMatches("<h3>Task List</h3>");
        component.Find("p").MarkupMatches("<p>This page displays a task board for a selected project. Select a project from the dropdown to view the tasks for that project.</p>");
        component.Find("select.form-select").MarkupMatches(@"
        <select class=""form-select mb-3"">
            <option value="""">-- Select a Project --</option>
            <option value=""1"">Project Alpha</option>
            <option value=""2"">Project Beta</option>
        </select>
    ");
        component.FindAll("p")[1].MarkupMatches("<p>Please select a project to view the task board.</p>");
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

    [Fact]
    public async Task Clicking_New_Task_Opens_TaskModal () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("1"); // Select Project with Id=1

        await Task.Delay(100); // Allow time for state change

        var newTaskButton = component.Find("button.btn-success");
        newTaskButton.Click();

        // Assert: Check if the modal's backdrop becomes visible
        var modalBackdrop = component.Find("div.modal-backdrop");
        Assert.Contains("display: flex;", modalBackdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task Selecting_Project_Renders_KanbanBoard_With_Tasks () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("1"); // Select Project Alpha (Id=1)

        await Task.Delay(100); // Allow time for state change

        // Assert: Verify the Kanban board is rendered with tasks
        var boardTitle = component.Find("h4.text-center");
        Assert.Contains("Project Alpha - Task Board", boardTitle.TextContent);

        // Verify tasks in each stage
        var toDoTasks = component.FindAll("h5.text-primary + div .btn");
        var inProgressTasks = component.FindAll("h5.text-warning + div .btn");
        var inReviewTasks = component.FindAll("h5.text-info + div .btn");
        var doneTasks = component.FindAll("h5.text-success + div .btn");

        Assert.Single(toDoTasks);
        Assert.Equal("Task 1", toDoTasks[0].TextContent);

        Assert.Single(inProgressTasks);
        Assert.Equal("Task 2", inProgressTasks[0].TextContent);

        Assert.Single(inReviewTasks);
        Assert.Equal("Task 3", inReviewTasks[0].TextContent);

        Assert.Single(doneTasks);
        Assert.Equal("Task 4", doneTasks[0].TextContent);
    }

    [Fact]
    public async Task Clicking_On_Existing_Task_Opens_TaskModal () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("1"); // Select Project Alpha (Id=1)

        await Task.Delay(100); // Allow time for tasks to load

        var taskButton = component.Find("button.btn-outline-secondary"); // First task button
        taskButton.Click();

        // Assert: Check if the modal's backdrop becomes visible
        var modalBackdrop = component.Find("div.modal-backdrop");
        Assert.Contains("display: flex;", modalBackdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task Selecting_Another_Project_Loads_Correct_Tasks () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("2"); // Select Project Beta (Id=2)

        await Task.Delay(100); // Allow time for state change

        // Assert: Verify the Kanban board is rendered with tasks for Project Beta
        var boardTitle = component.Find("h4.text-center");
        Assert.Contains("Project Beta - Task Board", boardTitle.TextContent);

        // Verify tasks in the "To Do" column
        var toDoTasks = component.FindAll("h5.text-primary + div .btn");

        Assert.Single(toDoTasks);
        Assert.Equal("Task A", toDoTasks[0].TextContent);
    }

    [Fact]
    public async Task No_Tasks_Displays_Empty_Message () {
        // Arrange
        var dbContext = CreateDbContext();
        Services.AddSingleton(dbContext);

        var component = RenderComponent<TaskList>();

        // Act
        var select = component.Find("select.form-select");
        select.Change("2"); // Select Project Beta (Id=2)

        await Task.Delay(100); // Allow time for state change

        // Verify there are no tasks in "In Progress", "In Review", or "Done"
        var inProgressTasks = component.FindAll("h5.text-warning + div .btn");
        var inReviewTasks = component.FindAll("h5.text-info + div .btn");
        var doneTasks = component.FindAll("h5.text-success + div .btn");

        Assert.Empty(inProgressTasks);
        Assert.Empty(inReviewTasks);
        Assert.Empty(doneTasks);

        // Verify the empty message appears in these columns
        var emptyMessages = component.FindAll("div.kanban-card.empty-task");
        Assert.Equal(3, emptyMessages.Count);
    }
}
