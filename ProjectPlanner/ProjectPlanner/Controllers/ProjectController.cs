
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var projectModel = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);

            var user = GetCurrentUserId();
            if (user == null || projectModel == null ||
                !(await DoesUserHaveAccessToProject(user, projectModel.Id)).Value)
            {
                return NotFound();
            }
            return projectModel;
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject()
        {
            return Redirect("/Projects/Project/0");
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
