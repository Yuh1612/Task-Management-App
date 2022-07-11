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
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;

            var addEvent = new CreateProjectDomainEvent(user, this);
            AddEvent(addEvent);
        }

        public void Update(User user, string? name, string? description = null)
        {
            Name = name ?? Name;
            Description = description ?? Description;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }

        public bool HasOwner(User user)
        {
            return ProjectMembers.Any(p => p.UserId == user.Id && p.IsCreated == true);
        }

        public bool HasName(string? name)
        {
            if (name == null) return false;
            return Name == name;
        }

        public void AddMember(User user, bool isCreated = false)
        {
            var projectMember = new ProjectMember(this, user, isCreated);
            ProjectMembers.Add(projectMember);
            user.AddProject(projectMember);
        }

        public void RemoveMember(User user)
        {
            if (HasOwner(user) == true) throw new ArgumentException(nameof(user));
            var projectMember = ProjectMembers.FirstOrDefault(p => p.UserId == user.Id);
            ProjectMembers.Remove(projectMember);
        }

        public bool HasMember(User user)
        {
            return ProjectMembers.Any(p => p.UserId == user.Id);
        }

        public bool HasListTask(string name)
        {
            return ListTasks.Any(l => l.Name == name);
        }
    }
}