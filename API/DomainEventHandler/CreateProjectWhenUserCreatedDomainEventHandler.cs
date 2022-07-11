using Domain.Entities.Projects;
using Domain.Entities.Users.Events;
using Domain.Interfaces;
using MediatR;

namespace API.DomainEventHandler
{
    public class CreateProjectWhenUserCreatedDomainEventHandler : INotificationHandler<CreateUserDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateUserDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var project = new Project(notification.user, "Main Project");
                project.AddMember(notification.user, true);
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