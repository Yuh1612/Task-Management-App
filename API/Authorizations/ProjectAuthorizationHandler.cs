using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorizations
{
    public class ProjectAuthorizationHandler : AuthorizationHandler<HasUserRequirement, Project>
    {
        private readonly IUserRepository _userRepository;

        public ProjectAuthorizationHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasUserRequirement requirement, Project resource)
        {
            var userId = context.User.Claims.First(i => i.Type == "UserId").Value;
            var user = await _userRepository.FindAsync(Guid.Parse(userId));
            if (user == null) return;
            if (resource.HasOwner(user) || resource.HasMember(user))
            {
                context.Succeed(requirement);
            }
        }
    }
}