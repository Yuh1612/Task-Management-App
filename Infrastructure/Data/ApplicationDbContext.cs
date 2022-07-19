using Domain.Entities.Projects;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Tasks;
using MediatR;
using Domain.Histories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApplicationDbContext(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ListTask> ListTasks { get; set; }

        public DbSet<Domain.Entities.Tasks.Task> Tasks { get; set; }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public DbSet<TaskMember> TaskMembers { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<History> Histories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=VT-PGH;Database=TaskManagementDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Label>().HasData(
                new Label { Id = Guid.NewGuid(), Title = ".NET", Color = "Purple" },
                new Label { Id = Guid.NewGuid(), Title = "Java", Color = "Blue" },
                new Label { Id = Guid.NewGuid(), Title = "NestJs", Color = "Green" },
                new Label { Id = Guid.NewGuid(), Title = "ReactJs", Color = "Black" },
                new Label { Id = Guid.NewGuid(), Title = "Angular", Color = "Red" },
                new Label { Id = Guid.NewGuid(), Title = "Python", Color = "Orange" },
                new Label { Id = Guid.NewGuid(), Title = "VueJs", Color = "White" },
                new Label { Id = Guid.NewGuid(), Title = "Flutter", Color = "Black" },
                new Label { Id = Guid.NewGuid(), Title = "Dart", Color = "Gray" },
                new Label { Id = Guid.NewGuid(), Title = "Golang", Color = "Brown" },
                new Label { Id = Guid.NewGuid(), Title = "PHP", Color = "Pink" }
                );

            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<ListTask>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<ProjectMember>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<TaskMember>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Attachment>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Label>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Todo>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Domain.Entities.Tasks.Task>().HasQueryFilter(p => !p.IsDelete);
        }

        public async System.Threading.Tasks.Task SaveEntitiesAsync(bool IsAuthorize = true)
        {
            OnBeforeSaveChanges(IsAuthorize);
            await SaveChangesAsync();
            await _mediator.DispatchDomainEventsAsync(this);
        }

        private void OnBeforeSaveChanges(bool IsAuthorize = true)
        {
            ChangeTracker.DetectChanges();
            var historyEntries = new List<HistoryEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is History || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var historyEntry = new HistoryEntry(entry);
                historyEntry.Ref = entry.Entity.GetType().Name;
                if (IsAuthorize == true)
                {
                    var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId");
                    if (userId != null) historyEntry.UserId = Guid.Parse(userId.Value);
                }
                historyEntries.Add(historyEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        historyEntry.RefId[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            historyEntry.ActionType = ActionType.Create;
                            historyEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            historyEntry.ActionType = ActionType.Delete;
                            historyEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                historyEntry.ChangedColumns.Add(propertyName);
                                historyEntry.ActionType = ActionType.Update;
                                historyEntry.OldValues[propertyName] = property.OriginalValue;
                                historyEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var historyEntry in historyEntries)
            {
                Histories.Add(historyEntry.ToHistory());
            }
        }
    }
}