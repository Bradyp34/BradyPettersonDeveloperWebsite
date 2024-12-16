using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("taskuser")]
public partial class Taskuser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("taskid")]
    public int? Taskid { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [ForeignKey("Taskid")]
    [InverseProperty("Taskusers")]
    public virtual Projecttask? Task { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Taskusers")]
    public virtual Siteuser? User { get; set; }
}
