using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("project")]
public partial class Project
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("projectname")]
    [StringLength(255)]
    public string Projectname { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();

    [InverseProperty("Project")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
