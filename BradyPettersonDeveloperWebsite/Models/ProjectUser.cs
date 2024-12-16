using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("projectuser")]
public partial class Projectuser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("projectid")]
    public int? Projectid { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [ForeignKey("Projectid")]
    [InverseProperty("Projectusers")]
    public virtual Project? Project { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Projectusers")]
    public virtual Siteuser? User { get; set; }
}
