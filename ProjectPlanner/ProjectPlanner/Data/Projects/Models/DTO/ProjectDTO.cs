using System.ComponentModel.DataAnnotations;

namespace ProjectPlanner.Data.Projects.DTO;

public class ProjectDTO
{
    public int Id { get; set; } = -1;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Projects require a non-zero length name.")]
    public string Name { get; set; }
    public string? Description { get; set; }
}