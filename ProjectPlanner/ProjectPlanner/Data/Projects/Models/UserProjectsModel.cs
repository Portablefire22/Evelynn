using System.ComponentModel.DataAnnotations;

namespace ProjectPlanner.Data.Projects;

public class UserProjectsModel
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ProjectId { get; set; }
    public int Capabilities { get; set; }
}