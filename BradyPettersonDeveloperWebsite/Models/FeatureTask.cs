using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Featuretask
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Featureid { get; set; }

    public int? Taskid { get; set; }
}
