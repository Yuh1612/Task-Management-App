using Domain.Entities.Users;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }

        IProjectRepository projectRepository { get; }

        IListTaskRepository listTaskRepository { get; }

        IProjectMemberRepository projectMemberRepository { get; }

        ITaskRepository taskRepository { get; }

        ITodoRepository todoRepository { get; }

        IAttachmentRepository attachmentRepository { get; }

        ILabelRepository labelRepository { get; }

        IHistoryRepository historyRepository { get; }

        Task SaveChangeAsync();

        Task BeginTransaction();

        Task CommitTransaction(bool IsAuthorize = true);

        Task RollbackTransaction();
    }
}