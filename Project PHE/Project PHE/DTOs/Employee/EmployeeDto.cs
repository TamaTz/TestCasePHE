
using Project_PHE.Entities;

namespace Project_PHE.DTOs.Employee
{
    public class EmployeeDto
    {
        public string Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string RoleEmployee { get; set; }
        public string Password { get; set; }

        public static implicit operator Entities.Employee(EmployeeDto employeeDto)
        {
            return new Entities.Employee
            {
                Guid = employeeDto.Guid,
                Firstname = employeeDto.FirstName,
                Lastname = employeeDto.LastName,
                Phone = employeeDto.Phone,
                Email = employeeDto.Email,
                RoleEmployee = employeeDto.RoleEmployee,
                Password = employeeDto.Password
            };
        }

        public static explicit operator EmployeeDto(Entities.Employee employee)
        {
            return new EmployeeDto
            {
                Guid = employee.Guid,
                FirstName = employee.Firstname,
                LastName = employee.Lastname,
                Phone = employee.Phone,
                Email = employee.Email,
                RoleEmployee = employee.RoleEmployee,
                Password = employee.Password
            };
        }
    }
}
