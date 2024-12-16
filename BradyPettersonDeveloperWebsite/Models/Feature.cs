using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Feature
{
    public int? Id { get; set; }

    public string? FeatureName { get; set; }

    public string? Description { get; set; }

    public int? MoScoWclassification { get; set; }

    public virtual ICollection<FeatureTask> FeatureTasks { get; set; } = new List<FeatureTask>();
}
