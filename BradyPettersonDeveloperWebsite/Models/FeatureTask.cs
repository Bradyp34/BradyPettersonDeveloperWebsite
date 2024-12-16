using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("featuretask")]
public partial class Featuretask
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("featureid")]
    public int? Featureid { get; set; }

    [Column("taskid")]
    public int? Taskid { get; set; }

    [ForeignKey("Featureid")]
    [InverseProperty("Featuretasks")]
    public virtual Feature? Feature { get; set; }

    [ForeignKey("Taskid")]
    [InverseProperty("Featuretasks")]
    public virtual Task? Task { get; set; }
}
