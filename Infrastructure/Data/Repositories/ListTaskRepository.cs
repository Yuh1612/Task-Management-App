using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ListTaskRepository : GenericRepository<ListTask>, IListTaskRepository
    {
        public ListTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}