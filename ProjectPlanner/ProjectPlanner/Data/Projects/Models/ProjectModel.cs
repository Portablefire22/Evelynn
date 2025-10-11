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
    [Column(TypeName = "TEXT")]
    public DateTime Created { get; set; }

    [DataType(DataType.DateTime)]
    [Column(TypeName = "TEXT")]
    public DateTime? Modified { get; set; }

    public string ModifiedBy { get; set; }

    public double ModifiedOffset => double.Round(DateTime.Now.Subtract(Modified.GetValueOrDefault()).TotalHours, 1);
    public double CreatedOffset => double.Round(DateTime.Now.Subtract(Created).TotalHours, 1);
}