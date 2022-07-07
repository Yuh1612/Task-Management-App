using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ApplicationController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApplicationController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected int? GetCurrentUserId()
        {
            var userId = _contextAccessor.HttpContext.User.Claims.First(i => i.Type == "UserId").Value;
            return userId == null ? null : Int32.Parse(userId);
        }
    }
}