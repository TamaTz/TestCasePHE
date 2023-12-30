using Project_PHE.Contracts;
using Project_PHE.DTOs.Role;
using Project_PHE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_PHE.Services
{
    public class RoleServices
    {
        private readonly IRoleRepository _roleRepository;

        public RoleServices(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public IEnumerable<RoleDto> GetAllRoles()
        {
            var roles = _roleRepository.GetAll();

            if (!roles.Any()) return Enumerable.Empty<RoleDto>();

            var roleDtos = roles.Select(role => new RoleDto
            {
                Guid = role.Guid,
                Name = role.Name,
            });

            return roleDtos;
        }

        public RoleDto GetRoleByGuid(string guid)
        {
            var role = _roleRepository.GetByGuid(guid);

            if (role == null) return null;

            var roleDto = new RoleDto
            {
                Guid = role.Guid,
                Name = role.Name,
            };

            return roleDto;
        }

        public RoleDto CreateRole(RoleDto roleDto)
        {
            var newRole = new Role
            {
                Guid = Guid.NewGuid().ToString(),
                Name = roleDto.Name,
            };

            var createdRole = _roleRepository.Create(newRole);

            if (createdRole == null) return null;

            var createdRoleDto = new RoleDto
            {
                Guid = createdRole.Guid,
                Name = createdRole.Name,
            };

            return createdRoleDto;
        }

        public bool UpdateRole(string guid, RoleDto roleDto)
        {
            var existingRole = _roleRepository.GetByGuid(guid);

            if (existingRole == null) return false;

            existingRole.Name = roleDto.Name;

            return _roleRepository.Update(existingRole);
        }

        public bool DeleteRole(string guid)
        {
            var existingRole = _roleRepository.GetByGuid(guid);

            if (existingRole == null) return false;

            return _roleRepository.Delete(existingRole);
        }
    }
}
