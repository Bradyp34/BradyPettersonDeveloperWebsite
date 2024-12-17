using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Projecttask
{
    public int Id { get; set; }

    public string Taskname { get; set; } = null!;

    public DateOnly? Started { get; set; }

    public DateOnly? Due { get; set; }

    public int? Assigneeid { get; set; }

    public string? Details { get; set; }

    public int? Stage { get; set; }

    public int Projectid { get; set; }
}
