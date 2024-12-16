using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Task
{
    public int? Id { get; set; }

    public string? TaskName { get; set; }

    public bool? Completed { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Due { get; set; }

    public int? AssigneeId { get; set; }

    public string? Details { get; set; }

    public int? Stage { get; set; }

    public int? ProjectId { get; set; }

    public virtual ICollection<FeatureTask> FeatureTasks { get; set; } = new List<FeatureTask>();

    public virtual Project? Project { get; set; }

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
}
