using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPlanner.Data.Projects;

public class NoteAttachmentModel
{
    [Key]
    public int Id { get; set; }
    public int NoteId { get; set; }
    public string UploadedBy { get; set; }
    [DataType(DataType.DateTime)]
    [Column(TypeName = "TEXT")]
    public DateTime Created { get; set; }
    public string FilePath { get; set; }
}