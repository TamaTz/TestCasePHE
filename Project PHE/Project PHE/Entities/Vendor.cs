
namespace Project_PHE.Entities
{
    public class Vendor
    {
        public Vendor()
        {
        }
        public string? Guid { get; set; } = null!;
        public string NameCompany { get; set; }
        public string EmailCompany { get; set; }
        public string PhoneCompany { get; set; }
        public byte[]? UploadImage { get; set; }
        public string? IsApproved { get; set; }
        public string EmployeeVendor { get; set; }

        public virtual Employee EmployeeNavigation { get; set; } = null!;

        public virtual Approvel ApprovelNavigation { get; set; } = null!;


    }
}
