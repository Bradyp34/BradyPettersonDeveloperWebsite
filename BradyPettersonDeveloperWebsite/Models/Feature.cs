using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Feature
{
    public int Id { get; set; }

    public string? Featurename { get; set; }

    public string? Description { get; set; }

    public int? Moscow { get; set; }

    public int Projectid { get; set; }
}
