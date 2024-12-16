using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Project
{
    public int? Id { get; set; }

    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
