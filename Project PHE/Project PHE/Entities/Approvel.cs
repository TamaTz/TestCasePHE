namespace Project_PHE.Entities
{
    public class Approvel 
    {
        public string Guid { get; set; }
        public string ApprovalVendor { get; set; }

        public virtual Vendor VendorNavigation { get; set; } = null!;

    }
}
