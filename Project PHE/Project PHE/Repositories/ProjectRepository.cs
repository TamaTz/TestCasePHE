using Project_PHE.Contracts;
using Project_PHE.Data;
using Project_PHE.Entities;

namespace Project_PHE.Repositories
{
    public class ProjectRepository : GeneralRepository<Project>, IProjectRepository
    {
       public ProjectRepository(PheDbContext context) : base(context)
       {
       }

    }
}
