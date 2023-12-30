namespace Project_PHE.Entities
{
    public class Employee 
    {
        public string? Guid { get; set; }
        public string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleEmployee { get; set; }

        public virtual Vendor VendorNavigation { get; set; } = null!;
        public virtual ICollection<Role> RoleEmployes { get; set; }


        public string GetFullName()
        {
            var fullname = Firstname + ' ' + Lastname;

            return fullname.Trim().Replace("  ", " ");
        }
    }


}
