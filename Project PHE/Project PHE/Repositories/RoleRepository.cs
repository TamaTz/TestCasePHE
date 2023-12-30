using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.Entities;

namespace Project_PHE.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(PheDbContext context) : base(context)
        {
        }
    }
}
