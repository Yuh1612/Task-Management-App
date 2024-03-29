﻿namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<Entities.Tasks.Task>
    {
        Task<Entities.Tasks.Task> GetOneByTodo(Guid todoId);

        Task<Entities.Tasks.Task> GetOneByAttachment(Guid attachmentId);

        Task<Entities.Tasks.Task?> GetTask(Guid taskId, Guid userId);
    }
}