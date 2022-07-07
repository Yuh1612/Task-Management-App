using Domain.Entities.ListTasks;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class ListTaskRepository : GenericRepository<ListTask>, IListTaskRepository
    {
        public ListTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}