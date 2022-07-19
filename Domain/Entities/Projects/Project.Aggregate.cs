using Domain.Base;
using Domain.Entities.Projects.Events;
using Domain.Entities.Users;

namespace Domain.Entities.Projects
{
    public partial class Project : IAggregateRoot
    {
        public Project(string name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            ProjectMembers = new HashSet<ProjectMember>();
            ListTasks = new HashSet<ListTask>();
        }

        public void Update(string? name, string? description = null)
        {
            Name = name ?? Name;
            Description = description ?? Description;
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

        public void AddMember(User member, bool isCreated = false)
        {
            var projectMember = ProjectMembers.FirstOrDefault(x => x.UserId == member.Id);
            if (projectMember != null)
            {
                projectMember.IsDelete = false;
                return;
            }
            projectMember = new ProjectMember(this, member, isCreated);
            ProjectMembers.Add(projectMember);
            member.AddProject(projectMember);
        }

        public void RemoveMember(User member)
        {
            var projectMember = ProjectMembers.First(p => p.UserId == member.Id);
            projectMember.IsDelete = true;
        }

        public bool HasMember(User member)
        {
            return ProjectMembers.Any(p => p.UserId == member.Id);
        }

        public bool HasListTask(string name)
        {
            return ListTasks.Any(l => l.Name == name);
        }

        public void AddListTask(ListTask listTask)
        {
            var currentListTask = new ListTask(listTask.Name, listTask.Color);

            ListTasks.Add(currentListTask);
        }

        public void RemoveListTask(ListTask listTask)
        {
            listTask.IsDelete = true;

            var removeEvent = new DeleteListTaskDomainEvent(listTask);
            AddEvent(removeEvent);
        }

        public void UpdateListTask(ListTask listTask)
        {
            var currentListTask = ListTasks.First(x => x.Id == listTask.Id);
            currentListTask.Update(listTask.Name, listTask.Color);
        }

        public void Delete()
        {
            IsDelete = true;
            foreach (var listTask in ListTasks)
            {
                RemoveListTask(listTask);
            }
            foreach (var projectmember in ProjectMembers)
            {
                projectmember.IsDelete = true;
            }
        }
    }
}