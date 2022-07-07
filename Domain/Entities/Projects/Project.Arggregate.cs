using Domain.Base;
using Domain.Entities.ListTasks;
using Domain.Entities.Projects.Events;
using Domain.Entities.Users;

namespace Domain.Entities.Projects
{
    public partial class Project : IAggregateRoot
    {
        public Project(User user, string name, string? description = null)
        {
            Name = name;
            Description = description;
            ProjectMembers = new HashSet<ProjectMember>();
            ListTasks = new HashSet<ListTask>();
            CreateDate = DateTime.UtcNow;
            CreatedById = user.Id;

            var addEvent = new CreateProjectDomainEvent(user, this);
            AddEvent(addEvent);
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = Id;
        }

        public bool IsThisUserCreated(User user)
        {
            return ProjectMembers.Where(p => p.UserId == user.Id && p.IsCreated == true).Any();
        }

        public bool IsListTaskExist(string name)
        {
            return ListTasks.Any(l => l.Name == name);
        }
    }
}