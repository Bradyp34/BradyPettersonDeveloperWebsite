using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class FeatureTask
{
    public int? Id { get; set; }

    public string? Description { get; set; }

    public int? FeatureId { get; set; }

    public int? TaskId { get; set; }

    public virtual Feature? Feature { get; set; }

    public virtual Task? Task { get; set; }
}
