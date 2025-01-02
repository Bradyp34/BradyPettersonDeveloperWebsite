using Bunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BradyPettersonDeveloperWebsite.Components.Pages; // Adjust if needed
using BradyPettersonDeveloperWebsite.Models;
using Microsoft.AspNetCore.Components;
using BradyPettersonDeveloperWebsite.Components;

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
    public void ProjectManager_HasNewProjectButton () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // No projects are actually needed for this test
        var cut = RenderComponent<ProjectManager>();

        // Assert: Check the "New Project" button presence
        var newProjectButton = cut.Find("button.btn.btn-success");
        Assert.NotNull(newProjectButton);
        Assert.Contains("New Project", newProjectButton.TextContent);
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

    [Fact]
    public void IdeaBoard_NoProjectSelected_ShowsPromptMessage () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var cut = RenderComponent<IdeaBoard>();

        Assert.Contains("Please select a project to view the idea board.", cut.Markup);
    }

    [Fact]
    public async Task IdeaBoard_ProjectsAppearInDropdown () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add test projects
        context.Projects.Add(new Project { Projectname = "Project Alpha" });
        context.Projects.Add(new Project { Projectname = "Project Beta" });
        await context.SaveChangesAsync();

        var cut = RenderComponent<IdeaBoard>();

        // The dropdown should have these projects as options
        var options = cut.FindAll("select.form-select option");
        Assert.Contains(options, opt => opt.TextContent.Contains("Project Alpha"));
        Assert.Contains(options, opt => opt.TextContent.Contains("Project Beta"));
    }

    [Fact]
    public async Task IdeaBoard_SelectProject_ShowsIdeaBoardAndColumns () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var project = new Project { Projectname = "Gamma Project" };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var cut = RenderComponent<IdeaBoard>();

        // Act: select the project
        var select = cut.Find("select.form-select");
        select.Change(project.Id.ToString());

        // Assert: Idea board should now be visible
        Assert.DoesNotContain("Please select a project", cut.Markup);
        Assert.Contains("Gamma Project", cut.Markup);

        // Check MoSCoW column headers
        Assert.Contains("Must Have", cut.Markup);
        Assert.Contains("Should Have", cut.Markup);
        Assert.Contains("Could Have", cut.Markup);
        Assert.Contains("Won't Have", cut.Markup);

        // Check the "New Idea" button
        var newIdeaButton = cut.Find("button.btn.btn-success");
        Assert.Contains("New Idea", newIdeaButton.TextContent);
    }

    [Fact]
    public async Task IdeaBoard_ShowsFeaturesInCorrectColumns () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var project = new Project { Projectname = "Delta Project" };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        // Add features with different MoSCoW values
        context.Features.Add(new Feature { Projectid = project.Id, Featurename = "MustFeature", Moscow = 1 });
        context.Features.Add(new Feature { Projectid = project.Id, Featurename = "ShouldFeature", Moscow = 2 });
        context.Features.Add(new Feature { Projectid = project.Id, Featurename = "CouldFeature", Moscow = 3 });
        context.Features.Add(new Feature { Projectid = project.Id, Featurename = "WontFeature", Moscow = 4 });
        await context.SaveChangesAsync();

        var cut = RenderComponent<IdeaBoard>();

        // Act: select the project to load features
        var select = cut.Find("select.form-select");
        select.Change(project.Id.ToString());

        // Assert: Each feature should appear in its correct column
        Assert.Contains("MustFeature", cut.Markup);
        Assert.Contains("ShouldFeature", cut.Markup);
        Assert.Contains("CouldFeature", cut.Markup);
        Assert.Contains("WontFeature", cut.Markup);
    }

    [Fact]
    public void UserControl_NoUsers_ShowsNoUsersFoundMessage () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated(); // Empty database

        var cut = RenderComponent<UserControl>();

        // Assert
        Assert.Contains("No users found.", cut.Markup);
    }

    [Fact]
    public async Task UserControl_WithUsers_ShowsUsersTable () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add some test users
        context.Siteusers.Add(new Siteuser { Fullname = "Alice", Username = "alice", Position = "Developer", Password = "" });
        context.Siteusers.Add(new Siteuser { Fullname = "Bob", Username = "bob", Position = "Manager", Password = "" });
        await context.SaveChangesAsync();

        // Act
        var cut = RenderComponent<UserControl>();

        // Assert: The table should now be present
        var table = cut.Find("table.table-striped");
        Assert.NotNull(table);

        // Check that the users appear
        Assert.Contains("Alice", cut.Markup);
        Assert.Contains("alice", cut.Markup);
        Assert.Contains("Bob", cut.Markup);
        Assert.Contains("bob", cut.Markup);
    }

    [Fact]
    public async Task UserControl_UsersOrderedById () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add users in reverse order
        context.Siteusers.Add(new Siteuser { Fullname = "Zed", Username = "zed", Position = "Tester", Password = "" });    // Will get Id=1
        context.Siteusers.Add(new Siteuser { Fullname = "Adam", Username = "adam", Position = "Developer", Password = "" }); // Will get Id=2
        await context.SaveChangesAsync();

        var cut = RenderComponent<UserControl>();

        // Assert the order by checking the rendered rows
        var rows = cut.FindAll("table.table-striped tbody tr");
        Assert.Equal(2, rows.Count);

        // First row should have Zed (Id=1), second should have Adam (Id=2)
        Assert.Contains("Zed", rows[0].TextContent);
        Assert.Contains("Adam", rows[1].TextContent);
    }

    [Fact]
    public void UserControl_HasAddUserButton () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var cut = RenderComponent<UserControl>();

        // Check the "Add User" button presence
        var addUserButton = cut.Find("button.btn.btn-success");
        Assert.NotNull(addUserButton);
        Assert.Contains("Add User", addUserButton.TextContent);

        // Click it (no assertion other than no exceptions thrown)
        addUserButton.Click();
    }

    [Fact]
    public async Task UserControl_EditButtonAppearsForEachUser () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add multiple users
        context.Siteusers.Add(new Siteuser { Fullname = "Charlie", Username = "charlie", Position = "Designer", Password = "" });
        context.Siteusers.Add(new Siteuser { Fullname = "Diana", Username = "diana", Position = "QA", Password = "" });
        await context.SaveChangesAsync();

        var cut = RenderComponent<UserControl>();

        // There should be an Edit button for each user
        var editButtons = cut.FindAll("button.btn.btn-primary.btn-sm");
        Assert.Equal(2, editButtons.Count);

        // Click the first edit button to ensure no exception
        editButtons[0].Click();
    }


    //modal tests

    [Fact]
    public void ProjectModal_InitialState_Hidden () {
        var cut = RenderComponent<ProjectModal>();
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task ProjectModal_ShowNewProject_DisplaysNewProjectHeaderAndEmptyFields () {
        var cut = RenderComponent<ProjectModal>();
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        await cut.InvokeAsync(() => cut.Instance.Show()); // New project (no ID)

        var header = cut.Find(".modal-title");
        Assert.Equal("New Project", header.TextContent.Trim());

        var projectNameValue = cut.Find("#projectName").GetAttribute("value");
        Assert.True(string.IsNullOrEmpty(projectNameValue));

        var projectDescriptionValue = cut.Find("#projectDescription").TextContent;
        Assert.True(string.IsNullOrEmpty(projectDescriptionValue.Trim()));
    }

    [Fact]
    public async Task ProjectModal_SaveNewProject_InsertsProjectAndFiresOnProjectSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        bool onProjectSavedCalled = false;

        var cut = RenderComponent<ProjectModal>(parameters => parameters
            .Add(p => p.OnProjectSaved, EventCallback.Factory.Create<Project>(this, (Project p) => onProjectSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show()); // New project
        cut.Find("#projectName").Change("New Project Name");
        cut.Find("#projectDescription").Change("A new description");

        var saveButton = cut.Find("#saveButton");
        saveButton.Click();

        await Task.Delay(100);

        var savedProject = context.Projects.FirstOrDefault(p => p.Projectname == "New Project Name");
        Assert.NotNull(savedProject);
        Assert.Equal("A new description", savedProject.Description);

        Assert.True(onProjectSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task ProjectModal_SaveExistingProject_UpdatesProjectAndFiresOnProjectSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingProject = new Project { Projectname = "Old Name", Description = "Old Desc" };
        context.Projects.Add(existingProject);
        await context.SaveChangesAsync();

        bool onProjectSavedCalled = false;
        var cut = RenderComponent<ProjectModal>(parameters => parameters
            .Add(p => p.OnProjectSaved, EventCallback.Factory.Create<Project>(this, (Project p) => onProjectSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(existingProject));

        // Update fields
        cut.Find("#projectName").Change("Updated Name");
        cut.Find("#projectDescription").Change("Updated Desc");

        var saveButton = cut.Find("#saveButton");
        saveButton.Click();

        await Task.Delay(100);

        var updatedProject = context.Projects.FirstOrDefault(p => p.Id == existingProject.Id);
        Assert.NotNull(updatedProject);
        Assert.Equal("Updated Name", updatedProject.Projectname);
        Assert.Equal("Updated Desc", updatedProject.Description);

        Assert.True(onProjectSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task ProjectModal_CloseModal_HidesModal () {
        var cut = RenderComponent<ProjectModal>();
        await cut.InvokeAsync(() => cut.Instance.Show());

        var closeButton = cut.Find(".btn-close");
        closeButton.Click();

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public void TaskModal_InitialState_Hidden () {
        var cut = RenderComponent<TaskModal>();
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task TaskModal_ShowNewTask_DisplaysNewTaskHeaderAndEmptyFields () {
        var cut = RenderComponent<TaskModal>();
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Show as new task for projectId=123
        await cut.InvokeAsync(() => cut.Instance.Show(123));

        var header = cut.Find(".modal-title");
        Assert.Equal("New Task", header.TextContent.Trim());

        // Fields should be empty (or null):
        var taskNameValue = cut.Find("#taskName").GetAttribute("value");
        Assert.True(string.IsNullOrEmpty(taskNameValue));

        var detailsValue = cut.Find("#taskDetails").TextContent;
        Assert.True(string.IsNullOrEmpty(detailsValue.Trim()));
    }

    [Fact]
    public async Task TaskModal_SaveNewTask_InsertsIntoDatabaseAndFiresOnTaskSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        bool onTaskSavedCalled = false;

        var cut = RenderComponent<TaskModal>(parameters => parameters
            .Add(p => p.OnTaskSaved, EventCallback.Factory.Create(this, () => onTaskSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(300)); // Show modal as a new task for projectId=300

        cut.Find("#taskName").Change("New Task Name");
        cut.Find("#taskDetails").Change("New Task Details");
        cut.Find("#taskStage").Change("3"); // In Review
        // Assume no assignee or dates for simplicity

        var saveButton = cut.Find("button.btn.btn-primary");
        saveButton.Click();

        await Task.Delay(100);

        var savedTask = context.Projecttasks.FirstOrDefault(t => t.Taskname == "New Task Name");
        Assert.NotNull(savedTask);
        Assert.Equal("New Task Details", savedTask.Details);
        Assert.Equal(3, savedTask.Stage);
        Assert.Equal(300, savedTask.Projectid);

        Assert.True(onTaskSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task TaskModal_SaveExistingTask_UpdatesDatabaseAndFiresOnTaskSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingTask = new Projecttask {
            Projectid = 400,
            Taskname = "Old Name",
            Details = "Old Details",
            Stage = 1
        };
        context.Projecttasks.Add(existingTask);
        context.SaveChanges();

        bool onTaskSavedCalled = false;
        var cut = RenderComponent<TaskModal>(parameters => parameters
            .Add(p => p.OnTaskSaved, EventCallback.Factory.Create(this, () => onTaskSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(existingTask));

        // Update fields
        cut.Find("#taskName").Change("Updated Name");
        cut.Find("#taskDetails").Change("Updated Details");
        cut.Find("#taskStage").Change("4"); // Done

        var saveButton = cut.Find("button.btn.btn-primary");
        saveButton.Click();

        await Task.Delay(100);

        var updated = context.Projecttasks.FirstOrDefault(t => t.Id == existingTask.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Taskname);
        Assert.Equal("Updated Details", updated.Details);
        Assert.Equal(4, updated.Stage);

        Assert.True(onTaskSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task TaskModal_DeleteExistingTask_RemovesFromDatabaseAndFiresOnTaskSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingTask = new Projecttask {
            Projectid = 500,
            Taskname = "Deletable Task",
            Details = "Delete Me",
            Stage = 1
        };
        context.Projecttasks.Add(existingTask);
        context.SaveChanges();

        bool onTaskSavedCalled = false;
        var cut = RenderComponent<TaskModal>(parameters => parameters
            .Add(p => p.OnTaskSaved, EventCallback.Factory.Create(this, () => onTaskSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(existingTask));

        var deleteButton = cut.Find("button.btn.btn-danger");
        deleteButton.Click();

        await Task.Delay(100);

        var deleted = context.Projecttasks.FirstOrDefault(t => t.Id == existingTask.Id);
        Assert.Null(deleted);

        Assert.True(onTaskSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task TaskModal_CloseModal_HidesModal () {
        var cut = RenderComponent<TaskModal>();
        await cut.InvokeAsync(() => cut.Instance.Show(999));

        var closeButton = cut.Find(".btn-close");
        closeButton.Click();

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public void FeatureModalTests () {
        Services.AddDbContext<AppDbContext>(options => {
            options.UseInMemoryDatabase("TestFeatureDb");
        });
    }

    [Fact]
    public void FeatureModal_StartsHidden () {
        // Arrange
        var cut = RenderComponent<FeatureModal>();

        // Assert: Modal should not be visible initially
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task FeatureModal_ShowNewFeature_DisplaysNewTaskHeaderAndEmptyFields () {
        var cut = RenderComponent<FeatureModal>();
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Show the modal for a new feature (no parameters means a new feature)
        await cut.InvokeAsync(() => cut.Instance.Show(123)); // or cut.Instance.Show() depending on your code

        // Check header
        var header = cut.Find(".modal-title");
        Assert.Equal("New Task", header.TextContent.Trim());

        // Fields should be empty or have no value attribute
        var featureNameInput = cut.Find("#featureName");
        var featureNameValue = featureNameInput.GetAttribute("value");
        Assert.True(string.IsNullOrEmpty(featureNameValue), "FeatureName input should be empty");

        var descriptionInput = cut.Find("#description");
        var descriptionValue = descriptionInput.GetAttribute("value");
        Assert.True(string.IsNullOrEmpty(descriptionValue), "Description input should be empty");
    }

    [Fact]
    public async Task FeatureModal_ShowExistingFeature_DisplaysEditTaskHeaderAndPopulatesFields () {
        // Arrange
        var cut = RenderComponent<FeatureModal>();
        var context = Services.GetRequiredService<AppDbContext>();

        // Add an existing Feature in DB
        var existingFeature = new Feature { Featurename = "Existing Feature", Description = "Existing Description", Projectid = 456, Moscow = 2 };
        context.Features.Add(existingFeature);
        context.SaveChanges();

        // Act: Show the modal for the existing feature
        await cut.InvokeAsync(() => cut.Instance.Show(existingFeature));

        // Assert: Modal should now be visible and say "Edit Task"
        var header = cut.Find(".modal-title");
        Assert.Equal("Edit Task", header.TextContent.Trim());

        // Fields should be populated
        var featureNameInput = cut.Find("#featureName");
        Assert.Equal("Existing Feature", featureNameInput.GetAttribute("value"));

        var descriptionInput = cut.Find("#description");
        Assert.Equal("Existing Description", descriptionInput.GetAttribute("value"));

        // The MoSCoW select should reflect the existing feature's value
        var moscowSelect = cut.Find("#featureDesignation");
        // BUnit sets the "value" attribute for selected option, we can check that
        Assert.Equal("2", moscowSelect.GetAttribute("value"));
    }

    [Fact]
    public async Task FeatureModal_SaveExistingFeature_UpdatesDatabaseAndFiresOnFeatureSaved () {
        // Arrange
        var context = Services.GetRequiredService<AppDbContext>();
        var existingFeature = new Feature { Featurename = "Old Name", Description = "Old Desc", Projectid = 200, Moscow = 2 };
        context.Features.Add(existingFeature);
        context.SaveChanges();

        bool onFeatureSavedCalled = false;
        var cut = RenderComponent<FeatureModal>(parameters => parameters
            .Add(p => p.OnFeatureSaved, EventCallback.Factory.Create<Feature>(this, (Feature f) => {
                onFeatureSavedCalled = true;
            }))
        );

        await cut.InvokeAsync(() => cut.Instance.Show(existingFeature));

        // Change fields
        cut.Find("#featureName").Change("Updated Name");
        cut.Find("#description").Change("Updated Desc");
        var select = cut.Find("#featureDesignation");
        select.Change("3"); // 'Could'

        // Act: Click save
        var saveButton = cut.Find("button.btn.btn-primary");
        saveButton.Click();

        await Task.Delay(100); // allow async save to complete

        // Assert: Check DB for updated feature
        var updatedFeature = context.Features.FirstOrDefault(f => f.Id == existingFeature.Id);
        Assert.NotNull(updatedFeature);
        Assert.Equal("Updated Name", updatedFeature.Featurename);
        Assert.Equal("Updated Desc", updatedFeature.Description);
        Assert.Equal(3, updatedFeature.Moscow);

        // OnFeatureSaved fired
        Assert.True(onFeatureSavedCalled);

        // Modal hidden
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task FeatureModal_CloseModal_ChangesVisibility () {
        // Arrange
        var cut = RenderComponent<FeatureModal>();
        await cut.InvokeAsync(() => cut.Instance.Show(999));

        // Act: click close button
        var closeButton = cut.Find(".btn-close");
        closeButton.Click();

        // Assert: Modal hidden
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public void UserModal_InitialState_Hidden () {
        var cut = RenderComponent<UserModal>();
        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task UserModal_ShowNewUser_DisplaysAddUserHeaderAndEmptyFields () {
        var cut = RenderComponent<UserModal>();
        await cut.InvokeAsync(() => cut.Instance.Show());

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: flex;", backdrop.GetAttribute("style"));

        var header = cut.Find(".modal-title");
        Assert.Equal("Add User", header.TextContent.Trim());

        var fullnameValue = cut.Find("#fullname").GetAttribute("value") ?? "";
        Assert.Equal(string.Empty, fullnameValue);

        var usernameValue = cut.Find("#username").GetAttribute("value") ?? "";
        Assert.Equal(string.Empty, usernameValue);

        var passwordValue = cut.Find("#password").GetAttribute("value") ?? "";
        Assert.Equal(string.Empty, passwordValue);

        var positionValue = cut.Find("#position").GetAttribute("value") ?? "";
        Assert.Equal(string.Empty, positionValue);

        Assert.Empty(cut.FindAll("button.btn-danger"));
    }

    [Fact]
    public async Task UserModal_ShowExistingUser_DisplaysEditUserHeaderAndPopulatesFields () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingUser = new Siteuser {
            Fullname = "John Doe",
            Username = "jdoe",
            Password = "secret",
            Position = "Developer"
        };
        context.Siteusers.Add(existingUser);
        await context.SaveChangesAsync();

        var cut = RenderComponent<UserModal>();
        await cut.InvokeAsync(() => cut.Instance.Show(existingUser));

        var header = cut.Find(".modal-title");
        Assert.Equal("Edit User", header.TextContent.Trim());

        Assert.Equal("John Doe", cut.Find("#fullname").GetAttribute("value"));
        Assert.Equal("jdoe", cut.Find("#username").GetAttribute("value"));
        Assert.Equal("secret", cut.Find("#password").GetAttribute("value"));
        Assert.Equal("Developer", cut.Find("#position").GetAttribute("value"));

        var deleteButton = cut.Find("button.btn-danger");
        Assert.NotNull(deleteButton);
    }

    [Fact]
    public async Task UserModal_SaveNewUser_InsertsUserAndFiresOnUserSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        bool onUserSavedCalled = false;

        var cut = RenderComponent<UserModal>(parameters => parameters
            .Add(p => p.OnUserSaved, EventCallback.Factory.Create(this, () => onUserSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show());

        cut.Find("#fullname").Change("Alice Wonderland");
        cut.Find("#username").Change("alicew");
        cut.Find("#password").Change("mypassword");
        cut.Find("#position").Change("Manager");

        cut.Find("button.btn-primary").Click();

        await Task.Delay(100);

        var savedUser = context.Siteusers.FirstOrDefault(u => u.Username == "alicew");
        Assert.NotNull(savedUser);
        Assert.Equal("Alice Wonderland", savedUser.Fullname);
        Assert.Equal("mypassword", savedUser.Password);
        Assert.Equal("Manager", savedUser.Position);

        Assert.True(onUserSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task UserModal_SaveExistingUser_UpdatesAndFiresOnUserSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingUser = new Siteuser {
            Fullname = "Bob Brown",
            Username = "bobb",
            Password = "oldpass",
            Position = "Tester"
        };
        context.Siteusers.Add(existingUser);
        await context.SaveChangesAsync();

        bool onUserSavedCalled = false;
        var cut = RenderComponent<UserModal>(parameters => parameters
            .Add(p => p.OnUserSaved, EventCallback.Factory.Create(this, () => onUserSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(existingUser));

        cut.Find("#fullname").Change("Bob Brown Jr.");
        cut.Find("#username").Change("bobbyb");
        cut.Find("#password").Change("newpass");
        cut.Find("#position").Change("Senior Tester");

        cut.Find("button.btn-primary").Click();
        await Task.Delay(100);

        var updatedUser = context.Siteusers.FirstOrDefault(u => u.Id == existingUser.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("Bob Brown Jr.", updatedUser.Fullname);
        Assert.Equal("bobbyb", updatedUser.Username);
        Assert.Equal("newpass", updatedUser.Password);
        Assert.Equal("Senior Tester", updatedUser.Position);

        Assert.True(onUserSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task UserModal_DeleteExistingUser_RemovesUserAndFiresOnUserSaved () {
        var context = Services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var existingUser = new Siteuser {
            Fullname = "Charlie Chaplin",
            Username = "charlie",
            Password = "silentstar",
            Position = "Actor"
        };
        context.Siteusers.Add(existingUser);
        await context.SaveChangesAsync();

        bool onUserSavedCalled = false;
        var cut = RenderComponent<UserModal>(parameters => parameters
            .Add(p => p.OnUserSaved, EventCallback.Factory.Create(this, () => onUserSavedCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show(existingUser));

        cut.Find("button.btn-danger").Click();
        await Task.Delay(100);

        var deletedUser = context.Siteusers.FirstOrDefault(u => u.Id == existingUser.Id);
        Assert.Null(deletedUser);

        Assert.True(onUserSavedCalled);

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
    }

    [Fact]
    public async Task UserModal_CloseModal_HidesModal () {
        var cut = RenderComponent<UserModal>();
        await cut.InvokeAsync(() => cut.Instance.Show());

        cut.Find(".btn-close").Click();

        var backdrop = cut.Find(".modal-backdrop");
        Assert.Contains("display: none;", backdrop.GetAttribute("style"));
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

