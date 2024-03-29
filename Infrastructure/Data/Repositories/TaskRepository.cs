﻿using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Entities.Tasks.Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Domain.Entities.Tasks.Task> GetOneByAttachment(Guid attachmentId)
        {
            var task = await dbSet.FirstOrDefaultAsync(t => t.Attachments.Any(t => t.Id == attachmentId) == true);
            if (task == null) throw new KeyNotFoundException(nameof(task));
            return task;
        }

        public async Task<Domain.Entities.Tasks.Task> GetOneByTodo(Guid todoId)
        {
            var task = await dbSet.FirstOrDefaultAsync(t => t.Todos.Any(t => t.Id == todoId) == true);
            if (task == null) throw new KeyNotFoundException(nameof(task));
            return task;
        }

        public async Task<Domain.Entities.Tasks.Task?> GetTask(Guid taskId, Guid userId)
        {
            var task = await dbSet.FirstOrDefaultAsync(c => c.Id == taskId);
            if (task != null)
            {
                return task.ListTask.Project.ProjectMembers.Any(c => c.UserId == userId) ? task : null;
            }
            return task;
        }
    }
}