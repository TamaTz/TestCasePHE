using Project_PHE.Entities;
using Project_PHE.Utilities.Enums;
using System.Numerics;

namespace Project_PHE.DTOs.Company
{
    public class VendorDto
    {
        public string? Guid { get; set; }
        public string NameCompany { get; set; }
        public string EmailCompany { get; set; }
        public string PhoneCompany { get; set; }
        public byte[]? UploadImage { get; set; }
        public string? IsApproved { get; set; }  
        public string EmployeeVendor { get ; set; }

        // Implicit conversion from VendorDto to Vendor
        public static implicit operator Vendor(VendorDto vendorDto)
        {
            return new Vendor
            {
                Guid = vendorDto.Guid,
                NameCompany = vendorDto.NameCompany,
                EmailCompany = vendorDto.EmailCompany,
                PhoneCompany = vendorDto.PhoneCompany,
                UploadImage = vendorDto.UploadImage,
                IsApproved = vendorDto.IsApproved,
                EmployeeVendor = vendorDto.EmployeeVendor
        };
        }

        // Explicit conversion from Vendor to VendorDto
        public static explicit operator VendorDto(Vendor vendor)
        {
            return new VendorDto
            {
                Guid = vendor.Guid,
                NameCompany = vendor.NameCompany,
                EmailCompany = vendor.EmailCompany,
                PhoneCompany = vendor.PhoneCompany,
                UploadImage = vendor.UploadImage,
                IsApproved = vendor.IsApproved,
                EmployeeVendor = vendor.EmployeeNavigation?.GetFullName()
            };
        }
    }
}
