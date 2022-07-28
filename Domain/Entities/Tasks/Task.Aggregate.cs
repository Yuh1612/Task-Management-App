using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Entities.Tasks
{
    public partial class Task : IAggregateRoot
    {
        public Task(ListTask listTask, string name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            ListTask = listTask;
            Todos = new HashSet<Todo>();
            TaskMembers = new HashSet<TaskMember>();
            Attachments = new HashSet<Attachment>();
            Labels = new HashSet<Label>();
        }

        public void Update(string? name, string? description)
        {
            Name = name ?? Name;
            Description = description ?? Description;
        }

        public void AddTodo(string name, string? description = null, Guid? ParentId = null)
        {
            Todos.Add(new Todo
            {
                Name = name,
                Description = description,
                ParentId = ParentId,
            });
        }

        public void RemoveTodo(Todo todo)
        {
            todo.IsDelete = true;
        }

        public bool HasMember(Guid userId)
        {
            return TaskMembers.Any(x => x.UserId == userId);
        }

        public void AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);
        }

        public void RemoveAttachment(Attachment attachment)
        {
            attachment.IsDelete = true;
        }

        public void AddMember(User user, bool isActive = false)
        {
            var taskMember = new TaskMember(this, user, isActive);
            TaskMembers.Add(taskMember);
            user.AddTask(taskMember);
        }

        public void RemoveMember(User user)
        {
            var taskMember = TaskMembers.First(x => x.UserId == user.Id);
            taskMember.IsDelete = true;
        }

        public void ActiveMember(Guid userId)
        {
            foreach (var taskmember in TaskMembers)
            {
                taskmember.IsActive = false;
                if (taskmember.UserId == userId) taskmember.IsActive = true;
            }
        }

        public bool HasLabel(Label label)
        {
            return Labels.Any(x => x.Id == label.Id);
        }

        public void AddLabel(Label label)
        {
            Labels.Add(label);
        }

        public void RemoveLabel(Label label)
        {
            Labels.Remove(label);
        }

        public void Delete()
        {
            IsDelete = true;
            foreach (var taskMember in TaskMembers)
            {
                taskMember.IsDelete = true;
            }
            foreach (var todo in Todos)
            {
                todo.IsDelete = true;
            }
            foreach (var attachment in Attachments)
            {
                attachment.IsDelete = true;
            }
            foreach (var label in Labels)
            {
                RemoveLabel(label);
            }
        }
    }
}