using Microsoft.AspNetCore.Mvc;
using Project_PHE.DTOs.Project;
using Project_PHE.Services;
using Project_PHE.Utilities.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Project_PHE.Controllers
{
    [Route("api/projects")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectServices _projectServices;

        public ProjectController(ProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            try
            {
                var projects = _projectServices.GetallProject();

                if (!projects.Any())
                    return NotFound(new ResponseHandler<ProjectDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });

                return Ok(new ResponseHandler<IEnumerable<ProjectDto>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data found",
                    Data = projects
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectDto ProjectDto)
        {
            try
            {
                var newProject = _projectServices.CreateProject(ProjectDto);

                return CreatedAtAction(nameof(GetProjectById), new { projectGuid = newProject.Guid }, newProject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{projectGuid}")]
        public IActionResult GetProjectById(string Guid)
        {
            var project = _projectServices.GetProjectByGuid(Guid);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPut("{projectGuid}")]
        public IActionResult UpdateProject(string projectGuid, [FromBody] ProjectDto updateProjectDto)
        {
            try
            {
                var updatedProject = _projectServices.UpdateProject(projectGuid, updateProjectDto);

                if (updatedProject == null)
                {
                    return NotFound();
                }

                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{projectGuid}")]
        public IActionResult DeleteProject(string projectGuid)
        {
            var result = _projectServices.DeleteProject(projectGuid);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
