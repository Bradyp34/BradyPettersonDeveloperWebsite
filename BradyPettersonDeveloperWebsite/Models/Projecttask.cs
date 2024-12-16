using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("projecttask")]
public partial class Projecttask
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("taskname")]
    [StringLength(255)]
    public string Taskname { get; set; } = null!;

    [Column("started", TypeName = "timestamp without time zone")]
    public DateTime? Started { get; set; }

    [Column("due", TypeName = "timestamp without time zone")]
    public DateTime? Due { get; set; }

    [Column("assigneeid")]
    public int? Assigneeid { get; set; }

    [Column("details")]
    public string? Details { get; set; }

    [Column("stage")]
    public int? Stage { get; set; }

    [Column("projectid")]
    public int? Projectid { get; set; }

    [ForeignKey("Assigneeid")]
    [InverseProperty("Projecttasks")]
    public virtual Siteuser? Assignee { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<Featuretask> Featuretasks { get; set; } = new List<Featuretask>();

    [ForeignKey("Projectid")]
    [InverseProperty("Projecttasks")]
    public virtual Project? Project { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<Taskuser> Taskusers { get; set; } = new List<Taskuser>();
}
