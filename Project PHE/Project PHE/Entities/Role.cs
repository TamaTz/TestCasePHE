namespace Project_PHE.Entities
{
    public class Role
    {
        public string? Guid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employee> EmployeeRoles { get; set; }

    }
}
