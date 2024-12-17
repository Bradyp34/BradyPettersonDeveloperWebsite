using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Taskuser
{
    public int Id { get; set; }

    public int? Taskid { get; set; }

    public int? Userid { get; set; }
}
