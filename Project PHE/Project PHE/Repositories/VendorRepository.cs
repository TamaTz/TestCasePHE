using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.Entities;

namespace Project_PHE.Repositories
{
    public class VendorRepository : GeneralRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(PheDbContext context) : base(context)
        {
        }
    }
}
