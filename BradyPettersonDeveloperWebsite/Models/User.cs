using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class User
{
    public int? Id { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? JobTitle { get; set; }

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
}
