using Domain.Entities.Tasks;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class LabelRepository : GenericRepository<Label>, ILabelRepository
    {
        public LabelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}