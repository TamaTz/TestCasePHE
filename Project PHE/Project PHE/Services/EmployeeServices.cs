using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.DTOs.Employee;
using Project_PHE.Entities;
using Project_PHE.Utilities.Handler;

namespace Project_PHE.Services
{
    public class EmployeeServices
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly PheDbContext _dbContext;


        public EmployeeServices(IEmployeeRepository employeeRepository, PheDbContext dbContext)
        {
            _employeeRepository = employeeRepository;
            _dbContext = dbContext;
            _dbContext = dbContext;
        }

        public IEnumerable<EmployeeDto> GetallEmployee()
        {
            var employees = _employeeRepository.GetAll();

            if (!employees.Any()) return Enumerable.Empty<EmployeeDto>();

            var getEmployees = new List<EmployeeDto>();

            foreach (var employee in employees)
                getEmployees.Add(new EmployeeDto
                {
                    Guid = employee.Guid,
                    FirstName = employee.Firstname,
                    LastName = employee.Lastname,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    RoleEmployee = employee.RoleEmployee,
                    Password = employee.Password,
                });

            return getEmployees;
        }

        public RegistrationResult RegisterAccount(RegisterEmployeeDto registerDto)
        {
            try
            {
                var userRoleGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6";

                // Create a new GUID for the employee
                var employeeGuid = Guid.NewGuid().ToString();

                // Create an Employee entity
                var employee = new Employee
                {
                    Guid = employeeGuid,
                    Firstname = registerDto.Firstname,
                    Lastname = registerDto.Lastname,
                    Password = Hashing.HashPassword(registerDto.Password),
                    Phone = registerDto.Phone,
                    Email = registerDto.Email,
                    RoleEmployee = userRoleGuid, // Use the GUID of the default role


                };

                // Save the employee to the database
                _employeeRepository.Create(employee);

                return RegistrationResult.Success;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.ToString());
                return RegistrationResult.UnknownError;
            }
        }


        public enum RegistrationResult
        {
            Success = 1,
            EmailAlreadyExists = 2,
            UnknownError = 0
        }


        public bool UpdateEmployee(string guid, EmployeeDto vendorDto)
        {
            var existingEmployee = _employeeRepository.GetByGuid(guid);

            if (existingEmployee == null) return false; // Vendor not found

            existingEmployee.Firstname = existingEmployee.Firstname;
            existingEmployee.Lastname = existingEmployee.Lastname;
            existingEmployee.Email = existingEmployee.Email;
            existingEmployee.RoleEmployee = vendorDto.RoleEmployee;
            existingEmployee.Phone = vendorDto.Phone;

            return _employeeRepository.Update(existingEmployee);
        }

        public LoginDto Login(LoginDto loginVM)
        {


            var account = _employeeRepository.GetAll();
            var employee = _dbContext.Employees.ToList();


            var query = from emp in employee
                        join acc in account
                        on emp.Guid equals acc.Guid
                        where emp.Email == loginVM.Email
                        select new LoginDto
                        {
                            Email = emp.Email,
                            Password = acc.Password
                        };
            var data = query.FirstOrDefault();

            if (data != null && Hashing.ValidatePassword(loginVM.Password, data.Password))
            {
                loginVM.Password = data.Password;
            }
            return data;
        }


        public IEnumerable<string> GetRoles(string guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);

            if (employee == null) return Enumerable.Empty<string>();

            var employeeRoles = from role in _dbContext.Roles
                                join emp in _dbContext.Employees on role.Guid equals emp.RoleEmployee
                                where emp.RoleEmployee == guid
                                select role.Name;

            return employeeRoles.ToList();
        }


        public Employee GetEmail(string email)
        {
            var employee = _dbContext.Set<Employee>().FirstOrDefault(e => e.Email == email);

            return employee;
        }

    }
}

