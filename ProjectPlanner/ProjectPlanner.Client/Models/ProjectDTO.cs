namespace ProjectPlanner.Client.Models;

public class ProjectDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}