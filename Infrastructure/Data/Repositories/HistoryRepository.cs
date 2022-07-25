using Domain.Histories;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class HistoryRepository : GenericRepository<History>, IHistoryRepository
    {
        public HistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}