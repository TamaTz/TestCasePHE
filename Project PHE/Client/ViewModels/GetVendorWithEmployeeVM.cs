namespace Client.ViewModels
{
    public class GetVendorWithEmployeeVM
    {
        public string? Guid { get; set; }
        public string NameCompany { get; set; }
        public string EmailCompany { get; set; }
        public string PhoneCompany { get; set; }
        public byte[]? UploadImage { get; set; }
        public string? IsApproved { get; set; }
        public string EmployeeVendor { get; set; }
        public string EmployeeName { get; set; }
    }
}
