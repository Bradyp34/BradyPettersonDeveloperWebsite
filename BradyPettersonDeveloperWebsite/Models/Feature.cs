using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

[Table("feature")]
public partial class Feature
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("featurename")]
    [StringLength(255)]
    public string Featurename { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("moscowclassification")]
    public short? Moscowclassification { get; set; }

    [InverseProperty("Feature")]
    public virtual ICollection<Featuretask> Featuretasks { get; set; } = new List<Featuretask>();
}
