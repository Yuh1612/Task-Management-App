using Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectMemberRepository : IGenericRepository<ProjectMember>
    {
        public Task<bool> IsProjectExist(int userId, int projectId);

        public Task<bool> IsProjectExist(int userId, string? projectName);
    }
}