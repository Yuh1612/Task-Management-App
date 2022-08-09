using API.IntegrationEvents.Events;
using Domain.Entities.Users;
using Domain.Interfaces;
using EventBus.Abstractions;

namespace API.IntegrationEvents.EventHandling
{
    public class UserCreatedIntergrationEventHandler : IIntegrationEventHandler<UserCreatedIntergrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserCreatedIntergrationEventHandler> _logger;

        public UserCreatedIntergrationEventHandler(ILogger<UserCreatedIntergrationEventHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UserCreatedIntergrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = new User(@event.Id, @event.UserName, @event.Email);
                user.AddCreateUserDomainEvent();
                await _unitOfWork.userRepository.InsertAsync(user);
                await _unitOfWork.CommitTransaction(false);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(ex.Message);
            }
        }
    }
}