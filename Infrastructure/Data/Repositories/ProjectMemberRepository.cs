using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class ProjectMemberRepository : GenericRepository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}