using Domain.Entities.ListTasks;
using Domain.Entities.Projects.Events;
using Domain.Interfaces;
using MediatR;

namespace API.DomainEventHandler
{
    public class CreateListTaskWhenProjectCreatedDomainEventHandler : INotificationHandler<CreateProjectDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateListTaskWhenProjectCreatedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateProjectDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var listtask = new ListTask(notification.user, notification.project, "Main List");
                await _unitOfWork.listTaskRepository.InsertAsync(listtask);
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