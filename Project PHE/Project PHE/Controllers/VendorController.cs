using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PHE.DTOs.Company;
using Project_PHE.Entities;
using Project_PHE.Services;
using Project_PHE.Utilities.Handler;
using System.Net;

namespace Project_PHE.Controllers
{
    [ApiController]
    [Route("vendors")]
    [Authorize("admin")]
    public class VendorController : ControllerBase
    {
        private readonly VendorServices _vendorServices;

        public VendorController(VendorServices vendorServices)
        {
            _vendorServices = vendorServices;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll() 
        {
            try
            {
                var entities = _vendorServices.GetAllVendors();

                if (!entities.Any())
                    return NotFound(new ResponseHandler<GetVendorWithEmployeeDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });
                return Ok(new ResponseHandler<IEnumerable<GetVendorWithEmployeeDto>>
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
                var entity = _vendorServices.GetVendorByGuid(guid);

                if (entity == null)
                    return NotFound(new ResponseHandler<VendorDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor not found"
                    });

                return Ok(new ResponseHandler<VendorDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Vendor found",
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
        [Authorize("user")]
        public IActionResult Create([FromBody] VendorDto vendorDto)
        {
            try
            {
                var createdEntity = _vendorServices.CreateVendor(vendorDto);

                if (createdEntity == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Failed to create vendor"
                    });

                return CreatedAtAction(nameof(GetByGuid), new { guid = createdEntity.Guid }, new ResponseHandler<VendorDto>
                {
                    Code = StatusCodes.Status201Created,
                    Status = HttpStatusCode.Created.ToString(),
                    Message = "Vendor created successfully",
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
        public IActionResult Update(string guid, [FromBody] VendorDto vendorDto)
        {
            try
            {
                var success = _vendorServices.UpdateVendor(guid, vendorDto);

                if (!success)
                    return NotFound(new ResponseHandler<VendorDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor not found"
                    });

                return Ok(new ResponseHandler<VendorDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Vendor updated successfully",
                    Data = vendorDto
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
                var success = _vendorServices.DeleteVendor(guid);

                if (!success)
                    return NotFound(new ResponseHandler<VendorDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor not found"
                    });

                return Ok(new ResponseHandler<object>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Vendor deleted successfully"
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

        [HttpPut("proccess")]
        public IActionResult VendorAccept(string guid)
        {
            try
            {
                var update = _vendorServices.VendorAccept(guid);
                return update switch
                {
                    -1 => NotFound(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Guid not found",
                        Data = guid
                    }),
                    -3 => BadRequest(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "This Vendor has been accepted",
                        Data = guid
                    }),
                    -4 => BadRequest(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "This Vendor has been decline",
                        Data = guid
                    }),
                    0 => StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponseExceptionHandler
                        {
                            Code = StatusCodes.Status500InternalServerError,
                            Status = HttpStatusCode.InternalServerError.ToString(),
                            Message = "Error retrieving data from the database"
                        }),
                    _ => Ok(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Successfully updated",
                        Data = guid
                    })
                };
            }
            catch (Exception ex)
            {
                //if error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = ex.Message
                });
            }
        }

        [HttpPut("approved")]
        public IActionResult VendorApprove(string guid)
        {
            try
            {
                var updateResult = _vendorServices.VendorApprove(guid);

                switch (updateResult)
                {
                    case -1:
                        return NotFound(new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status404NotFound,
                            Status = HttpStatusCode.NotFound.ToString(),
                            Message = "Guid not found",
                            Data = guid
                        });

                    case -3:
                        // For example, you might want to return a specific error response
                        return BadRequest(new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "This Vendor has already been approved, but RoleEmployee update failed",
                            Data = guid
                        });

                    case -4:
                        return BadRequest(new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "This Vendor has already been declined",
                            Data = guid
                        });

                    case 0:
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseExceptionHandler
                            {
                                Code = StatusCodes.Status500InternalServerError,
                                Status = HttpStatusCode.InternalServerError.ToString(),
                                Message = "Error updating data in the database"
                            });

                    case 1:
                        return Ok(new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status200OK,
                            Status = HttpStatusCode.OK.ToString(),
                            Message = "Successfully updated",
                            Data = guid
                        });

                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            new ResponseExceptionHandler
                            {
                                Code = StatusCodes.Status500InternalServerError,
                                Status = HttpStatusCode.InternalServerError.ToString(),
                                Message = "Unhandled error in the server"
                            });
                }
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                // Log.Error(ex, "An error occurred in VendorApprove endpoint");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = ex.Message
                });
            }
        }


        [HttpPut("decline")]
        public async Task<IActionResult> VendorDecline(string guid)
        {
            try
            {
                var update = await _vendorServices.VendorDecline(guid);
                return update switch
                {
                    -5 => BadRequest(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "This vendor has been decline",
                        Data = guid
                    }),
                    -4 => BadRequest(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "This vendor has been approve",
                        Data = guid
                    }),
                    -3 => BadRequest(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "This vendor has been process",
                        Data = guid
                    }),
                    -1 => NotFound(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Guid not found",
                        Data = guid
                    }),
                    0 => StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponseExceptionHandler
                        {
                            Code = StatusCodes.Status500InternalServerError,
                            Status = HttpStatusCode.InternalServerError.ToString(),
                            Message = "Error retrieving data from the database"
                        }),
                    _ => Ok(new ResponseHandler<string>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Successfully updated",
                        Data = guid
                    })
                };
            }
            catch (Exception ex)
            {
                //if error
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
