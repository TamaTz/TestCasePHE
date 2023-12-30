namespace Project_PHE.DTOs.Role
{
    public class RoleDto
    {
        public string? Guid { get; set; }
        public string Name { get; set; }

        public static implicit operator Entities.Role(RoleDto roleDto)
        {
            return new Entities.Role
            {
                Guid = roleDto.Guid,
                Name = roleDto.Name,
            };
        }

        public static explicit operator RoleDto(Entities.Role role) {
            return new RoleDto
            {
                Guid = role.Guid,
                Name = role.Name,
            };
        }

    }
}
