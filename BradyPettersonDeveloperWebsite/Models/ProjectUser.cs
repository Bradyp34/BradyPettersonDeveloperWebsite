using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Projectuser
{
    public int Id { get; set; }

    public int? Projectid { get; set; }

    public int? Userid { get; set; }
}
