using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PHE.DTOs.Role;
using Project_PHE.Services;
using Project_PHE.Utilities.Handler;
using System;
using System.Linq;
using System.Net;

namespace Project_PHE.Controllers
{
    [Route("role")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly RoleServices _services;

        public RoleController(RoleServices services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var entities = _services.GetAllRoles();

                if (!entities.Any())
                    return NotFound(new ResponseHandler<RoleDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });

                return Ok(new ResponseHandler<IEnumerable<RoleDto>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data found",
                    Data = entities
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

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(string guid)
        {
            try
            {
                var entity = _services.GetRoleByGuid(guid);

                if (entity == null)
                    return NotFound(new ResponseHandler<RoleDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Role not found"
                    });

                return Ok(new ResponseHandler<RoleDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Role found",
                    Data = entity
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
        public IActionResult Create([FromBody] RoleDto roleDto)
        {
            try
            {
                var createdEntity = _services.CreateRole(roleDto);

                if (createdEntity == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Failed to create role"
                    });

                return CreatedAtAction(nameof(GetByGuid), new { guid = createdEntity.Guid }, new ResponseHandler<RoleDto>
                {
                    Code = StatusCodes.Status201Created,
                    Status = HttpStatusCode.Created.ToString(),
                    Message = "Role created successfully",
                    Data = createdEntity
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

        [HttpPut("{guid}")]
        public IActionResult Update(string guid, [FromBody] RoleDto roleDto)
        {
            try
            {
                var success = _services.UpdateRole(guid, roleDto);

                if (!success)
                    return NotFound(new ResponseHandler<RoleDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Role not found"
                    });

                return Ok(new ResponseHandler<RoleDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Role updated successfully",
                    Data = roleDto
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

        [HttpDelete("{guid}")]
        public IActionResult Delete(string guid)
        {
            try
            {
                var success = _services.DeleteRole(guid);

                if (!success)
                    return NotFound(new ResponseHandler<RoleDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Role not found"
                    });

                return Ok(new ResponseHandler<object>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Role deleted successfully"
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
    }
}
