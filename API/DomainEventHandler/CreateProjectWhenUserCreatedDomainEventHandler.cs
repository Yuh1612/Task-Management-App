using Domain.Entities.Projects;
using Domain.Entities.Users.Events;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;
using MediatR;

namespace API.DomainEventHandler
{
    public class CreateProjectWhenUserCreatedDomainEventHandler : INotificationHandler<CreateUserDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IProjectDomainService _projectDomainService;

        public CreateProjectWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork, IProjectDomainService projectDomainService)
        {
            _unitOfWork = unitOfWork;
            _projectDomainService = projectDomainService;
        }

        public async Task Handle(CreateUserDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var project = new Project(notification.user, "Main Project");
                _projectDomainService.AddMemberToProject(notification.user, project, true);
                await _unitOfWork.projectRepository.InsertAsync(project);

                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}