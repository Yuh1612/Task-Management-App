using Domain.Base;
using Domain.Entities.Projects.Events;

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

        public void AddMember(Guid memberId, bool isCreated = false)
        {
            var projectMember = ProjectMembers.FirstOrDefault(x => x.UserId == memberId);
            if (projectMember != null)
            {
                projectMember.IsDelete = false;
                return;
            }
            projectMember = new ProjectMember(Id, memberId, isCreated);
            ProjectMembers.Add(projectMember);
        }

        public void RemoveMember(Guid memberId)
        {
            var projectMember = ProjectMembers.First(p => p.UserId == memberId);
            projectMember.IsDelete = true;
        }

        public bool HasMember(Guid memberId)
        {
            return ProjectMembers.Any(p => p.UserId == memberId);
        }

        public void AddListTask(ListTask listTask)
        {
            var currentListTask = new ListTask(listTask.Name, listTask.Color);

            ListTasks.Add(currentListTask);
        }

        public void RemoveListTask(ListTask listTask)
        {
            listTask.IsDelete = true;
            AddEvent(new DeleteListTaskDomainEvent(listTask));
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
                listTask.IsDelete = true;
            }
            foreach (var projectmember in ProjectMembers)
            {
                projectmember.IsDelete = true;
            }

            AddEvent(new DeleteProjectDomainEvent(this));
        }
    }
}