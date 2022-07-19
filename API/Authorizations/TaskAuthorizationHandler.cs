using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorizations
{
    public class TaskAuthorizationHandler : AuthorizationHandler<HasUserRequirement, Domain.Entities.Tasks.Task>
    {
        private readonly IUserRepository _userRepository;

        public TaskAuthorizationHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasUserRequirement requirement, Domain.Entities.Tasks.Task resource)
        {
            var userId = context.User.Claims.First(i => i.Type == "UserId").Value;
            var user = await _userRepository.FindAsync(Guid.Parse(userId));
            if (user == null) return;
            if (resource.ListTask.Project.HasOwner(user) || resource.ListTask.Project.HasMember(user))
            {
                context.Succeed(requirement);
            }
        }
    }
}