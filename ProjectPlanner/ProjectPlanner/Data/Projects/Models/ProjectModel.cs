using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace ProjectPlanner.Data.Projects;

public class ProjectModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    [DataType(DataType.DateTime)]
    [Column(TypeName = "INTEGER")]
    public DateTime Created { get; set; }
    
    [DataType(DataType.DateTime)]
    [Column(TypeName = "INTEGER")]
    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    public double ModifiedOffset => DateTime.Now.Subtract(Modified.GetValueOrDefault()).TotalHours;
}