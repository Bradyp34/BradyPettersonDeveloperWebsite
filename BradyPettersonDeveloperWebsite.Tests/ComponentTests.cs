using Bunit;
using Xunit;
using BradyPettersonDeveloperWebsite.Components;
using BradyPettersonDeveloperWebsite.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Moq;

public class ComponentTests : TestContext {
    private AppDbContext SetupDbContext () {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use unique database names for each test
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Siteusers.AddRange(
            new Siteuser { Id = 1, Fullname = "Alice Johnson", Position = "Developer" },
            new Siteuser { Id = 2, Fullname = "Bob Smith", Position = "Designer" }
        );
        dbContext.SaveChanges();

        return dbContext;
    }

    [Fact]
    public void TaskModal_ShowsModal_ForNewTask () {
        // Arrange
        var dbContext = SetupDbContext();
        Services.AddScoped(_ => dbContext);
        var cut = RenderComponent<TaskModal>();

        // Act
        cut.InvokeAsync(() => cut.Instance.Show(new Projecttask()));

        // Assert
        cut.MarkupMatches(@"
        <div class=""modal-backdrop"" style=""display: flex;"">
            <div class=""modal-box"">
                <div class=""modal-header"">
                    <h5 class=""modal-title"">New Task</h5>
                    <button type=""button"" class=""btn-close"">&times;</button>
                </div>
                <div class=""modal-body"">
                    <h3></h3>
                    <div class=""mb-3"">
                        <label for=""taskName"" class=""form-label"">Task Name</label>
                        <input type=""text"" class=""form-control"" id=""taskName"" />
                    </div>
                    <div class=""mb-3"">
                        <label for=""taskDetails"" class=""form-label"">Details</label>
                        <textarea class=""form-control"" id=""taskDetails""></textarea>
                    </div>
                    <div class=""mb-3"">
                        <label for=""taskAssignee"" class=""form-label"">Assignee</label>
                        <select class=""form-select"" id=""taskAssignee"">
                            <option value="""">-- Select an Assignee --</option>
                            <option value=""1"">Alice Johnson - Developer</option>
                            <option value=""2"">Bob Smith - Designer</option>
                        </select>
                    </div>
                    <div class=""mb-3"">
                        <label for=""taskStage"" class=""form-label"">Stage</label>
                        <select class=""form-select"" id=""taskStage"">
                            <option value=""1"">To Do</option>
                            <option value=""2"">In Progress</option>
                            <option value=""3"">In Review</option>
                            <option value=""4"">Done</option>
                        </select>
                    </div>
                    <div class=""mb-3"">
                        <label for=""startDate"" class=""form-label"">Start Date</label>
                        <input type=""date"" class=""form-control"" id=""startDate"" />
                    </div>
                    <div class=""mb-3"">
                        <label for=""endDate"" class=""form-label"">Due Date</label>
                        <input type=""date"" class=""form-control"" id=""endDate"" />
                    </div>
                    <div class=""d-flex justify-content-end"">
                        <button class=""btn btn-primary"">Save</button>
                    </div>
                </div>
            </div>
        </div>");
    }

    [Fact]
    public void TaskModal_SavesTask_OnSaveClick () {
        // Arrange
        var dbContext = SetupDbContext();
        Services.AddScoped(_ => dbContext);
        var cut = RenderComponent<TaskModal>();

        var newTask = new Projecttask { Taskname = "Test Task", Details = "Task Details" };

        cut.InvokeAsync(() => cut.Instance.Show(newTask));

        // Act
        cut.Find("button.btn-primary").Click();

        // Assert
        Assert.Contains(dbContext.Projecttasks, t => t.Taskname == "Test Task" && t.Details == "Task Details");
    }
}
