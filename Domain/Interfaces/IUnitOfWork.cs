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

        Task SaveChangeAsync();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();
    }
}