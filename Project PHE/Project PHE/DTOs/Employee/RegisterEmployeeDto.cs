namespace Project_PHE.DTOs.Employee
{
    public class RegisterEmployeeDto
    {
        public string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string RoleEmployee { get ; set; }
    }
}
