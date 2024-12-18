using System;
using System.Collections.Generic;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class Siteuser
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Position { get; set; }
}
