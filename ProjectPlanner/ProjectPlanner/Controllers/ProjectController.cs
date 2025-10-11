
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Client.Models;
using ProjectPlanner.Data;
using ProjectPlanner.Data.Projects;

namespace ProjectPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        private string? GetCurrentUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task<ActionResult<bool>> DoesUserHaveAccessToProject(string uuid, int projectId)
        {
            var userProjects =
                await _context.UserProjects.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == uuid);
            return userProjects != null;
        }
        
        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjectsByUserId(string userId)
        {
            var projects = await _context.UserProjects.Where(y => y.UserId == userId).Select(x => x.ProjectId).ToListAsync();
            return await _context.Projects.Where(x => projects.Contains(x.Id)).ToListAsync();
        }
        
        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectModel>> GetProjectModel(int id)
        {

            var user = GetCurrentUserId();
            if (user == null ||
                !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return NotFound();
            }
            var projectModel = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (projectModel == null) return NotFound();

            var projectDto = new ProjectDTO()
            {
                Id = projectModel.Id,
                Name = projectModel.Name,
                Description = projectModel.Description,
            };
            return Ok(projectDto);
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject()
        {
            return Redirect("/Projects/Project/0");
        }

        [HttpPut("{id}/NewNote")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutNote(int id, NoteDTO note)
        {
            var user = GetCurrentUserId();
            if (user == null || !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return BadRequest();
            }

            var noteMod = new NotesModel()
            {
                Name = note.Name,
                Created = DateTime.Now,
                ModifiedBy = user,
                ProjectId = id,
                FilePath = ""
            };

            _context.Notes.Add(noteMod);

            await _context.SaveChangesAsync();

            _context.Update(noteMod);
            noteMod.FilePath = $"Storage/Notes/{id}/";

            Directory.CreateDirectory(noteMod.FilePath);
            noteMod.FilePath = $"{noteMod.FilePath}{noteMod.Id}";
            await System.IO.File.WriteAllTextAsync(noteMod.FilePath, note.Contents);
            
            await _context.SaveChangesAsync();
            note.Id = noteMod.Id;
            return Ok(note);
        }
        
        // GET: api/Project/5
        [HttpPut("{id}/EditNote/{noteId}")]
        public async Task<ActionResult<NoteDTO>> EditProjectNote(int id, NoteDTO note)
        {

            var user = GetCurrentUserId();
            if (user == null ||
                !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return NotFound();
            }
            var noteModel = await _context.Notes.FirstOrDefaultAsync(
                x => x.ProjectId == id);

            if (noteModel == null || noteModel.FilePath == null) return NotFound();

            await System.IO.File.WriteAllTextAsync(noteModel.FilePath, note.Contents);

            _context.Update(noteModel);

            noteModel.Name = note.Name;
            noteModel.Modified = DateTime.Now;
            noteModel.ModifiedBy = user;

            await _context.SaveChangesAsync();
            // Set contents to nothing as it would be a waste to send it all back
            note.Contents = "";
            return Ok(note);
        }
        
        // GET: api/Project/5
        [HttpGet("{id}/Notes/{noteId}")]
        public async Task<ActionResult<NoteDTO>> GetProjectNote(int id, int noteId)
        {

            var user = GetCurrentUserId();
            if (user == null ||
                !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return NotFound();
            }
            var noteModel = await _context.Notes.FirstOrDefaultAsync(
                x => x.ProjectId == id);

            if (noteModel == null || noteModel.FilePath == null) return NotFound();

            var contents = await System.IO.File.ReadAllTextAsync(noteModel.FilePath);
            
            var noteDto = new NoteDTO()
            {
                Id = noteModel.Id,
                Name = noteModel.Name,
                Contents = contents,
            };
            
            return Ok(noteDto);
        }
        
        [HttpPut("{id}/NewGantt")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutGantt(int id, GanttDTO gantt)
        {
            var user = GetCurrentUserId();
            if (user == null || !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return BadRequest();
            }

            var ganttMod = new GanttModel()
            {
                Name = gantt.Name,
                Description = gantt.Description,
                Created = DateTime.Now,
                ModifiedBy = user,
                ProjectId = id,
                XmlPath = ""
            };

            _context.Gantts.Add(ganttMod);

            await _context.SaveChangesAsync();

            _context.Update(ganttMod);
            ganttMod.XmlPath = $"Storage/Gantt/{id}/{ganttMod.Id}.xml";
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        // GET: api/Project/5
        [HttpGet("{id}/Gantt")]
        public async Task<ActionResult<IEnumerable<GanttModel>>> GetProjectGantts(int id)
        {

            var user = GetCurrentUserId();
            if (user == null ||
                !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return NotFound();
            }
            var projectModel = await _context.Gantts.Where(x => x.ProjectId == id).ToArrayAsync();
           
            return projectModel;
        }


        [HttpPut("NewProject")]
        public async Task<IActionResult> CreateProject(ProjectDTO projectDto)
        {
            var user = GetCurrentUserId();
            if (user == null) return NotFound();

            var project = new ProjectModel()
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                Created = DateTime.Now,
                ModifiedBy = user,
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            
            var userProj = new UserProjectsModel()
            {
                UserId = user,
                ProjectId = project.Id,
                Capabilities = 3
            };

            await _context.UserProjects.AddAsync(userProj);
            await _context.SaveChangesAsync();

            projectDto.Id = project.Id;
            return Ok(projectDto);
        }
        
        [HttpPut("EditProject/{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditProject(int id, ProjectDTO project)
        {
            var user = GetCurrentUserId();
            if (user == null || !(await DoesUserHaveAccessToProject(user, id)).Value)
            {
                return BadRequest();
            }
            
            var projectModel = await _context.Projects.FindAsync(id);
            if (projectModel == null) return NotFound();

            _context.Update(projectModel);

            projectModel.Name = project.Name;
            projectModel.Description = project.Description;
            projectModel.Modified = DateTime.Now;
            projectModel.ModifiedBy = user;
            
            await _context.SaveChangesAsync();
            return Ok(project);
        }
        
        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectModel(int id, ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return BadRequest();
            }
            
            var user = GetCurrentUserId();
            if (user == null ||
                !(await DoesUserHaveAccessToProject(user, projectModel.Id)).Value)
            {
                return BadRequest();
            }

            _context.Entry(projectModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> PostProjectModel(ProjectModel projectModel)
        {
            _context.Projects.Add(projectModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectModelExists(projectModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjectModel", new { id = projectModel.Id }, projectModel);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectModel(int id)
        {
            var projectModel = await _context.Projects.FirstOrDefaultAsync(x=> x.Id == id);
            var user = GetCurrentUserId();
            if (user == null || projectModel == null ||
                !(await DoesUserHaveAccessToProject(user, projectModel.Id)).Value)
            {
                return NotFound();
            }

            _context.Projects.Remove(projectModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectModelExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
