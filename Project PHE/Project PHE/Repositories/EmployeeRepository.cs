using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.Entities;

namespace Project_PHE.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PheDbContext context) : base(context)
        {
        }

    }
}
