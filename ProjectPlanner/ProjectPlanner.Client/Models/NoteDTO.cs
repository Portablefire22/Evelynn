namespace ProjectPlanner.Client.Models;

public class NoteDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProjectId { get; set; }
    public string Contents { get; set; }
}