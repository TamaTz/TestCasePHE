using Project_PHE.Entities;

namespace Project_PHE.DTOs.Approvel
{
    public class ApprovelDto
    {
        public string Guid { get; set; }
        public string ApprovalVendor {  get; set; }

        public static implicit operator Entities.Approvel(ApprovelDto dto)
        {
            return new Entities.Approvel
            {
                Guid = dto.Guid,
                ApprovalVendor = dto.ApprovalVendor,
            };

        }
        public static explicit operator ApprovelDto(Entities.Approvel approvel)
        {
            return new ApprovelDto
            {
                Guid = approvel.Guid,
                ApprovalVendor = approvel.ApprovalVendor,
            };
        }
    }
}
