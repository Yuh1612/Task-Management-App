using Domain.Entities.Projects.Events;
using Domain.Interfaces;
using MediatR;

namespace API.DomainEventHandler
{
    public class DeleteTaskWhenListTaskDeletedDomainEventHandler : INotificationHandler<DeleteListTaskDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskWhenListTaskDeletedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteListTaskDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                foreach (var task in notification.listTask.Tasks)
                {
                    task.Delete();
                    _unitOfWork.taskRepository.Update(task);
                }
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