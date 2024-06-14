using Microsoft.AspNetCore.Mvc;
using ProjectModule.Abstraction.ViewModels;
using ProjectModule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] Project project, CancellationToken cancellation)
        {
            try
            {
                await _projectService.AddProject(project, cancellation);
                return Ok("Project added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectByID(string id, CancellationToken cancellation)
        {
            try
            {
                var project = await _projectService.getProjcectByID(id, cancellation);
                if (project == null)
                    return NotFound("Project not found.");
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetProjectByTitle(string title, CancellationToken cancellation)
        {
            try
            {
                var project = await _projectService.GetByTitle(title, cancellation);
                if (project == null)
                    return NotFound("Project not found.");
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int? page = 1, int? pageSize = 10)
        {
            try
            {
                var projects = await _projectService.GetPaging(keyword, page, pageSize);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] Project project, CancellationToken cancellation)
        {
            try
            {
                await _projectService.UpdateProject(id, project, cancellation);
                return Ok("Project updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}
