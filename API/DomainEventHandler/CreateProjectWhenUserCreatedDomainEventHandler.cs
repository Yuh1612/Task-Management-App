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

        public CreateProjectWhenUserCreatedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateUserDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var currentProject = new Project("Main Project");
                currentProject.AddMember(notification.user, true);
                await _unitOfWork.projectRepository.InsertAsync(currentProject);
                currentProject.AddListTask(new ListTask("Planning"));
                currentProject.AddListTask(new ListTask("To-do"));
                currentProject.AddListTask(new ListTask("Doing"));
                await _unitOfWork.CommitTransaction(false);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}