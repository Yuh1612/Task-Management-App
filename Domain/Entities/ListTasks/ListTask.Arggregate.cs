using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Entities.ListTasks
{
    public partial class ListTask
    {
        public ListTask(User user, Project project, string name, string? color = null)
        {
            Name = name;
            Color = color;
            Tasks = new HashSet<Tasks.Task>();
            Project = project;
            CreateDate = DateTime.UtcNow;
            CreatedById = user.Id;
        }

        public void Update(User user, string? name, string? color)
        {
            Name = name ?? Name;
            Color = color ?? Color;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = user.Id;
        }
    }
}