using Client.Models;
using Client.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;   
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.Gets(jwToken);
            var employees = new List<Employee>();

            if (result.Data != null)
            {
                employees = result.Data.Select(e => new Employee
                {
                    Guid = e.Guid,
                    Firstname = e.Firstname,
                    Lastname = e.Lastname,
                    Email = e.Email,
                    Phone = e.Phone,
                }).ToList();
            }
            return View(employees);
        }
    }
}
