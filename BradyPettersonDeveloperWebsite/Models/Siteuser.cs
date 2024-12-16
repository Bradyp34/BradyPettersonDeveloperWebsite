using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("siteuser")]
public partial class Siteuser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fullname")]
    [StringLength(255)]
    public string Fullname { get; set; } = null!;

    [Column("username")]
    [StringLength(255)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("position")]
    [StringLength(255)]
    public string? Position { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();

    [InverseProperty("Assignee")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [InverseProperty("User")]
    public virtual ICollection<Taskuser> Taskusers { get; set; } = new List<Taskuser>();
}
