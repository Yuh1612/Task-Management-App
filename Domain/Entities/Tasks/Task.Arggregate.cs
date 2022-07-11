using Domain.Base;
using Domain.Entities.Labels;
using Domain.Entities.ListTasks;
using Domain.Entities.Users;

namespace Domain.Entities.Tasks
{
    public partial class Task : IAggregateRoot
    {
        public Task(User user, ListTask listTask, string name, string? description = null)
        {
            Name = name;
            Description = description;
            ListTask = listTask;
            Todos = new HashSet<Todo>();
            TaskMembers = new HashSet<TaskMember>();
            Attachments = new HashSet<Attachment>();
            Labels = new HashSet<Label>();
            CreateDate = DateTime.UtcNow;
            CreatedById = user.Id;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public void Update(User user, string? name, string? description)
        {
            Name = name ?? Name;
            Description = description ?? Description;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public void AddTodo(User user, string name, string? description = null, int? ParentId = null)
        {
            Todos.Add(new Todo
            {
                Name = name,
                Description = description,
                ParentId = ParentId,
                CreateDate = DateTime.UtcNow,
                CreatedById = user.Id,
                UpdateDate = DateTime.UtcNow,
                UpdatedById = user.Id
            });
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public bool HasTodo(int todoId)
        {
            return Todos.Any(x => x.Id == todoId);
        }

        public void RemoveTodo(User user, int todoId)
        {
            Todos.Remove(Todos.First(t => t.Id == todoId));
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public bool HasOwner(User user)
        {
            return ListTask.HasOwner(user);
        }

        public bool HasMember(User user)
        {
            return TaskMembers.Any(x => x.UserId == user.Id);
        }

        public void AddAttachment(User user, string fileName, string type, string storageUrl)
        {
            var attachment = new Attachment(fileName, type, storageUrl);
            Attachments.Add(attachment);
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public void RemoveAttachment(User user, int attachmentId)
        {
            var attachment = Attachments.FirstOrDefault(x => x.Id == attachmentId);
            if (attachment == null) throw new KeyNotFoundException(nameof(attachment));
            Attachments.Remove(attachment);
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public void AddMember(User user, bool isActive = false)
        {
            var taskMember = new TaskMember(this, user, isActive);
            TaskMembers.Add(taskMember);
            user.AddTask(taskMember);
        }

        public void RemoveMember(User user)
        {
            var taskMember = TaskMembers.FirstOrDefault(x => x.UserId == user.Id);
            TaskMembers.Remove(taskMember);
        }

        public void ActiveMember(User user)
        {
            foreach (var taskmember in TaskMembers)
            {
                taskmember.IsActive = false;
                if (taskmember.UserId == user.Id) taskmember.IsActive = true;
            }
        }
    }
}