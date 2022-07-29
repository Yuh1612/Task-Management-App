using Domain.Entities.Projects.Events;
using Domain.Interfaces;
using MediatR;

namespace API.DomainEventHandler
{
    public class DeleteTaskWhenProjectDeletedDomainEventHandler : INotificationHandler<DeleteProjectDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskWhenProjectDeletedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProjectDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                foreach (var ListTask in notification.project.ListTasks)
                {
                    foreach (var task in ListTask.Tasks)
                    {
                        task.Delete();
                    }
                }
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
            }
        }
    }
}