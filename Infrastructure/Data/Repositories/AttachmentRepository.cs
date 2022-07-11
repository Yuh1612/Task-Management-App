using Domain.Entities.Tasks;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Data.Repositories
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}