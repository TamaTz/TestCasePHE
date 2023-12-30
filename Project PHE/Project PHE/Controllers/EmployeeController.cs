using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PHE.DTOs.Employee;
using Project_PHE.Services;
using Project_PHE.Utilities.Handler;
using Project_PHE.Utility;
using Project_PHE.ViewModel.Response;
using System.Net;
using System.Security.Claims;
using static Project_PHE.Services.EmployeeServices;

namespace Project_PHE.Controllers
{
    [Route("employee")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeServices _employeeServices;
        private readonly ITokenService _tokenService;

        public EmployeeController(EmployeeServices employeeServices, ITokenService tokenService)
        {
            _employeeServices = employeeServices;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var entities = _employeeServices.GetallEmployee();

                if (!entities.Any())
                    return NotFound(new ResponseHandler<EmployeeDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });

                return Ok(new ResponseHandler<IEnumerable<EmployeeDto>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data found",
                    Data = entities
                });
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
        [HttpPost("register/account")]
        public IActionResult Register([FromBody] RegisterEmployeeDto registerDto)
        {
            try
            {
                var registrationResult = _employeeServices.RegisterAccount(registerDto);

                switch (registrationResult)
                {
                    case RegistrationResult.Success:
                        return StatusCode(StatusCodes.Status201Created, new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status201Created,
                            Status = HttpStatusCode.Created.ToString(),
                            Message = "Account registered successfully",
                        });

                    case RegistrationResult.EmailAlreadyExists:
                        return BadRequest(new ResponseHandler<string>
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Email already exists. Please use a different email."
                        });

                    case RegistrationResult.UnknownError:
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                        {
                            Code = StatusCodes.Status500InternalServerError,
                            Status = HttpStatusCode.InternalServerError.ToString(),
                            Message = "Failed to register account. Please try again later."
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseExceptionHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpPut("{guid}")]
        public IActionResult Update(string guid, [FromBody] EmployeeDto vendorDto)
        {
            try
            {
                var success = _employeeServices.UpdateEmployee(guid, vendorDto);

                if (!success)
                    return NotFound(new ResponseHandler<EmployeeDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor not found"
                    });

                return Ok(new ResponseHandler<EmployeeDto>
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

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDto loginVM)
        {
            var account = _employeeServices.Login(loginVM);
            var employee = _employeeServices.GetEmail(loginVM.Email);

            if (account == null)
            {
                return NotFound(new ResponseVM<LoginDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Account Not Found"
                });
            }

            if (account.Password != loginVM.Password)
            {
                return BadRequest(new ResponseVM<LoginDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Password Invalid"
                });
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, employee.Guid.ToString()),
                new(ClaimTypes.Name, $"{employee.Firstname}{employee.Lastname}"),
                new(ClaimTypes.Email, employee.Email),
            };

            var roles = _employeeServices.GetRoles(employee.Guid);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _tokenService.GenerateToken(claims);


            return Ok(new ResponseVM<string>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Login Success",
                Data = token
            });

        }

    }
}
