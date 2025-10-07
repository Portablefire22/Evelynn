using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPlanner.Data.Projects;

public class NotesModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProjectId { get; set; }
    
    [DataType(DataType.DateTime)]
    [Column(TypeName = "TEXT")]
    public DateTime Created { get; set; }
    [DataType(DataType.DateTime)]
    [Column(TypeName = "TEXT")]
    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    public double ModifiedOffset => DateTime.Now.Subtract(Modified.GetValueOrDefault()).TotalHours;
}