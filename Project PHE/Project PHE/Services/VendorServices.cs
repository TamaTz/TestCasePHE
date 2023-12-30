using Project_PHE.Contracts;
using Project_PHE.DTOs.Company;
using Project_PHE.Entities;
using Project_PHE.Utilities.Enums;
using System.Transactions;

namespace Project_PHE.Services
{
    public class VendorServices
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public const string VendorRoleGuid = "158f7caf-D2AD-45ad-4c30-48db58db1641";

        public VendorServices(IVendorRepository vendorRepository, IEmployeeRepository employeeRepository)
        {
            _vendorRepository = vendorRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<GetVendorWithEmployeeDto> GetAllVendors()
        {
            var vendors = _vendorRepository.GetAll();

            if (!vendors.Any()) return Enumerable.Empty<GetVendorWithEmployeeDto>();

            List<GetVendorWithEmployeeDto> getVendorDtos = new();

            foreach (var vendor in vendors)
            {
                string employeeFullName = string.Empty;

                if (vendor.EmployeeVendor != null)
                {
                    var employee = _employeeRepository.GetByGuid(vendor.EmployeeVendor); 
                    employeeFullName = $"{employee?.Firstname} {employee?.Lastname}";
                }

                getVendorDtos.Add(new GetVendorWithEmployeeDto
                {
                    Guid = vendor.Guid,
                    NameCompany = vendor.NameCompany,
                    EmailCompany = vendor.EmailCompany,
                    PhoneCompany = vendor.PhoneCompany,
                    UploadImage = vendor.UploadImage,
                    IsApproved = vendor.IsApproved,
                    EmployeeVendor = vendor.EmployeeVendor,
                    EmployeeName = employeeFullName

                });
            }

            return getVendorDtos;
        }

        public VendorDto GetVendorByGuid(string guid)
        {
            var vendor = _vendorRepository.GetByGuid(guid);

            if (vendor == null) return null; // Vendor not found

            var vendorDto = new VendorDto
            {
                Guid = vendor.Guid,
                NameCompany = vendor.NameCompany,
                EmailCompany = vendor.EmailCompany,
                PhoneCompany = vendor.PhoneCompany,
                UploadImage = vendor.UploadImage,
                IsApproved = vendor.IsApproved,
                EmployeeVendor = vendor.EmployeeVendor
            };

            return vendorDto;
        }

        public VendorDto CreateVendor(VendorDto vendorDto)
        {
            var newVendor = new Vendor
            {
                Guid = Guid.NewGuid().ToString(), // Generate a new GUID for the vendor
                EmployeeVendor = vendorDto.EmployeeVendor,
                NameCompany = vendorDto.NameCompany,
                EmailCompany = vendorDto.EmailCompany,
                PhoneCompany = vendorDto.PhoneCompany,
                UploadImage = vendorDto.UploadImage,
                IsApproved = IsApprovedStatus.pending.ToString(), // Set default approval status to Pending
            };

            var createdVendor = _vendorRepository.Create(newVendor);

            if (createdVendor == null) return null; // Failed to create vendor

            var createdVendorDto = new VendorDto
            {
                Guid = createdVendor.Guid,
                EmployeeVendor = vendorDto.EmployeeVendor,
                NameCompany = createdVendor.NameCompany,
                EmailCompany = createdVendor.EmailCompany,
                PhoneCompany = createdVendor.PhoneCompany,
                UploadImage = createdVendor.UploadImage,
                IsApproved = IsApprovedStatus.pending.ToString(), // Set default approval status to Pending
            };

            return createdVendorDto;
        }

        


        public int VendorAccept(string guid)
        {
            using var transactionScope = new TransactionScope();

            var vendor = _vendorRepository.GetByGuid(guid);
            if (vendor == null) return -1 ;
            if (vendor.IsApproved == IsApprovedStatus.proccess.ToString()) return -3;
            if (vendor.IsApproved == IsApprovedStatus.decline.ToString()) return -4;

            vendor.IsApproved = IsApprovedStatus.proccess.ToString();
            var update = _vendorRepository.Update(vendor);
            if (!update) return 0;

            transactionScope.Complete();
            return 1;

        }

        public int VendorApprove(string guid)
        {
            using var transactionScope = new TransactionScope();

            var vendor = _vendorRepository.GetByGuid(guid);
            if (vendor == null) return -1;
            if (vendor.IsApproved == IsApprovedStatus.approve.ToString()) return -3;
            if (vendor.IsApproved == IsApprovedStatus.decline.ToString()) return -4;

            // Update the vendor's approval status
            vendor.IsApproved = IsApprovedStatus.approve.ToString();
            var updateVendor = _vendorRepository.Update(vendor);
            if (!updateVendor) return 0;

            // Update the corresponding Employee in the _employeeRepository
            var employee = _employeeRepository.GetByGuid(vendor.EmployeeVendor);

            if (employee != null)
            {
                // Check if the employee is already a vendor
                if (employee.RoleEmployee == VendorRoleGuid) return -5; // Employee is already a vendor

                // Get the current RoleEmployee value
                var currentRoleEmployee = employee.RoleEmployee;

                // Set the RoleEmployee to the predefined static value for "vendor"
                employee.RoleEmployee = VendorRoleGuid;

                // Update the Employee in the repository
                var updateEmployee = _employeeRepository.Update(employee);

                // Check if the update on Employee was successful
                if (!updateEmployee)
                {
                    // If the update fails, roll back the transaction
                    transactionScope.Dispose();
                    return 0;
                }

                // Log the previous RoleEmployee value
                Console.WriteLine(currentRoleEmployee);
            }

            transactionScope.Complete();
            return 1;
        }




        public bool UpdateVendor(string guid, VendorDto vendorDto)
        {
            var existingVendor = _vendorRepository.GetByGuid(guid);

            if (existingVendor == null) return false; // Vendor not found

            existingVendor.NameCompany = vendorDto.NameCompany;
            existingVendor.EmailCompany = vendorDto.EmailCompany;
            existingVendor.PhoneCompany = vendorDto.PhoneCompany;
            existingVendor.UploadImage = vendorDto.UploadImage;
            existingVendor.IsApproved = vendorDto.IsApproved;

            return _vendorRepository.Update(existingVendor);
        }

        public async Task<int> VendorDecline(string guid)
        {
            // Begin the transaction scope
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var vendor = _vendorRepository.GetByGuid(guid);

            if (vendor is null) return -1; //data not found
            if (vendor.IsApproved == IsApprovedStatus.approve.ToString()) return -4;
            if (vendor.IsApproved == IsApprovedStatus.decline.ToString()) return -5;

            //update status reject
            vendor.IsApproved = IsApprovedStatus.decline.ToString();
            var update = _vendorRepository.Update(vendor);
            if (!update) return 0; //failed update
         

            // If everything is successful, commit the transaction
            transactionScope.Complete();
            return 1;
        }

        public bool DeleteVendor(string guid)
        {
            var existingVendor = _vendorRepository.GetByGuid(guid);

            if (existingVendor == null) return false; // Vendor not found

            return _vendorRepository.Delete(existingVendor);
        }
    }
}
